using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Vuforia;
public class TrackEj6 : MonoBehaviour
{
    #region PROTECTED_MEMBER_VARIABLES
    public enum TrackingStatusFilter
    {
        Tracked, Tracked_ExtendedTracked, Tracked_ExtendedTracked_Limited
    }

    public TrackingStatusFilter StatusFilter = TrackingStatusFilter.Tracked_ExtendedTracked_Limited;
    public bool UsePoseSmoothing = false;
    public AnimationCurve AnimationCurve = AnimationCurve.Linear(0, 0, LERP_DURATION, 1);
    public UnityEvent OnTargetFound = new();
    public UnityEvent OnTargetLost = new();
    protected ObserverBehaviour mObserverBehaviour;
    protected TargetStatus mPreviousTargetStatus = TargetStatus.NotObserved;
    protected bool mCallbackReceivedOnce;
    const float LERP_DURATION = 0.3f;
    PoseSmoother mPoseSmoother;
    #endregion // PROTECTED_MEMBER_VARIABLES

    #region UNITY_MONOBEHAVIOUR_METHODS

    protected virtual void Start()
    {
        mObserverBehaviour = GetComponent<ObserverBehaviour>();
        if (mObserverBehaviour)
        {
            mObserverBehaviour.OnTargetStatusChanged += OnObserverStatusChanged;
            mObserverBehaviour.OnBehaviourDestroyed += OnObserverDestroyed;
            OnObserverStatusChanged(mObserverBehaviour, mObserverBehaviour.TargetStatus);
            SetupPoseSmoothing();
        }
    }

    protected virtual void OnDestroy()
    {
        if (VuforiaBehaviour.Instance != null)
            VuforiaBehaviour.Instance.World.OnStateUpdated -= OnStateUpdated;

        if (mObserverBehaviour)
            OnObserverDestroyed(mObserverBehaviour);
        mPoseSmoother?.Dispose();
    }

    void OnObserverDestroyed(ObserverBehaviour observer)
    {
        mObserverBehaviour.OnTargetStatusChanged -= OnObserverStatusChanged;
        mObserverBehaviour.OnBehaviourDestroyed -= OnObserverDestroyed;
        mObserverBehaviour = null;
    }
    void OnObserverStatusChanged(ObserverBehaviour behaviour, TargetStatus targetStatus) {
        var name = mObserverBehaviour.TargetName;
        if (mObserverBehaviour is VuMarkBehaviour vuMarkBehaviour && vuMarkBehaviour.InstanceId != null){
            name += " (" + vuMarkBehaviour.InstanceId + ")";
        }
        Debug.Log($"Target status: {name} {targetStatus.Status} -- {targetStatus.StatusInfo}");
        HandleTargetStatusChanged(mPreviousTargetStatus.Status, targetStatus.Status);
        HandleTargetStatusInfoChanged(targetStatus.StatusInfo);

        mPreviousTargetStatus = targetStatus;
    }

    protected virtual void HandleTargetStatusChanged(Status previousStatus, Status newStatus)
    {
        var shouldBeRendererBefore = ShouldBeRendered(previousStatus);
        var shouldBeRendererNow = ShouldBeRendered(newStatus);
        if (shouldBeRendererBefore != shouldBeRendererNow)
        {
            if (shouldBeRendererNow)
            {
                OnTrackingFound();
                if (GameObject.Find("MainMenu").GetComponent<RectTransform>().localScale == Vector3.one)
                {
                    Button btn = GameObject.Find("btnCompos").GetComponent<Button>();
                    btn.onClick.Invoke();
                }
            }
            else
            {
                OnTrackingLost();
            }
        }
        else
        {
            if (!mCallbackReceivedOnce && !shouldBeRendererNow) {
                OnTrackingLost(); // This is the first time we are receiving this callback, and the target is not visible yet.--> Hide the augmentation.
            }
        }
        mCallbackReceivedOnce = true;
    }

    protected virtual void HandleTargetStatusInfoChanged(StatusInfo newStatusInfo) {
        if (newStatusInfo == StatusInfo.WRONG_SCALE) {
            Debug.LogErrorFormat("The target {0} appears to be scaled incorrectly. This might result in tracking issues. Please make sure that the target size corresponds " +
                "to the size of the physical object in meters and regenerate the target or set the correct size in the target's inspector.", mObserverBehaviour.TargetName);
        }
    }
    #endregion // UNITY_MONOBEHAVIOUR_METHODS

    protected bool ShouldBeRendered(Status status) {
        if (status == Status.TRACKED) {
            return true;
        }

        if (StatusFilter == TrackingStatusFilter.Tracked_ExtendedTracked && status == Status.EXTENDED_TRACKED) {
            
            return true;
        }

        if (StatusFilter == TrackingStatusFilter.Tracked_ExtendedTracked_Limited && (status == Status.EXTENDED_TRACKED || status == Status.LIMITED)) {
            // in this mode, render the augmentation even if the target's tracking status is LIMITED. this is mainly recommended for Anchors.
            return true;
        }
        return false;
    }

