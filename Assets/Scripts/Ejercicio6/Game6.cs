using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Game6 : MonoBehaviour
{
    TMP_Dropdown drop;
    TextMeshProUGUI txtCompuesto, txtElem1, txtElem2, txtTiempo2, txtErrores, txtAyudas, txtLogs, txtHelp;
    DatosEj6 datos;
    List<Enlace> listaM;
    List<Ayuda> listaMH;
    bool hide = false, hideOpc;
    int ayudas, errores, nivPrev = 0;
    double tiempo;

    async void Start(){
        hideOpc = true;
        tiempo = 0d;
        txtTiempo2 = GameObject.Find("txtTiempo2").GetComponent<TextMeshProUGUI>();
        txtHelp = GameObject.Find("txtHelp").GetComponent<TextMeshProUGUI>();
        bool au = PlayerPrefs.GetInt("actAudio") == 0 ? true : false;
        GameObject.Find("audioInstruc").GetComponent<AudioSource>().mute = au;
        txtLogs = GameObject.Find("txtLogs").GetComponent<TextMeshProUGUI>();
        datos = DatosEj6.Instance;
        listaM = await FBManager.getInstance().OnLoadBonds("Metálico");
        listaMH = await FBManager.getInstance().OnLoadHelps("Metalico");

        drop = GameObject.Find("Dropdown").GetComponent<TMP_Dropdown>();
        drop.ClearOptions();
        List<string> ls = new List<string>();
        ls.AddRange(listaM.Select(x => x.Descripcion)); // Llenar los datos a un list temporal
        drop.AddOptions(ls);
        drop.value = 0;

        GameObject.Find("lblUser").GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetString("nombre");
        txtCompuesto = GameObject.Find("txtCompuesto").GetComponent<TextMeshProUGUI>();
        txtElem1 = GameObject.Find("txtElem1").GetComponent<TextMeshProUGUI>();
        txtElem2 = GameObject.Find("txtElem2").GetComponent<TextMeshProUGUI>();
        txtErrores = GameObject.Find("txtErrores").GetComponent<TextMeshProUGUI>();
        txtAyudas = GameObject.Find("txtAyudas").GetComponent<TextMeshProUGUI>();
        txtCompuesto.text = drop.captionText.text;
        OnChange();
    }

    IEnumerator ControlTiempo() {
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

    public void Next() {
        if (drop.value < drop.options.Count()) {
            datos.ResetIndex();
            nivPrev++;
            drop.value++;
            GameObject.Find("avance").GetComponent<Image>().fillAmount += 0.33f;
        }
        else {
            nivPrev = 0;
            drop.value = 0;
            // Debug.Log("Reiniciando los parametros");
        }
        OnChange();
        GameObject.Find("btnNext").GetComponent<RectTransform>().localScale = Vector3.zero;
        GameObject.Find("pnlMsg").GetComponent<RectTransform>().localScale = Vector3.zero;

        if (GameObject.Find("avance").GetComponent<Image>().fillAmount >= 0.6f) {
            GameObject.Find("btnTxt").GetComponent<Text>().text = "Finalizar";
        }
        int nex = drop.value;
        Guardar(nex);
    }

    public string GetDificultad(int nivel)
    {
        string res = "Facil";
        switch (nivel)
        {
            case 1: res = "Facil"; break;
            case 2: res = "Regular"; break;
            case 3: res = "Dificil"; break;
        }
        return res;
    }

    public void Guardar(int aciertos)
    {
        string idUsuario, userMail, device, fecha, hora;
        idUsuario = PlayerPrefs.GetString("idUsuario");
        device = PlayerPrefs.GetString("device");
        userMail = PlayerPrefs.GetString("userMail");
        fecha = System.DateTime.Now.ToShortDateString();
        hora = System.DateTime.Now.ToShortTimeString();
        string niv = GetDificultad(nivPrev);
        errores = Convert.ToInt32(txtErrores.text);
        ayudas = Convert.ToInt32(txtAyudas.text);
        double tiempo = tiempo = double.Parse(GameObject.Find("txtTiempo2").GetComponent<TextMeshProUGUI>().text, System.Globalization.NumberStyles.Any);
        FBManager.getInstance().SaveExercise(new Ejercicio2("Reaq v2.2", "Metálicos", idUsuario, niv, aciertos, errores / 2, ayudas, (float)(tiempo), ((aciertos * 300) - (((errores / 2) + ayudas) * 100)), aciertos, fecha, hora));
        txtLogs.text = txtLogs.text.Replace("EE", "E");
        Debug.Log(getHex(nivPrev));
        FBManager.getInstance().SaveBehaviour(new Behaviour(idUsuario, "Metálicos", txtLogs.text + getHex(nivPrev), fecha, hora, "Reaq v2.2"));
        txtLogs.text = string.Empty;
    }

    public string getHex(int nivPrev)
    {
        string res = "";
        switch (nivPrev)
        {
            case 1: res = "B"; break;
            case 2: res = "C"; break;
            case 3: res = "D"; break;
        }
        return res;
    }

    public void OnChange()
    {
        txtCompuesto.text = drop.captionText.text;
        Enlace c = DatosEj6.Instance.GetInformacionEnlaces(listaM, txtCompuesto.text);        
        txtElem1.text = c.Elem1;
        txtElem2.text = c.Elem2;
    }

    public void HidePanel()
    {
        hide = !hide;
        if (hide)
        {
            GameObject.Find("Panel").GetComponent<RectTransform>().localScale = Vector3.zero;
            GameObject.Find("hideText").GetComponent<Text>().text = "Instrucciones";
        }
        else
        {
            GameObject.Find("Panel").GetComponent<RectTransform>().localScale = Vector3.one;
            GameObject.Find("hideText").GetComponent<Text>().text = "Ocultar";
        }
    }
    public void Update()
    {
        StartCoroutine("ControlTiempo");
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                SceneManager.LoadScene("menu2");
            }
        }
    }

    public IEnumerator ShowAyuda()
    {
        ayudas = Convert.ToInt32(txtAyudas.text);
        ayudas++;
        txtAyudas.text = ayudas.ToString();
        txtLogs.text += "H";
        Ayuda h1 = datos.GetAyudaMet(listaMH, txtCompuesto.text, datos.UpdateIndex());
        GameObject.Find("pnlHelp").GetComponent<RectTransform>().localScale = Vector3.one;
        txtHelp.text = h1.Descripcion;
        yield return new WaitForSeconds(3f);
        GameObject.Find("pnlHelp").GetComponent<RectTransform>().localScale = Vector3.zero;
    }

    public void Ayuda()
    {
        StartCoroutine("ShowAyuda");
    }

    public void Opciones()
    {
        hideOpc = !hideOpc;
        Vector3 scale = (hideOpc == true) ? Vector3.zero : Vector3.one;
        string msg = (hideOpc == true) ? "Mostrar Opciones" : "Ocultar Opciones";
        GameObject.Find("txtOpc").GetComponent<Text>().text = msg;
        GameObject.Find("btnAudio").GetComponent<RectTransform>().localScale = scale;
        GameObject.Find("btnReset").GetComponent<RectTransform>().localScale = scale;
        GameObject.Find("btnMainMenu").GetComponent<RectTransform>().localScale = scale;
        GameObject.Find("btnAyuda").GetComponent<RectTransform>().localScale = scale;
        Vector3 pos = (GameObject.Find("btnNext").GetComponent<RectTransform>().localScale == Vector3.one && GameObject.Find("btnMainMenu").GetComponent<RectTransform>().localScale == Vector3.one) ?
        GameObject.Find("btnNext2").GetComponent<RectTransform>().localPosition : GameObject.Find("btnMainMenu").GetComponent<RectTransform>().localPosition;
        GameObject.Find("btnNext").GetComponent<RectTransform>().localPosition = pos;
    }

    public void OpcionesAudio()
    {
        int au = PlayerPrefs.GetInt("actAudio") == 0 ? 1 : 0;
        string msg = PlayerPrefs.GetInt("actAudio") == 1 ? "Activar Audio" : "Desactivar Audio";
        GameObject.Find("hideAudio").GetComponent<Text>().text = msg;
        PlayerPrefs.SetInt("actAudio", au);
        Debug.Log("Audio:  " + au);
    }    
}