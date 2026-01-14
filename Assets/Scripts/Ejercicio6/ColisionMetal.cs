using System.Collections;
using TMPro;
using UnityEngine;

public class ColisionMetal : MonoBehaviour
{
    TextMeshProUGUI txtElem1, txtElem2, txtCompuesto, txtMsg;
    TMP_Dropdown drop;
    double tiempo;
    public GameObject acero, cao, laton;
    bool mostrar = false;

    private void Start()
    {
        txtElem1 = GameObject.Find("txtElem1").GetComponent<TextMeshProUGUI>();
        txtElem2 = GameObject.Find("txtElem2").GetComponent<TextMeshProUGUI>();
        txtCompuesto = GameObject.Find("txtCompuesto").GetComponent<TextMeshProUGUI>();
        txtMsg = GameObject.Find("txtMsg").GetComponent<TextMeshProUGUI>();
        drop = GameObject.Find("Dropdown").GetComponent<TMP_Dropdown>();
        acero.transform.gameObject.SetActive(false);
    }

    public void Select(string modelo, bool active)
    {
        GameObject aux = null;
        switch (modelo)
        {
            case "Acero": aux = acero; break;
            case "Latón": aux = laton; break;
            case "Oxido de Calcio": aux = cao; break;
        }
        aux.transform.gameObject.SetActive(active);
    }

    private void OnTriggerStay(Collider other)
    {
        tiempo += Time.deltaTime;
        if (tiempo > 2f && mostrar)
        {
            mostrar = false;
            Select(txtCompuesto.text, true);
            StartCoroutine("HideAnimation");
            tiempo = 0;

        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(this.name + "  esta colisionado con : " + other.name);
        tiempo = 0;
        string comp = GameObject.Find("txtCompuesto").GetComponent<TextMeshProUGUI>().text;

        string elem1 = this.name;
        string elem2 = other.name;
        //Debug.Log("elem1: " + elem1 + ";  Elem2: " + elem2 + "  ; Compuesto: " + comp);
        Enlace c = DatosEj6.Instance.GetInformacionEnlaces(comp);// obtenemos los valores del compuesto a buscar

        if ((txtElem1.text.Equals(elem1) || (txtElem2.text.Equals(elem1)) && (txtElem1.text.Equals(elem2) || (txtElem2.text.Equals(elem2)))))
        {
            //Si esta visible el objeto, no hacer nada
            if (!mostrar)
            {
                Debug.Log("Correcto, se incrementa en uno la lista y esta en espera de la animacion ");
                mostrar = true;
            }
            txtMsg.text = "El " + comp + " esta formado por el elemento: " + c.Elem1 + ", el cual tiene " +
                c.Elect1 + "  de Electronegatividad y el elemento " + c.Elem2 + " tiene " + c.Elect2 + " de Electronegatividad";
        }
        else
        {
            Debug.Log("Marcadores erroneos....");
            mostrar = false;
            txtMsg.text = string.Empty;
            if (!(txtElem1.text.Equals(elem1) || (txtElem2.text.Equals(elem1))))
            {
                txtMsg.text = "El elemento " + elem1 + " no forma parte del compuesto " + c.Descripcion + ". \n";
            }
            if (!(txtElem1.text.Equals(elem2) || (txtElem2.text.Equals(elem2))))
            {
                txtMsg.text += "El elemento " + elem2 + " no forma parte del compuesto " + c.Descripcion;
            }
        }
        StartCoroutine("Mostrar");
    }

    IEnumerator Mostrar()
    {
        yield return new WaitForSeconds(1.5f);
        GameObject.Find("pnlMsg").GetComponent<RectTransform>().localScale = Vector3.one;
        yield return new WaitForSeconds(5f);
        GameObject.Find("pnlMsg").GetComponent<RectTransform>().localScale = Vector3.zero;
    }

    IEnumerator HideAnimation()
    {
        yield return new WaitForSeconds(5f);
        GameObject.Find("btnNext").GetComponent<RectTransform>().localScale = Vector3.one;
        Select(txtCompuesto.text, false);
    }
}