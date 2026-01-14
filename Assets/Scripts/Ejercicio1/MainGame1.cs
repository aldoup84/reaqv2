using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainGame1 : MonoBehaviour
{
    DatosEj1 datos;
    List<Elemento> lista;
    List<Compuesto> listaC;
    TextMeshProUGUI txtElem, txtTiempo2;
    Text txtTarget;
    double tiempo;
    bool hide, hideOpc;
    async void Start(){
        datos = DatosEj1.Instance;
        listaC = await FBManager.getInstance().OnLoadIDCompuestos();
        txtElem = GameObject.Find("txtElem").GetComponent<TextMeshProUGUI>();
        tiempo = 0d;
        hideOpc = true;
        bool au = PlayerPrefs.GetInt("actAudio") == 0 ? true : false;
        GameObject.Find("audioInstruc").GetComponent<AudioSource>().mute = au;
        txtTiempo2 = GameObject.Find("txtTiempo2").GetComponent<TextMeshProUGUI>();
        GameObject.Find("lblUser").GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetString("nombre");
        StartCoroutine("ControlTiempo");
    }

    IEnumerator ControlTiempo(){
        try {
            tiempo = double.Parse(txtTiempo2.text, System.Globalization.NumberStyles.Any);
            tiempo += Time.deltaTime;
            txtTiempo2.text = tiempo.ToString("F");
        }
        catch (Exception e) {
            Debug.Log(e.Message);
        }
        yield return new WaitForSeconds(0);
    }

    public void ShowPanel2() {
        GameObject.Find("Panel3").GetComponent<RectTransform>().localScale = Vector3.one; //new Vector3(0.4f, 0.5f, 1f);
    }

    public void HidePanel() {
        hide = !hide;
        Debug.Log("hide:  " + hide);
        if (hide) {
            GameObject.Find("Panel").GetComponent<RectTransform>().localScale = Vector3.zero;
            GameObject.Find("hideText").GetComponent<Text>().text = "Mostrar Instrucciones";
        }
        else {
            GameObject.Find("Panel").GetComponent<RectTransform>().localScale = Vector3.one;
            GameObject.Find("hideText").GetComponent<Text>().text = "Ocultar Instrucciones";
        }
    }

    private void Update(){
        StartCoroutine("ControlTiempo");
        if (Application.platform == RuntimePlatform.Android){
            if (Input.GetKey(KeyCode.Escape)) {
                SceneManager.LoadScene("menu2");
            }
        }
    }

    public void Opciones(){
 //       Debug.Log("hideOpc:  " + hideOpc);
        hideOpc = !hideOpc;
        if (hideOpc){
            GameObject.Find("btnAudio").GetComponent<RectTransform>().localScale = Vector3.zero;
            GameObject.Find("btnMainMenu").GetComponent<RectTransform>().localScale = Vector3.zero;
        }
        else{
            GameObject.Find("btnAudio").GetComponent<RectTransform>().localScale = Vector3.one;
            GameObject.Find("btnMainMenu").GetComponent<RectTransform>().localScale = Vector3.one;
        }
    }

    public void OpcionesAudio(){
        int au = PlayerPrefs.GetInt("actAudio") == 0 ? 1 : 0;
        string msg = PlayerPrefs.GetInt("actAudio") == 1 ? "Activar Audio" : "Desactivar Audio";
        GameObject.Find("hideAudio").GetComponent<Text>().text = msg;
        PlayerPrefs.SetInt("actAudio", au);
        Debug.Log("Audio:  " + au);
    }
}
