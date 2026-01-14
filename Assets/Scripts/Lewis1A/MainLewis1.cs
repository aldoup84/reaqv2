using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainLewis1 : MonoBehaviour
{
    DatosLewis1 datosLewis1;
    List<Electron> lista;
    TextMeshProUGUI txtElem;
    Text txtTarget;
    double tiempo;
    TMP_Dropdown drop;
    void Start()
    {
        txtElem = GameObject.Find("txtElem").GetComponent<TextMeshProUGUI>();
        drop = GameObject.Find("Dropdown").GetComponent<TMP_Dropdown>();
        tiempo = 0f;

        datosLewis1 = DatosLewis1.Instance;
        lista = datosLewis1.CrearListaElementos();

        List<string> lsX = new List<string>();
        lista.ForEach(b => lsX.Add(b.Compuesto));
        drop.AddOptions(lsX);
        lsX = null;
        drop.value = 0;
        txtElem.text = drop.captionText.text;
        tiempo = 0f;
    }

    IEnumerator ControlTiempo()
    {
        tiempo += Time.deltaTime;
        yield return new WaitForSeconds(1f);
    }
}
