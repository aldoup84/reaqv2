using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AuthManager : MonoBehaviour {
    protected Firebase.Auth.FirebaseAuth auth;
    protected Firebase.Auth.FirebaseUser user;
    private string displayName;
    public Text inputLoginName;
    public InputField InputLoginEmail, InputLoginPassword, InputFBUser, InputFBEmail, InputFBPassword, InputFBName, InputEdad, InputEscuela, InputGrupo, InputRegName;
    public TMP_Dropdown dropGenero, dropTipoEscuela;
    private bool signedIn;
    private bool logIndicator = false;

    void Start() {
        InitializeFirebase();
    }

    private void Update() {
        if (logIndicator) {
            ActivatedSession();
            GetSesssionProfile();
            GetActiveSession();
            SceneManager.LoadScene("Scene01");
        }
    }

    void InitializeFirebase() {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }

    void AuthStateChanged(object sender, System.EventArgs eventArgs) {
        if (auth.CurrentUser != user) {
            signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
            if (!signedIn && user != null) {
                Debug.Log("Signed out " + user.UserId);
            }
            user = auth.CurrentUser;
            if (signedIn) {
                Debug.Log("Signed in " + user.UserId);
                displayName = user.DisplayName ?? "";
            }
        }
    }

    public void CreateUserByEmail(string email, string password) {
        Debug.Log("Email: " + email + ", Password: " + password);
        try {
            auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
               // Debug.Log("Estatus  " + task.Status);
                if (task.IsCanceled) {
                    Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                    return;
                }

                if (task.IsFaulted) {
                    Debug.Log("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                    LoginSessionByEmail();
                    return;
                }

                // Firebase user has been created.
                Firebase.Auth.FirebaseUser newUser = task.Result.User;
                Debug.LogFormat("Firebase user created successfully: {0} ({1})", newUser.DisplayName, newUser.UserId);
            });
        }
        catch (Exception e) {
            Debug.Log(e.Message);
        }
    }

    public void ActivatedSession() {
        string email = InputLoginEmail.text;
        string password = InputLoginPassword.text;

        void InitializeFirebase() {
            auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
            auth.StateChanged += AuthStateChanged;
            AuthStateChanged(this, null);
        }

        void AuthStateChanged(object sender, System.EventArgs eventArgs) {
            if (auth.CurrentUser != user) {
                signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
                if (!signedIn && user != null) {
                    Debug.Log("Signed out " + user.UserId);
                }
                user = auth.CurrentUser; if (signedIn) {
                    Debug.Log("Signed in " + user.UserId);
                }
            }
        }

        void OnDestroy() {
            auth.StateChanged -= AuthStateChanged;
            auth = null;
        }
    }

    public void GetSesssionProfile() {
        Firebase.Auth.FirebaseUser user = auth.CurrentUser;
        if (user != null) {
            string name = user.DisplayName;
            string email = user.Email;
            System.Uri photo_url = user.PhotoUrl;
            string uId = user.UserId;
        }
    }

    public void GetActiveSession() {
        Firebase.Auth.FirebaseAuth auth;
        Firebase.Auth.FirebaseUser user;
        void InitializeFireBase() {
            Debug.Log("Setting up Firebase");
            auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
            auth.StateChanged += AuthStateChanged;
            AuthStateChanged(this, null);
        }

        void AuthStateChanged(object sender, System.EventArgs eventArgs) {
            if (auth.CurrentUser != user) {
                signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
                if (!signedIn && user != null) {
                    Debug.Log("Signed out " + user.UserId);
                }
                user = auth.CurrentUser;
                if (signedIn) {
                    Debug.Log("Signed in " + user.UserId);
                }
            }
        }

        void OnDestroy() {
            auth.StateChanged -= AuthStateChanged;
            auth = null;
        }
    }

    public void CerrarSesion() {
        auth.StateChanged -= AuthStateChanged;
        auth = null;
    }

    public void LoginSessionByEmail() {
        string email = InputLoginEmail.text;
        string password = InputLoginPassword.text;
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if (task.IsCanceled) {
                Debug.Log("SignInWithEmailAndPasswordAsync was canceled");
                return;
            }
            if (task.IsFaulted) {
                Debug.Log("SignInWithEmailAndPasswordAsync encountered an Error" + task.Exception);
                return;
            }
            Firebase.Auth.FirebaseUser newUser = task.Result.User;
            Debug.LogFormat("User signed In successfully: {0} ({1})", newUser.DisplayName, newUser.UserId);
            logIndicator = true;
        });
    }

    public void RegistrarFB() {
        //InputRegEmail.text = InputFBEmail.text;
        //InputRegPassword.text = InputFBPassword.text;
        //InputRegName.text = InputFBName.text;
        //InputRegUser.text = InputFBUser.text;
        CreateUserByEmail(InputFBEmail.text, InputFBPassword.text);

        // FBManager.getInstance().SaveUser(new Usuario(InputFBUser.text, InputFBName.text, InputFBEmail.text, InputFBPassword.text, InputEscuela.text, InputGrupo.text, dropGenero.captionText.text, dropTipoEscuela.captionText.text, Convert.ToInt32(this.InputEdad.text)));
        GameObject.Find("LoginPanel").GetComponent<RectTransform>().localScale = Vector3.zero;
        GameObject.Find("PanelUserFB").GetComponent<RectTransform>().localScale = Vector3.zero;
        GameObject.Find("PanelRegistro").GetComponent<RectTransform>().localScale = new Vector3(0.6f, 0.5f, 0.5f);
    }

    public void MostrarRegFB() {
        GameObject.Find("LoginPanel").GetComponent<RectTransform>().localScale = Vector3.zero;
        GameObject.Find("PanelRegistro").GetComponent<RectTransform>().localScale = Vector3.zero;
        GameObject.Find("PanelUserFB").GetComponent<RectTransform>().localScale = new Vector3(0.6f, 0.5f, 0.5f);
    }

    public void RegUser() {
        if (FBManager.getInstance().SaveUser(new Usuario(InputFBUser.text, InputFBName.text, InputFBEmail.text.ToLower(), InputFBPassword.text, InputEscuela.text, InputGrupo.text, dropGenero.captionText.text, dropTipoEscuela.captionText.text, Convert.ToInt32(this.InputEdad.text)))){
            PlayerPrefs.SetString("id", InputFBUser.text);
            PlayerPrefs.SetString("nombre", InputFBName.text);
            PlayerPrefs.SetString("email", InputFBEmail.text);
            PlayerPrefs.SetInt("derechos", 1);
            PlayerPrefs.SetString("device", SystemInfo.deviceName);
            Debug.Log(InputFBUser.text + "  " + InputFBName.text + "  " + InputFBEmail.text + "  " + InputFBPassword.text);
            SceneManager.LoadScene("menu2");
            return;
        }
        GameObject.Find("LoginPanel").GetComponent<RectTransform>().localScale = new Vector3(0.6f, 0.5f, 0.5f);
        GameObject.Find("PanelRegistro").GetComponent<RectTransform>().localScale = Vector3.zero;
        GameObject.Find("PanelUserFB").GetComponent<RectTransform>().localScale = Vector3.zero;
    }
}