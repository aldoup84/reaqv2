using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ColisionMol : MonoBehaviour
{
    TextMeshProUGUI txtElem1, txtElem2, txtElem3, txtInfo2;
    TMP_Dropdown drop;

    List<Compuesto> lc;
    async void Start()
    {
        txtElem1 = GameObject.Find("txtElem1").GetComponent<TextMeshProUGUI>();
        txtElem2 = GameObject.Find("txtElem2").GetComponent<TextMeshProUGUI>();
        txtElem3 = GameObject.Find("txtElem3").GetComponent<TextMeshProUGUI>();
        txtInfo2 = GameObject.Find("txtInfo2").GetComponent<TextMeshProUGUI>();
        //lc = DatosEj2.Instance.RecuperarListaCompuesto();
        lc = await FBManager.getInstance().OnLoadIDCompuestos();
        drop = GameObject.Find("Dropdown").GetComponent<TMP_Dropdown>();
    }

    private void OnTriggerExit(Collider other)
    {
        string elem = GameObject.Find("txtElem2A").GetComponent<TextMeshProUGUI>().text;
        string comp = GameObject.Find("txtElem").GetComponent<TextMeshProUGUI>().text;
        Debug.Log("Colisionado con : " + other.name);

        if (other.tag.Equals("Clon"))
        {
            
            Compuesto c = FBManager.getInstance().GetIDComp(comp);// obtenemos los valores del compuesto a buscar
            Debug.Log("Compuesto:  " + c.Descripcion + "	Elem1: " + c.Elemento1 + "	Elem2: " + c.Elemento2 + "	Elem3: " + c.Elemento3);
            int nIndex = lc.IndexOf(c);

            if (!(txtElem1.text.Equals(elem) || (txtElem2.text.Equals(elem)) || txtElem3.text.Equals(elem)))
            {
                bool enc = false;
                if (c.Elemento1.Equals(elem))
                {
                    txtElem1.text = c.Elemento1;
                    //txtElem1.GetComponentInChildren<RawImage>().color = this.GetComponent<MeshRenderer>().material.color;
                    txtElem1.GetComponentInChildren<RawImage>().texture = this.GetComponent<MeshRenderer>().material.mainTexture;
                    txtInfo2.text = "El " + comp + " contiene " + c.AtomoElem1 + " atomo(s) de " + elem;
                    lc.ElementAt(nIndex).Elemento1 = "-";
                    enc = true;
                }
                else if (c.Elemento2.Equals(elem))
                {
                    txtElem2.GetComponent<TextMeshProUGUI>().text = c.Elemento2;
                    txtElem2.GetComponentInChildren<RawImage>().color = this.GetComponent<MeshRenderer>().material.color;
                    txtInfo2.text = "El " + comp + " contiene " + c.AtomoElem2 + " atomo(s) de " + elem;
                    lc.ElementAt(nIndex).Elemento2 = "-";
                    enc = true;
                }
                else if (c.Elemento3.Equals(elem))
                {
                    Debug.Log("Elemento 3");
                    txtElem3.text = c.Elemento3;
                    txtElem3.GetComponentInChildren<RawImage>().color = this.GetComponent<MeshRenderer>().material.color;
                    txtInfo2.text = "El " + comp + " contiene " + c.AtomoElem3 + " atomo(s) de " + elem;
                    lc.ElementAt(nIndex).Elemento3 = "-";
                    enc = true;
                }
                if (enc)
                {
                    StartCoroutine("Mostrar");
                }
                Debug.Log("Cambios al Compuesto:  " + c.Descripcion + "	Elem1: " + lc.ElementAt(nIndex).Elemento1 + "	Elem2: " + lc.ElementAt(nIndex).Elemento2 + "	Elem3: " + lc.ElementAt(nIndex).Elemento3);
            }
            if (!(txtElem1.text.Equals(elem) || (txtElem2.text.Equals(elem)) || txtElem3.text.Equals(elem)))
            {
                StartCoroutine("MostrarError");
                GameObject.Find("txtMsg").GetComponent<TextMeshProUGUI>().text = "El " + elem + " no es parte de este compuesto quimico";
            }
            if (lc.ElementAt(nIndex).Elemento1.Equals("-") && lc.ElementAt(nIndex).Elemento2.Equals("-") && lc.ElementAt(nIndex).Elemento3.Equals("-"))
            {
                StartCoroutine("Clear");
            }
        }
    }

    IEnumerator Clear()
    {
        yield return new WaitForSeconds(3);
        txtElem1.text = string.Empty;
        txtElem3.text = string.Empty;
        txtElem2.text = string.Empty;
        txtInfo2.text = string.Empty;
        txtElem1.GetComponentInChildren<RawImage>().color = Color.white;
        txtElem2.GetComponentInChildren<RawImage>().color = Color.white;
        txtElem3.GetComponentInChildren<RawImage>().color = Color.white;

        GameObject.Find("txtElem2A").GetComponent<TextMeshProUGUI>().text = "Cambiando el Objetivo";
        yield return new WaitForSeconds(2);
        GameObject.Find("txtElem2A").GetComponent<TextMeshProUGUI>().text = string.Empty;
        if (drop.value < drop.options.Count())
        {
            drop.value++;
            GameObject.Find("avance").GetComponent<Image>().fillAmount += 0.20f;
        }
        else
        {
            lc = DatosEj2.Instance.RecuperarListaCompuesto();
            drop.value = 0;
            Debug.Log("Reiniciando los parametros");
        }
        GameObject.Find("txtElem").GetComponent<TextMeshProUGUI>().text = drop.captionText.text;
        Debug.Log("Elemento Formado Correctamente");
        GameObject.Find("Panel4").GetComponent<RectTransform>().localScale = Vector3.zero;
    }

    IEnumerator Mostrar()
    {
        GameObject.Find("Panel4").GetComponent<RectTransform>().localScale = Vector3.one;
        yield return new WaitForSeconds(4f);
        GameObject.Find("Panel4").GetComponent<RectTransform>().localScale = Vector3.zero;
    }

    IEnumerator MostrarError()
    {
        GameObject.Find("pnlMsg").GetComponent<RectTransform>().localScale = Vector3.one;
        yield return new WaitForSeconds(4f);
        GameObject.Find("pnlMsg").GetComponent<RectTransform>().localScale = Vector3.zero;
    }
}