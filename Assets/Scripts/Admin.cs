using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Admin : MonoBehaviour {
    TextMeshProUGUI txtAuth, txtAuth2, txtVersion;
    InputField txtPass;
    // Use this for initialization
    
    void Start() {        
        txtAuth = GameObject.Find("txtAuth").GetComponent<TextMeshProUGUI>();
        txtAuth2 = GameObject.Find("txtAuth2").GetComponent<TextMeshProUGUI>();
        txtPass = GameObject.Find("txtPass").GetComponent<InputField>();
        string id = PlayerPrefs.GetString("idUsuario");
        string nombre = PlayerPrefs.GetString("nombre");
        int derechos = PlayerPrefs.GetInt("derechos");
        string email = PlayerPrefs.GetString("email");
        string userName = PlayerPrefs.GetString("nombre");       
        GameObject.Find("lblUsuario").GetComponent<TextMeshProUGUI>().text = "Usuario: " + email;
        FBManager.getInstance().OnLoadExercices();
        FBManager.getInstance().OnLoadUsers();
        GameObject.Find("txtVersion").GetComponent<Text>().text = Application.version;        
    }
    public void Audio() {
        int actAudio = 0;
        if (GameObject.Find("ScrollAudio").GetComponent<Scrollbar>().value >= .5) {
            GameObject.Find("ScrollAudio").GetComponent<Scrollbar>().value = 1;
            GameObject.Find("Handle").GetComponent<Image>().color = Color.green;
            GameObject.Find("txtSt").GetComponent<TextMeshProUGUI>().text = "Activado";
            actAudio = 1;
        }
        else {
            GameObject.Find("ScrollAudio").GetComponent<Scrollbar>().value = 0;
            GameObject.Find("Handle").GetComponent<Image>().color = Color.white;
            GameObject.Find("txtSt").GetComponent<TextMeshProUGUI>().text = "Desactivado";
        }
        Debug.Log("Audio: " + actAudio);
        PlayerPrefs.SetInt("actAudio", actAudio);
    }

    public void ShowVersion() {
    }
    public void OnAuth() {
        if (txtAuth.text.Equals(this.txtPass.text)) {
            GameObject.Find("btnOpcion2").GetComponent<Button>().interactable = true;
            GameObject.Find("Panel2").GetComponent<RectTransform>().localScale = Vector3.zero;
            GameObject.Find("MainPanel").GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.75f, 1f);
        }
    }

    public void HideMenu() {
        GameObject.Find("MainPanel").GetComponent<RectTransform>().localScale = Vector3.zero;
    }
    public void ShowMenu() {
        GameObject.Find("MainPanel").GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.75f, 1f);
    }

    public void HidePanel2() {
        GameObject.Find("Panel2").GetComponent<RectTransform>().localScale = Vector3.zero;
    }
    public void ShowPanel2()
    {
        GameObject.Find("Panel2").GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.75f, 1f);
    }
}