    /// <summary> Implementation of the ITrackableEventHandler function called when the tracking state changes. </summary>
    #region PROTECTED_METHODS

    protected virtual void OnTrackingFound() {
        if (mObserverBehaviour)
            SetComponentsEnabled(true);
        OnTargetFound?.Invoke();
    }

    protected virtual void OnTrackingLost() {
        if (mObserverBehaviour)
            SetComponentsEnabled(false);
        OnTargetLost?.Invoke();
    }

    protected void SetupPoseSmoothing() {
        UsePoseSmoothing &= VuforiaBehaviour.Instance.WorldCenterMode == WorldCenterMode.DEVICE; // pose smoothing only works with the DEVICE world center mode
        mPoseSmoother = new PoseSmoother(mObserverBehaviour, AnimationCurve);
        VuforiaBehaviour.Instance.World.OnStateUpdated += OnStateUpdated;
    }

    void OnStateUpdated() {
        if (enabled && UsePoseSmoothing)
            mPoseSmoother.Update();
    }

    void SetComponentsEnabled(bool enable) {
        var components = VuforiaRuntimeUtilities.GetComponentsInChildrenExcluding<Component, DefaultObserverEventHandler>(gameObject);
        foreach (var component in components) {
            switch (component)  {
                case Renderer rendererComponent:
                    rendererComponent.enabled = enable;
                    break;
                case Collider colliderComponent:
                    colliderComponent.enabled = enable;
                    break;
                case Canvas canvasComponent:
                    canvasComponent.enabled = enable;
                    break;
                case RuntimeMeshRenderingBehaviour runtimeMeshComponent:
                    runtimeMeshComponent.enabled = enable;
                    break;
            }
        }
    }
    #endregion // PROTECTED_METHODS
    
    class PoseSmoother
    {
        const float e = 0.001f;
        const float MIN_ANGLE = 2f;

        PoseLerp mActivePoseLerp;
        Pose mPreviousPose;

        readonly ObserverBehaviour mTarget;
        readonly AnimationCurve mAnimationCurve;

        TargetStatus mPreviousStatus;

        public PoseSmoother(ObserverBehaviour target, AnimationCurve animationCurve)
        {
            mTarget = target;
            mAnimationCurve = animationCurve;
        }

        public void Update()
        {
            var currentPose = new Pose(mTarget.transform.position, mTarget.transform.rotation);
            var currentStatus = mTarget.TargetStatus;

            UpdatePoseSmoothing(currentPose, currentStatus);

            mPreviousPose = currentPose;
            mPreviousStatus = currentStatus;
        }

        void UpdatePoseSmoothing(Pose currentPose, TargetStatus currentTargetStatus)
        {
            if (mActivePoseLerp == null && ShouldSmooth(currentPose, currentTargetStatus))
            {
                mActivePoseLerp = new PoseLerp(mPreviousPose, currentPose, mAnimationCurve);
            }

            if (mActivePoseLerp != null)
            {
                var pose = mActivePoseLerp.GetSmoothedPosition(Time.deltaTime);
                mTarget.transform.SetPositionAndRotation(pose.position, pose.rotation);

                if (mActivePoseLerp.Complete)
                {
                    mActivePoseLerp = null;
                }
            }
        }

        /// Smooth pose transition if the pose changed and the target is still being reported as "extended tracked" or it has just returned to
        /// "tracked" from previously being "extended tracked"
        bool ShouldSmooth(Pose currentPose, TargetStatus currentTargetStatus)
        {
            return (currentTargetStatus.Status == Status.EXTENDED_TRACKED || (currentTargetStatus.Status == Status.TRACKED && mPreviousStatus.Status == Status.EXTENDED_TRACKED)) &&
                   (Vector3.SqrMagnitude(currentPose.position - mPreviousPose.position) > e || Quaternion.Angle(currentPose.rotation, mPreviousPose.rotation) > MIN_ANGLE);
        }

        public void Dispose()
        {
            mActivePoseLerp = null;
        }
    }

    class PoseLerp
    {
        readonly AnimationCurve mCurve;
        readonly Pose mStartPose;
        readonly Pose mEndPose;
        readonly float mEndTime;

        float mElapsedTime;

        public bool Complete { get; private set; }

        public PoseLerp(Pose startPose, Pose endPose, AnimationCurve curve)
        {
            mStartPose = startPose;
            mEndPose = endPose;
            mCurve = curve;
            mEndTime = mCurve.keys[mCurve.length - 1].time;
        }

        public Pose GetSmoothedPosition(float deltaTime)
        {
            mElapsedTime += deltaTime;

            if (mElapsedTime >= mEndTime)
            {
                mElapsedTime = 0;
                Complete = true;
                return mEndPose;
            }

            var ratio = mCurve.Evaluate(mElapsedTime);
            var smoothPosition = Vector3.Lerp(mStartPose.position, mEndPose.position, ratio);
            var smoothRotation = Quaternion.Slerp(mStartPose.rotation, mEndPose.rotation, ratio);

            return new Pose(smoothPosition, smoothRotation);
        }
    }
}