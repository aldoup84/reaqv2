using System.Collections;
using TMPro;
using UnityEngine;

public class ReactColision : MonoBehaviour
{
    public GameObject explosion, efervecente, sp1, sp2;
    TextMeshProUGUI txtCompuesto;

    public void Start()
    {
        txtCompuesto = GameObject.Find("txtCompuesto").GetComponent<TextMeshProUGUI>();
    }

    public void Select(string modelo, bool active)
    {
        GameObject aux = null;
        switch (modelo)
        {
            case "Explosion": aux = explosion; break;
            case "Efervescente": aux = efervecente; break;
        }
        aux.transform.gameObject.SetActive(active);
    }

    public void OnTriggerStay(Collider other)
    {
        sp1.GetComponent<MeshRenderer>().enabled = false;
        sp2.GetComponent<MeshRenderer>().enabled = false;
    }

    public void OnTriggerExit(Collider other)
    {
        Select(txtCompuesto.text, false);
        StartCoroutine("Hide");
        sp1.GetComponent<MeshRenderer>().enabled = true;
        sp2.GetComponent<MeshRenderer>().enabled = true;
    }


    public void OnTriggerEnter(Collider other)
    {
        Select(txtCompuesto.text, true);
        GameObject.Find("pnlMsg").GetComponent<RectTransform>().localScale = Vector3.one;
    }

    public IEnumerator Hide()
    {
        yield return new WaitForSeconds(0f);
        GameObject.Find("btnNext").GetComponent<RectTransform>().localScale = Vector3.one;
        GameObject.Find("pnlMsg").GetComponent<RectTransform>().localScale = Vector3.zero;
        Select(txtCompuesto.text, false);
    }
}
