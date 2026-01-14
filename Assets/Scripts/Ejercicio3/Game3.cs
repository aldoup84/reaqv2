using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Game3 : MonoBehaviour
{
    TMP_Dropdown drop;
    TextMeshProUGUI txtCompuesto, txtElem1, txtElem2, txtTiempo2, txtErrores, txtAyudas, txtLogs, txtHelp;
    DatosEj3 datos;
    List<Enlace> listaC;
    List<Ayuda> listaCH;
    bool hide = false, hideOpc;
    double tiempo;
    int ayudas, errores, nivPrev = 0;

    async void Start() {
        string idUsuario = PlayerPrefs.GetString("idUsuario");
        tiempo = 0d;
        txtTiempo2 = GameObject.Find("txtTiempo2").GetComponent<TextMeshProUGUI>();
        txtLogs = GameObject.Find("txtLogs").GetComponent<TextMeshProUGUI>();
        StartCoroutine("ControlTiempo");
        hideOpc = true;
        bool au = PlayerPrefs.GetInt("actAudio") == 0 ? true : false;
        GameObject.Find("audioInstruc").GetComponent<AudioSource>().mute = au;
        string msg = PlayerPrefs.GetInt("actAudio") == 0 ? "Activar Audio" : "Desactivar Audio";
        GameObject.Find("hideAudio").GetComponent<Text>().text = msg;
        
        // Debug.Log("Iniciando la descarga desde firestore ");
        datos = DatosEj3.Instance;
        //listaC = datos.CrearListaEnlaces();
        listaC = await FBManager.getInstance().OnLoadBonds("Covalente");
        listaCH = await FBManager.getInstance().OnLoadHelps("Covalente");

        //Debug.Log("Cargando elementos " + listaC.Count());
        drop = GameObject.Find("Dropdown").GetComponent<TMP_Dropdown>();
        List<string> ls = new List<string>();
        ls.AddRange(listaC.Select(x => x.Descripcion)); // Llenar los datos a un list temporal
        drop.AddOptions(ls);
        drop.value = 0;
        GameObject.Find("lblUser").GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetString("nombre");
        txtCompuesto = GameObject.Find("txtCompuesto").GetComponent<TextMeshProUGUI>();
        txtElem1 = GameObject.Find("txtElem1").GetComponent<TextMeshProUGUI>();
        txtHelp = GameObject.Find("txtHelp").GetComponent<TextMeshProUGUI>();
        txtElem2 = GameObject.Find("txtElem2").GetComponent<TextMeshProUGUI>();
        txtErrores = GameObject.Find("txtErrores").GetComponent<TextMeshProUGUI>();
        txtAyudas = GameObject.Find("txtAyudas").GetComponent<TextMeshProUGUI>();
        txtCompuesto.text = drop.captionText.text;
        OnChange();
    }

    IEnumerator ControlTiempo(){
        try{
            tiempo = double.Parse(txtTiempo2.text, System.Globalization.NumberStyles.Any);
            tiempo += Time.deltaTime;
            txtTiempo2.text = tiempo.ToString("F");
        }
        catch (Exception e){
            Debug.Log(e.Message);
        }
        yield return new WaitForSeconds(0);
    }

    public void Next(){
        if (drop.value < drop.options.Count()) {
            datos.ResetIndex();
            nivPrev++;
            drop.value++;
            GameObject.Find("avance").GetComponent<Image>().fillAmount += 0.20f;
        }
        else {
            nivPrev = 0;
            drop.value = 0;
            Debug.Log("Reiniciando los parametros");
        }
        OnChange();
        GameObject.Find("btnNext").GetComponent<RectTransform>().localScale = Vector3.zero;
        GameObject.Find("pnlMsg").GetComponent<RectTransform>().localScale = Vector3.zero;
        int nex = drop.value + 1;
        Guardar(nex);
    }

    public string GetDificultad(int nivel)
    {
        string res = "Facil";
        switch (nivel)
        {
            case 1: res = "Muy Facil"; break;
            case 2: res = "Facil"; break;
            case 3: res = "Regular"; break;
            case 4: res = "Alto"; break;
            case 5: res = "Avanzado"; break;
        }
        return res;
    }

    public void Guardar(int aciertos)
    {
        string idUsuario, fecha, hora;
        idUsuario = PlayerPrefs.GetString("idUsuario");        
        fecha = System.DateTime.Now.ToShortDateString();
        hora = System.DateTime.Now.ToShortTimeString();
        string niv = GetDificultad(nivPrev);
        errores = Convert.ToInt32(txtErrores.text);
        ayudas = Convert.ToInt32(txtAyudas.text);
        double tiempo = tiempo = double.Parse(GameObject.Find("txtTiempo2").GetComponent<TextMeshProUGUI>().text, System.Globalization.NumberStyles.Any);
        FBManager.getInstance().SaveExercise(new Ejercicio2("Reaq v2.2", "Covalentes", idUsuario, niv, aciertos, errores / 2, ayudas, (float)(tiempo), ((aciertos * 300) - (((errores / 2) + ayudas) * 100)), aciertos, fecha, hora));
        txtLogs.text = txtLogs.text.Replace("EE", "E");
        FBManager.getInstance().SaveBehaviour(new Behaviour(idUsuario, "Covalente", txtLogs.text + nivPrev, fecha, hora, "Reaq v2.2"));
        txtLogs.text = string.Empty;
    }

    public void OnChange()
    {
        txtCompuesto.text = drop.captionText.text;
        // Debug.Log(drop.captionText.text);
        Enlace c = DatosEj3.Instance.GetInformacionEnlaces(listaC, txtCompuesto.text);
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
    /*
    public IEnumerator ShowAyuda()
    {
        ayudas = Convert.ToInt32(txtAyudas.text);
        ayudas++;
        txtAyudas.text = ayudas.ToString();
        txtLogs.text += "H";
        //Ayuda h1 = datos.GetAyuda2(txtCompuesto.text, datos.UpdateIndex());

        GameObject.Find("pnlHelp").GetComponent<RectTransform>().localScale = Vector3.one;
        txtHelp.text = h1.Descripcion;
        yield return new WaitForSeconds(3f);
        GameObject.Find("pnlHelp").GetComponent<RectTransform>().localScale = Vector3.zero;
    }
    */
    
    public IEnumerator ShowAyuda() {
        ayudas = Convert.ToInt32(txtAyudas.text);
        ayudas++;
        txtAyudas.text = ayudas.ToString();
        txtLogs.text += "H";
        //Ayuda h1 = datos.GetAyuda2(txtCompuesto.text, datos.UpdateIndex());
        Ayuda h1 = datos.GetAyudaCov(listaCH, txtCompuesto.text, datos.UpdateIndex());
        //h1 = FBManager.getInstance().OnLoadHelps("Covalente");
        GameObject.Find("pnlHelp").GetComponent<RectTransform>().localScale = Vector3.one;
        txtHelp.text = h1.Descripcion;
        yield return new WaitForSeconds(3f);
        GameObject.Find("pnlHelp").GetComponent<RectTransform>().localScale = Vector3.zero;
    }

    public void PedirAyuda()
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