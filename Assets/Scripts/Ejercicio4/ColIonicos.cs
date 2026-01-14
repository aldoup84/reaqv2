using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ColIonicos : MonoBehaviour
{
    TextMeshProUGUI txtElem1, txtElem2, txtCompuesto, txtMsg, txtErrores, txtAyudas, txtLogs;
    TMP_Dropdown drop;
    double tiempo;
    public GameObject sal, kcl, mgo, lif, zno, error;
    bool mostrar = false;
    int aciertos, errores, ayudas;
    public string log;

    private void Start()
    {
        txtElem1 = GameObject.Find("txtElem1").GetComponent<TextMeshProUGUI>();
        txtElem2 = GameObject.Find("txtElem2").GetComponent<TextMeshProUGUI>();
        txtCompuesto = GameObject.Find("txtCompuesto").GetComponent<TextMeshProUGUI>();
        txtErrores = GameObject.Find("txtErrores").GetComponent<TextMeshProUGUI>();
        txtAyudas = GameObject.Find("txtAyudas").GetComponent<TextMeshProUGUI>();
        txtLogs = GameObject.Find("txtLogs").GetComponent<TextMeshProUGUI>();
        txtMsg = GameObject.Find("txtMsg").GetComponent<TextMeshProUGUI>();
        drop = GameObject.Find("Dropdown").GetComponent<TMP_Dropdown>();
        sal.transform.gameObject.SetActive(false);
    }

    public void Select(string modelo, bool active)
    {
        GameObject aux = null;
        switch (modelo)
        {
            case "Cloruro de Sodio": aux = sal; break;
            case "Cloruro de Potasio": aux = kcl; break;
            case "Oxido de Magnesio": aux = mgo; break;
            case "Fluoruro de Litio": aux = lif; break;
            case "Oxido de Zinc": aux = zno; break;
            case "Error": aux = error; break;
        }
        aux.transform.gameObject.SetActive(active);
        if (PlayerPrefs.GetInt("actAudio") == 0)
        {
            aux.GetComponent<AudioSource>().mute = true;
        }
    }

    public IEnumerator OnErrorDetected()
    {

        Select("Error", true);
        yield return new WaitForSeconds(4f);
        Select("Error", false);
        StartCoroutine("HideAnimation");
        tiempo = 0;
        if (drop.value == 4)
        {
            GameObject.Find("TextF").GetComponent<Text>().text = "Finalizar";
        }
    }


    private void OnTriggerStay(Collider other)
    {
        GameObject.Find(this.name).GetComponent<MeshRenderer>().enabled = false;
        GameObject.Find(other.name).GetComponent<MeshRenderer>().enabled = false;
        tiempo += Time.deltaTime;

        if (tiempo > 1f && mostrar)
        {
            mostrar = false;
            Select(txtCompuesto.text, true);
            tiempo = 0;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Select(txtCompuesto.text, false);
        StartCoroutine("HideAnimation");
        tiempo = 0;
        if (drop.value == 4)
        {
            GameObject.Find("TextF").GetComponent<Text>().text = "Finalizar";
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        tiempo = 0;
        string comp = GameObject.Find("txtCompuesto").GetComponent<TextMeshProUGUI>().text;

        string elem1 = this.name;
        string elem2 = other.name;

        // Enlace c = DatosEj4.Instance.GetInformacionEnlaces(comp);// obtenemos los valores del compuesto a buscar
        Enlace c = FBManager.getInstance().GetInformacion(comp);// obtenemos los valores del compuesto a buscar

        if (elem1.Equals(c.Elem1) && elem2.Equals(c.Elem2) || elem1.Equals(c.Elem2) && elem2.Equals(c.Elem1))
        {
            if (!mostrar)
            {     //Si esta visible el objeto, no hacer nada
                Debug.Log("Haciendo visible el objeto 3D");
                mostrar = true;
            }
            txtMsg.text = "El " + comp + " se compone de: " + c.Elem1 + ", con " + c.Elect1 + "  de Electronegatividad,  " + c.Elem2 + " con " + c.Elect2 + " de Electronegatividad";
            Select(txtCompuesto.text, true);

            GameObject.Find("btnNext").GetComponent<RectTransform>().localScale = Vector3.one;

            if (GameObject.Find("btnMainMenu").GetComponent<RectTransform>().localScale == Vector3.one)
            {
                GameObject.Find("btnNext").GetComponent<RectTransform>().localPosition = GameObject.Find("btnNext2").GetComponent<RectTransform>().localPosition;
                //Debug.Log("Poniendolo en lugar del Main menu");
            }
            else
            {
                GameObject.Find("btnNext").GetComponent<RectTransform>().localPosition = GameObject.Find("btnMainMenu").GetComponent<RectTransform>().localPosition;
                //Debug.Log("Poniendolo al final");
            }

        }
        else
        {
            StartCoroutine("OnErrorDetected");
            txtLogs.text += "E";
            errores = Convert.ToInt32(txtErrores.text);
            txtMsg.text = string.Empty;
            Debug.Log("Marcadores erroneos....");
            GameObject.Find("pnlMsg").GetComponent<AudioSource>().Play();
            mostrar = false;
            errores++;
            txtLogs.text += "E";
            txtErrores.text = errores.ToString();
            if (!(txtElem1.text.Equals(elem1) && (txtElem1.text.Equals(elem2)) && (txtElem2.text.Equals(elem1) && (txtElem2.text.Equals(elem2)))))
            {
                txtMsg.text = "Los elementos " + elem1 + " y " + elem2 + " no forman parte del compuesto " + c.Descripcion;
                Debug.Log("Ntrando");
            }
            else if (!(txtElem1.text.Equals(elem2) || (txtElem2.text.Equals(elem2))))
            {
                txtMsg.text = "El elemento " + elem2 + " no forma parte del compuesto " + c.Descripcion;
            }
            else if (!(txtElem1.text.Equals(elem1) || (txtElem2.text.Equals(elem1))))
            {
                txtMsg.text = "El elemento " + elem1 + " no forma parte del compuesto " + c.Descripcion + ". \n";
            }
        }
        StartCoroutine("Mostrar");
    }

    IEnumerator Mostrar()
    {
        GameObject.Find("pnlMsg").GetComponent<RectTransform>().localScale = Vector3.one;
        yield return new WaitForSeconds(0f);
    }

    IEnumerator HideAnimation()
    {
        yield return new WaitForSeconds(0f);
        GameObject.Find("pnlMsg").GetComponent<RectTransform>().localScale = Vector3.zero;
        Select(txtCompuesto.text, false);
    }
}