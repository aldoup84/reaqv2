using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class ColStay : MonoBehaviour
{
    TextMeshProUGUI txtElem1, txtElem2, txtCompuesto, txtMsg, txtErrores, txtAyudas;
    TMP_Dropdown drop;
    double tiempo;
    public GameObject water, hcl, co2, nh3, ch4;
    bool mostrar = false;
    int aciertos, errores, ayudas;

    private void Start()
    {
        txtElem1 = GameObject.Find("txtElem1").GetComponent<TextMeshProUGUI>();
        txtElem2 = GameObject.Find("txtElem2").GetComponent<TextMeshProUGUI>();
        txtErrores = GameObject.Find("txtErrores").GetComponent<TextMeshProUGUI>();
        txtAyudas = GameObject.Find("txtAyudas").GetComponent<TextMeshProUGUI>();
        txtCompuesto = GameObject.Find("txtCompuesto").GetComponent<TextMeshProUGUI>();
        txtMsg = GameObject.Find("txtMsg").GetComponent<TextMeshProUGUI>();
        drop = GameObject.Find("Dropdown").GetComponent<TMP_Dropdown>();
        water.transform.gameObject.SetActive(false);
    }

    public void Select(string modelo, bool active)
    {
        GameObject aux = null;
        switch (modelo)
        {
            case "Agua": aux = water; break;
            case "Acido Clorhidrico": aux = hcl; break;
            case "Dioxido de Carbono": aux = co2; break;
            case "Amoniaco": aux = nh3; break;
            case "Metano": aux = ch4; break;
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
    private void OnTriggerExit(Collider other)
    {
        //        Debug.Log(this.name + "  esta colisionado con : " + other.name);
        tiempo = 0;
        string comp = GameObject.Find("txtCompuesto").GetComponent<TextMeshProUGUI>().text;

        string elem1 = this.name;
        string elem2 = other.name;

        Enlace c = DatosEj3.Instance.GetInformacionEnlaces(comp);// obtenemos los valores del compuesto a buscar

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
            errores = Convert.ToInt32(txtErrores.text);
            Debug.Log("Marcadores erroneos....");
            mostrar = false;
            errores++;
            txtErrores.text = errores.ToString();
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