using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Reacciones : MonoBehaviour
{
    TMP_Dropdown drop;
    TextMeshProUGUI txtCompuesto, txtElem1, txtElem2;
    DatosEj7 datos;
    List<React> listaC;
    bool hide = false, hideOpc;

    async void Start() {
        hideOpc = true;
        datos = DatosEj7.Instance;
        drop = GameObject.Find("Dropdown").GetComponent<TMP_Dropdown>();
        listaC = await FBManager.getInstance().OnLoadReacciones();
        Debug.Log ("Numero de Registros.: " + listaC.Count());
        List<string> ls = new List<string>();
        ls.AddRange(listaC.Select(x => x.Descripcion)); // Llenar los datos a un list temporal
        drop.AddOptions(ls);
        drop.value = 0;
        
        bool au = PlayerPrefs.GetInt("actAudio") == 0 ? true : false;
        GameObject.Find("audioInstruc").GetComponent<AudioSource>().mute = au;
        string msg = PlayerPrefs.GetInt("actAudio") == 0 ? "Activar Audio" : "Desactivar Audio";
        GameObject.Find("hideAudio").GetComponent<Text>().text = msg;
        txtCompuesto = GameObject.Find("txtCompuesto").GetComponent<TextMeshProUGUI>();

        txtElem1 = GameObject.Find("txtElem1").GetComponent<TextMeshProUGUI>();
        txtElem2 = GameObject.Find("txtElem2").GetComponent<TextMeshProUGUI>();
        txtCompuesto.text = drop.captionText.text;
        GameObject.Find("lblUser").GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetString("nombre");
        OnChange();

    }

    public void Next()
    {
        if (drop.value < drop.options.Count())
        {
            drop.value++;
            GameObject.Find("avance").GetComponent<Image>().fillAmount += 0.5f;
        }
        else
        {
            drop.value = 0;
            Debug.Log("Reiniciando los parametros");
        }
        OnChange();
        GameObject.Find("btnNext").GetComponent<RectTransform>().localScale = Vector3.zero;
        GameObject.Find("pnlMsg").GetComponent<RectTransform>().localScale = Vector3.zero;

        if (GameObject.Find("avance").GetComponent<Image>().fillAmount >= 0.6f)
        {
            GameObject.Find("btnTxt").GetComponent<Text>().text = "Finalizar";
        }
    }

    public void OnChange()
    {
        txtCompuesto.text = drop.captionText.text;      
        React c = DatosEj7.Instance.GetInfoReaccion(listaC, txtCompuesto.text);
        txtElem1.text = c.Elem1;
        txtElem2.text = c.Elem2;
        GameObject.Find("txtMsg").GetComponent<TextMeshProUGUI>().text = c.Efecto;
    }
    public void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                SceneManager.LoadScene("menu2");
            }
        }
    }

    public void Opciones()
    {
        hideOpc = !hideOpc;
        Vector3 scale = (hideOpc == true) ? Vector3.zero : Vector3.one;
        string msg = (hideOpc == true) ? "Mostrar Opciones" : "Ocultar Opciones";
        GameObject.Find("txtOpc").GetComponent<Text>().text = msg;
        GameObject.Find("btnAudio").GetComponent<RectTransform>().localScale = scale;
        GameObject.Find("btnMainMenu").GetComponent<RectTransform>().localScale = scale;
        GameObject.Find("btnReset").GetComponent<RectTransform>().localScale = scale;
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
    /*
    public void set1() {
        foreach (React a in this.datos.RecuperarLista()) {
            FBManager.getInstance().SaveReaction(a);
        }
    }
    */
}