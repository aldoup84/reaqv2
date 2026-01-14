using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Vuforia;

public class TrackInfo : MonoBehaviour {
    #region PROTECTED_MEMBER_VARIABLES
    public enum TrackingStatusFilter {
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

    Text txtTarget;
    TextMeshProUGUI txtElem1, txtElem2, txtElem3, txtInfo;
    #endregion // PROTECTED_MEMBER_VARIABLES
    Color[] colors;
    string[] elementos;

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

        txtElem1 = GameObject.Find("txtElem1").GetComponent<TextMeshProUGUI>();
        txtElem2 = GameObject.Find("txtElem2").GetComponent<TextMeshProUGUI>();
        txtElem3 = GameObject.Find("txtElem3").GetComponent<TextMeshProUGUI>();
        txtInfo = GameObject.Find("txtInfo2").GetComponent<TextMeshProUGUI>();
        GetColors();
    }

    public void GetColors()
    {
        colors = new Color[10];
        elementos = new string[10];
        colors[0] = new Color(0.75f, 1.00f, 0.96f, 1.00f);
        colors[1] = new Color(1.00f, 0.71f, 1.00f, 1.00f);
        colors[2] = new Color(0.79f, 1.00f, 0.46f, 1.00f);
        colors[3] = new Color(1.00f, 0.61f, 0.12f, 1.00f);
        colors[4] = new Color(0.04f, 0.76f, 0.12f, 1.00f);
        colors[5] = new Color(0.94f, 0.56f, 0.82f, 1.00f);
        colors[6] = new Color(0.98f, 1.00f, 0.27f, 1.00f);
        colors[7] = new Color(0.16f, 0.07f, 0.30f, 1.00f);
        colors[8] = new Color(0.43f, 0.07f, 0.07f, 1.00f);
        colors[9] = new Color(0.82f, 0.00f, 0.00f, 1.00f);

        elementos[6] = "Fe";
        elementos[7] = "O";
        elementos[9] = "H";
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
    void OnObserverStatusChanged(ObserverBehaviour behaviour, TargetStatus targetStatus)
    {
        var name = mObserverBehaviour.TargetName;
        if (mObserverBehaviour is VuMarkBehaviour vuMarkBehaviour && vuMarkBehaviour.InstanceId != null)
        {
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
            txtElem2.text = mObserverBehaviour.TargetName;
            if (shouldBeRendererNow)
            {
                OnTrackingFound();
                Debug.Log("Trackable " + mObserverBehaviour.TargetName + " found");
                GameObject.Find("Panel3").GetComponent<RectTransform>().localScale = Vector3.zero;
                string cmp = GameObject.Find("DropCompuesto").GetComponent<TMP_Dropdown>().captionText.text;
                GameObject.Find("txtNomComp").GetComponent<TextMeshProUGUI>().text = cmp;
                try
                {
                    Compuesto c = DatosEj1.Instance.GetInformacionCompuesto(cmp);
                    txtElem1.text = c.Elemento1;
                    txtElem2.text = c.Elemento2;
                    if (c.Elemento3.Equals("-"))
                    {
                        txtElem3.text = "";
                        GameObject.Find("txtElem3").GetComponent<RectTransform>().localScale = Vector3.zero;
                    }
                    else
                    {
                        txtElem3.text = c.Elemento3;
                        GameObject.Find("txtElem3").GetComponent<RectTransform>().localScale = Vector3.one;
                    }
                    txtInfo.text = c.Informacion;
                    GameObject.Find("Panel4").GetComponent<RectTransform>().localScale = Vector3.one;
                    StartCoroutine("VerPanel");
                }
                catch (Exception e)
                {
                    Debug.Log(e.Message);
                }
            }
            else
            {
                Debug.Log("Trackable " + mObserverBehaviour.TargetName + " lost");
                OnTrackingLost();
                GameObject.Find("MainMenu").GetComponent<RectTransform>().localScale = Vector3.one;
            }
        }
        else
        {
            if (!mCallbackReceivedOnce && !shouldBeRendererNow)
            {
                OnTrackingLost();
            }
        }
        mCallbackReceivedOnce = true;
    }

    #endregion // UNITY_MONOBEHAVIOUR_METHODS

    
 
    IEnumerator VerPanel()
    {
        yield return new WaitForSeconds(7);
        GameObject.Find("Panel4").GetComponent<RectTransform>().localScale = Vector3.zero;
        GameObject.Find("Panel3").GetComponent<RectTransform>().localScale = Vector3.one;
    }

    protected virtual void HandleTargetStatusInfoChanged(StatusInfo newStatusInfo)
    {
        if (newStatusInfo == StatusInfo.WRONG_SCALE)
        {
            Debug.LogErrorFormat("The target {0} appears to be scaled incorrectly. This might result in tracking issues. " +
                                 "Please make sure that the target size corresponds to the size of the " +
                                 "physical object in meters and regenerate the target or set the correct " +
                                 "size in the target's inspector.", mObserverBehaviour.TargetName);
        }
    }

    protected bool ShouldBeRendered(Status status)
    {
        if (status == Status.TRACKED)
        {
            return true;
        }

        if (StatusFilter == TrackingStatusFilter.Tracked_ExtendedTracked && status == Status.EXTENDED_TRACKED)
        {
            // also return true if the target is extended tracked
            return true;
        }

        if (StatusFilter == TrackingStatusFilter.Tracked_ExtendedTracked_Limited &&
            (status == Status.EXTENDED_TRACKED || status == Status.LIMITED))
        {
            // in this mode, render the augmentation even if the target's tracking status is LIMITED.
            // this is mainly recommended for Anchors.
            return true;
        }

        return false;
    }
 

    #region PUBLIC_METHODS

    /// <summary>  Implementation of the ITrackableEventHandler function called when the tracking state changes.
    /// </summary>


    #endregion // PUBLIC_METHODS
    protected virtual void OnTrackingFound()
    {
        if (mObserverBehaviour)
            SetComponentsEnabled(true);
        OnTargetFound?.Invoke();
        string fig = GameObject.Find("DropCompuesto").GetComponent<TMP_Dropdown>().captionText.text;
        GameObject figura = (GameObject)Instantiate(Resources.Load(fig, typeof(GameObject))) as GameObject;
        Transform f = GameObject.Find("Figura").GetComponent<Transform>();
        figura.GetComponent<Transform>().SetParent(f);
        figura.tag = "Clon";
        figura.GetComponent<Transform>().localPosition = new Vector3(0.5f, 0f, 0f);
        figura.GetComponent<Transform>().localScale = Vector3.one;

    }

    protected virtual void OnTrackingLost()
    {
        if (mObserverBehaviour)
            SetComponentsEnabled(false);
        OnTargetLost?.Invoke();
        var items = GameObject.FindGameObjectsWithTag("Clon");
        foreach (var t in items)
        {
            Destroy(t);
        }
    }

    protected void SetupPoseSmoothing()
    {
        UsePoseSmoothing &= VuforiaBehaviour.Instance.WorldCenterMode == WorldCenterMode.DEVICE; // pose smoothing only works with the DEVICE world center mode
        mPoseSmoother = new PoseSmoother(mObserverBehaviour, AnimationCurve);
        VuforiaBehaviour.Instance.World.OnStateUpdated += OnStateUpdated;
    }

    void OnStateUpdated()
    {
        if (enabled && UsePoseSmoothing)
            mPoseSmoother.Update();
    }

    void SetComponentsEnabled(bool enable)
    {
        var components = VuforiaRuntimeUtilities.GetComponentsInChildrenExcluding<Component, DefaultObserverEventHandler>(gameObject);
        foreach (var component in components)
        {
            switch (component)
            {
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
