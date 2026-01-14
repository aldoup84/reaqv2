using Firebase;
using Firebase.Database;
using Firebase.Auth;
using System;
using UnityEngine;
using UnityEngine.UI;

public class DBManager : MonoBehaviour
{
    DatabaseReference reference;
    FirebaseDatabase db;
    async void Start()
    {

        Firebase.FirebaseApp.DefaultInstance.Options.DatabaseUrl = new Uri("https://unityfb-a1403.firebaseio.com/");
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        InicializarDB();
    }

    void InicializarDB()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                //Debug.Log("Disponible");
            }
            else
            {
                Debug.LogError(System.String.Format("Could not resolve all Firebase dependency: {0} ", dependencyStatus));
            }
        });
    }

    void SetSettings()
    {
        db.SetPersistenceEnabled(true);

    }


    public void BtnGuardar()
    {
        string txtUser = GameObject.Find("lblUser2").GetComponent<Text>().text;
        string txtEmail = GameObject.Find("lblEmail2").GetComponent<Text>().text;
        string txtNom = GameObject.Find("lblNombre2").GetComponent<Text>().text;
        writeNewUser(txtUser, txtNom, txtEmail);
    }

    public void BtnGuardar(string idUsuario, string nomUsuario, string email)
    {
        writeNewUser(idUsuario, nomUsuario, email);
    }

    public void BtnGuardarEjercicio1()
    {
        string fecha = DateTime.Now.Date.ToString();
        writeNewData("0003", "0001", "0005", "admin", "facil", "ReAQv2", 10, 3, 0, 50.3f, 700, 1, fecha, fecha);
    }

    public void BtnGuardarEjercicio1(string folio, string idEjercicio, string idSesion, string idUsuario, string dificultad, string app, int aciertos, int errores, int ayudas, float tiempo, int puntos, int nivel, string fecha, string hora)
    {
        writeNewData(folio, idEjercicio, idSesion, idUsuario, dificultad, app, aciertos, errores, ayudas, tiempo, puntos, nivel, fecha, hora);
    }


    private void writeNewUser(string idUsuario, string nomUsuario, string email)
    {
        User user = new User(idUsuario, nomUsuario, email);
        string json = JsonUtility.ToJson(user);
        reference.Child("Usuarios").Child(idUsuario).SetRawJsonValueAsync(json);
    }

    private void writeNewData(string folio, string idEjercicio, string idSesion, string idUsuario, string dificultad, string app, int aciertos, int errores, int ayudas, float tiempo, int puntos, int nivel, string fecha, string hora)
    {

        Ejercicio ej = new Ejercicio(folio, idSesion, idEjercicio, idUsuario, dificultad, aciertos, errores, ayudas, tiempo, puntos, nivel, fecha, hora);
        string json = JsonUtility.ToJson(ej);
        reference.Child("Ejercicios").Child(app).Child(idEjercicio).Child(idUsuario).Child(folio).SetRawJsonValueAsync(json);
        Debug.Log("Datos Guardados");
    }
}

public class User
{
    public string nomUsuario, email, idUsuario;
    public User() { }
    public User(string idUsuario, string nomUsuario, string email)
    {
        this.idUsuario = idUsuario;
        this.nomUsuario = nomUsuario;
        this.email = email;
    }
}