using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class getEscenas : MonoBehaviour
{
    private ArrayList escenas = new ArrayList();
    private string setEscena = "LogIn";
    public string LoadingScene;

    public void CargarEscena()
    {
        Debug.Log("Se ha cargado la escena " + LoadingScene);
        SceneManager.LoadScene(LoadingScene);
    }

    public void CargarEscena(int IdEscena)
    {
        setEscena = escenas[IdEscena].ToString();
        Debug.Log("Se ha cargado la escena " + setEscena);
        SceneManager.LoadScene(setEscena);
    }

    // Use this for initialization
    void Start()
    {
        escenas.Add("LogIn");
        escenas.Add("menu2");
        escenas.Add("IDElementos");
        escenas.Add("ElecValencia");
        escenas.Add("Covalentes");
        escenas.Add("Ionicos");
        escenas.Add("CHONPS");
        escenas.Add("Metalicos");
        escenas.Add("Reacciones2");       
    }

    public void LoginMenu()
    {
        int indice = 0;
        OnSetScene(indice);
    }

    // Update is called once per frame
    public void MainMenu()
    {
        int indice = 1;
        OnSetScene(indice);
    }

    public void OnClickIDElementos()
    {
        int indice = 2;
        OnSetScene(indice);
    }

    public void OnClickIDCompuestos()
    {
        int indice = 3;
        OnSetScene(indice);
    }

    public void OnClickCovalentes()
    {
        int indice = 4;
        OnSetScene(indice);
    }

    public void OnClickIonicos()
    {
        int indice = 5;
        OnSetScene(indice);
    }
    public void OnClickCHONPS()
    {
        int indice = 6;
        OnSetScene(indice);
    }
    public void OnClickMetalicos()
    {
        int indice = 7;
        OnSetScene(indice);
    }
    public void OnClickReacciones()
    {
        int indice = 8;
        OnSetScene(indice);
    }

    public void OnDownloadMarkersClick()
    {
        Application.OpenURL("https://drive.google.com/open?id=1065MpdNbbhrOxYzxBHWj9h1-1r9Qe5Jd&authuser=aup840%40gmail.com&usp=drive_fs");
    }

    public void OnClickQuit()
    {
        Application.Quit();
    }

    public void OnSetScene(int indice)
    {
        setEscena = escenas[indice].ToString();
     //   Debug.Log("Se ha cargado la escena " + setEscena);
        SceneManager.LoadScene(setEscena);
    }
}