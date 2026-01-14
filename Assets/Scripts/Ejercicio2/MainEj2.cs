using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainEj2 : MonoBehaviour
{
    DatosEj1 datos;
    List<Compuesto> listaC;
    TextMeshProUGUI txtElem;
    Text txtTarget;
    double tiempo;
    TMP_Dropdown drop, drop1;
    bool hide = false, hideOpc;

    // Use this for initialization
    async void Start() {
        hideOpc = true;
        datos = DatosEj1.Instance;
        listaC = await FBManager.getInstance().OnLoadIDCompuestos();
        txtElem = GameObject.Find("txtElem").GetComponent<TextMeshProUGUI>();
        drop = GameObject.Find("Dropdown").GetComponent<TMP_Dropdown>();

        List<string> ls = new List<string>();
        foreach (Compuesto c in listaC) {
            ls.Add(c.Descripcion);
        }
        drop.AddOptions(ls);
        drop.value = 0;
        txtElem.text = drop.captionText.text; //listaC.FirstOrDefault().Descripcion;
        tiempo = 0f;
        datos = DatosEj1.Instance;
        GameObject.Find("lblUser").GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetString("nombre");
        bool au = PlayerPrefs.GetInt("actAudio") == 0 ? true : false;
        GameObject.Find("audioInstruc").GetComponent<AudioSource>().mute = au;
        string msg = PlayerPrefs.GetInt("actAudio") == 0 ? "Activar Audio" : "Desactivar Audio";
        GameObject.Find("hideAudio").GetComponent<Text>().text = msg;
    }

    IEnumerator ControlTiempo() {
        tiempo += Time.deltaTime;
        yield return new WaitForSeconds(1f);
    }

    public void HidePanel() {
        hide = !hide;
        Debug.Log("hide:  " + hide);
        if (hide) {
            GameObject.Find("Panel").GetComponent<RectTransform>().localScale = Vector3.zero;
            GameObject.Find("hideText").GetComponent<Text>().text = "Instrucciones";
        }
        else {
            GameObject.Find("Panel").GetComponent<RectTransform>().localScale = Vector3.one;
            GameObject.Find("hideText").GetComponent<Text>().text = "Ocultar";
        }
    }

    public void Update() {
        if (Application.platform == RuntimePlatform.Android) {
            if (Input.GetKey(KeyCode.Escape)) {
                SceneManager.LoadScene("menu2");
            }
        }
    }

    public void Ayuda() { }

    public void Opciones() {
        hideOpc = !hideOpc;
        if (hideOpc) {
            GameObject.Find("btnAudio").GetComponent<RectTransform>().localScale = Vector3.zero;
            GameObject.Find("btnMainMenu").GetComponent<RectTransform>().localScale = Vector3.zero;
        }
        else {
            GameObject.Find("btnAudio").GetComponent<RectTransform>().localScale = Vector3.one;
            GameObject.Find("btnMainMenu").GetComponent<RectTransform>().localScale = Vector3.one;
        }
    }

    public void OpcionesAudio() {
        int au = PlayerPrefs.GetInt("actAudio") == 0 ? 1 : 0;
        string msg = PlayerPrefs.GetInt("actAudio") == 1 ? "Activar Audio" : "Desactivar Audio";
        GameObject.Find("hideAudio").GetComponent<Text>().text = msg;
        PlayerPrefs.SetInt("actAudio", au);
        Debug.Log("Audio:  " + au);
    }
}