using Firebase.Auth;
using System;
using System.Collections;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Firebase.Extensions;
using Firebase;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour
{
    public const string LOGGED_USER_KEY = "LOGGED_USER";

    public GameObject txtEmail;
    public InputField txtPassword;

    public GameObject errMessage;
    //  private Animator errAnimator;

    private FirebaseAuth auth;
    async void Start()
    {
        this.auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        //    this.errAnimator = this.errMessage.GetComponent<Animator>();
    }

    public void HideErrorMsg()
    {
        //    this.errAnimator.SetBool("open", false);
    }

    private async Task<Firebase.Auth.FirebaseUser> DoFirebaseRequest(string email, string password)
    {
        FirebaseUser newUser2 = null;
        await auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }
            FirebaseUser newUser = task.Result.User;
            newUser2 = task.Result.User;
            Debug.LogFormat("User signed in successfully: {0} ({1})", newUser.DisplayName, newUser2.UserId);
        });
        return newUser2;
    }

    public async void OnLoginClick() {
        InputField txtEmail = GameObject.Find("txtUsuario").GetComponent<InputField>();
        string password = txtPassword.text;
        string email = txtEmail.text.ToString();       
        try
        {
            FirebaseUser user = await DoFirebaseRequest(email.ToLower(), password);            
            PlayerPrefs.SetString(LOGGED_USER_KEY, user.UserId);
            PlayerPrefs.SetString("email", user.Email);         
            //Debug.Log(user.UserId + " " + user.Email + " " + password);
            SceneManager.LoadScene("menu2");
        }
        catch (Exception ex){            
            Debug.LogError(ex.Message);
            StartCoroutine("ErrLog");
        }
    }    
    public IEnumerator ErrLog(){
        GameObject.Find("pnlErrLog").GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.3f, 1f);
        yield return new WaitForSeconds(3);
        GameObject.Find("pnlErrLog").GetComponent<RectTransform>().localScale = Vector3.zero;
    }
}