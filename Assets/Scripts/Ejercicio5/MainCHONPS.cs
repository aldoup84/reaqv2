using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainCHONPS : MonoBehaviour
{
    DatosCHONPS datosChonps;
    List<Pregunta> lista;
    TextMeshProUGUI txtElem, txtPregunta, txtTarget, txtTiempo, txtTiempo2, txtErrores, txtAyudas;
    double tiempo = 0, tiempo2 = 0;
    TMP_Dropdown drop;
    bool detener = false;
    bool hide = false, hideOpc;
    int ayudas = 0, errores = 0;

    async void Start() {
        datosChonps = DatosCHONPS.Instance;
        lista = await FBManager.getInstance().OnLoadPreguntas();
        txtElem = GameObject.Find("txtElem").GetComponent<TextMeshProUGUI>();
        txtTiempo = GameObject.Find("txtTiempo").GetComponent<TextMeshProUGUI>();
        txtTiempo2 = GameObject.Find("txtTiempo2").GetComponent<TextMeshProUGUI>();
        txtErrores = GameObject.Find("txtErrores").GetComponent<TextMeshProUGUI>();
        txtAyudas = GameObject.Find("txtAyudas").GetComponent<TextMeshProUGUI>();
        txtPregunta = GameObject.Find("txtPregunta").GetComponent<TextMeshProUGUI>();
        drop = GameObject.Find("Dropdown").GetComponent<TMP_Dropdown>();
        tiempo = 0f;
        List<string> lsX = new List<string>();
        lista.ForEach(b => lsX.Add(b.Respuesta));
        drop.AddOptions(lsX);
        lsX = null;
        hideOpc = true;
        txtElem.text = drop.captionText.text;
        txtTiempo2.text = "" + tiempo2.ToString("F");
        bool au = PlayerPrefs.GetInt("actAudio") == 0 ? true : false;
        GameObject.Find("audioInstruc").GetComponent<AudioSource>().mute = au;
        string msg = PlayerPrefs.GetInt("actAudio") == 0 ? "Activar Audio" : "Desactivar Audio";
        GameObject.Find("hideAudio").GetComponent<Text>().text = msg;
        GetPregunta1();
        GameObject.Find("lblUser").GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetString("nombre");
    }

    public void Next()
    {
        if (drop.value < drop.options.Count())
        {
            drop.value++;
        }
        else
        {
            drop.value = 0;
            Debug.Log("Reiniciando los parametros");
        }
        OnChange();
        GameObject.Find("btnNext").GetComponent<RectTransform>().localScale = Vector3.zero;
    }

    public void OnChange()
    {
        // txtTarget.text = drop.captionText.text;
        Pregunta p = DatosCHONPS.Instance.GetInformacionPregunta(DatosCHONPS.Instance.RecuperarLista(), drop.captionText.text);
        // Pregunta c = DatosCHONPS.Instance.GetInformacionPregunta(txtElem.text);
        txtElem.text = p.Respuesta;
    }

    public void OnDropChange()
    {
        GameObject.Find("txtInstrucciones").GetComponent<TextMeshProUGUI>().text =
            "Instrucciones: Coloca sobre la camara el marcador que corresponda a uno de los elementos" +
   "que forman el compuesto de " + drop.captionText.text;
    }

    public void GetPregunta1()
    {
        drop.value = 0;
        Pregunta p = DatosCHONPS.Instance.GetInformacionPregunta(lista, drop.captionText.text);     
        txtPregunta.text = p.Question;
        txtElem.text = p.Marcador;
        GameObject.Find("Panel3").GetComponent<RectTransform>().localScale = Vector3.zero;
        tiempo2 = 0f;
        txtTiempo2.text = "" + tiempo2.ToString("F");
    }

    public void GetPreguntas()
    {
        drop.value++;
        Pregunta p = DatosCHONPS.Instance.GetInformacionPregunta(lista, drop.captionText.text);
        txtPregunta.text = p.Question;
        txtElem.text = p.Marcador;
        GameObject.Find("Panel3").GetComponent<RectTransform>().localScale = Vector3.zero;
        tiempo2 = 0f;
        txtTiempo2.text = "" + tiempo2.ToString("F");
        GameObject.Find("txtTiempo2").GetComponent<RectTransform>().localScale = Vector3.one;
        Progreso();
        Guardar(drop.value);
    }

    public void SetAudio(int elem) {
        if (PlayerPrefs.GetInt("actAudio") == 1){
            AudioClip aud = Resources.Load("Audio/Chonps/CH-" + elem, typeof(AudioClip)) as AudioClip;
            AudioSource audio = GetComponent<AudioSource>();
            audio.clip = aud;
            audio.Play();
        }
    }

    public void OcultarPanel(){
        GameObject.Find("Panel4").GetComponent<RectTransform>().localScale = Vector3.zero;
    }

    IEnumerator ControlTiempo()
    {
        try
        {
            tiempo = double.Parse(txtTiempo.text, System.Globalization.NumberStyles.Any);
            tiempo2 = double.Parse(txtTiempo2.text, System.Globalization.NumberStyles.Any);
            tiempo += Time.deltaTime;
            tiempo2 += Time.deltaTime;
            txtTiempo.text = "" + tiempo.ToString("F");
            txtTiempo2.text = "" + tiempo2.ToString("F");
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
        yield return new WaitForSeconds(0);
    }

    public void PausarTiempo()
    {
        Debug.Log("Parando Rutina Tiempo");
        StopCoroutine("ControlTiempo");
    }

    void Update()
    {           // Update is called once per frame
        StartCoroutine("ControlTiempo");
        if (detener == false)
        {
            StartCoroutine("ControlTiempo");
        }
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                SceneManager.LoadScene("menu2");
            }
        }
    }

    public void ReiniciarTiempo()
    {
        Debug.Log("Reiniciando Tiempo");
        tiempo2 = 0.0f;
        StartCoroutine("ControlTiempo");

    }

    public void Progreso()
    {
        Button b = GameObject.Find("avance").GetComponent<Button>();
        b.GetComponent<Image>().fillAmount += 0.20f;

    }

    public string GetDificultad(int nivel)
    {
        string res = "Facil";
        if (nivel == 1 || nivel == 2)
        {
            res = "Facil";
        }
        else if (nivel == 3 || nivel == 4)
        {
            res = "Regular";
        }
        else if (nivel == 5 || nivel == 6)
        {
            res = "Dificil";
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
        string niv = GetDificultad(aciertos);
        errores = Convert.ToInt32(txtErrores.text);
        ayudas = Convert.ToInt32(txtAyudas.text);
        double tiempo = tiempo = double.Parse(GameObject.Find("txtTiempo2").GetComponent<TextMeshProUGUI>().text, System.Globalization.NumberStyles.Any);
        FBManager.getInstance().SaveExercise(new Ejercicio2("Reaq v2.2", "CHONPS", idUsuario, niv, aciertos, errores, ayudas, (float)(tiempo), aciertos * 200, aciertos, fecha, hora));
    }

    public void Opciones()
    {
        Debug.Log("hideOpc:  " + hideOpc);
        hideOpc = !hideOpc;
        if (hideOpc)
        {
            GameObject.Find("btnAudio").GetComponent<RectTransform>().localScale = Vector3.zero;
            GameObject.Find("btnMainMenu").GetComponent<RectTransform>().localScale = Vector3.zero;
        }
        else
        {
            GameObject.Find("btnAudio").GetComponent<RectTransform>().localScale = Vector3.one;
            GameObject.Find("btnMainMenu").GetComponent<RectTransform>().localScale = Vector3.one;
        }
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