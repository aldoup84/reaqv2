using System.Collections;
using TMPro;
using UnityEngine;

public class Info : MonoBehaviour
{

    TMP_Dropdown drop;
    TextMeshProUGUI txtInst;

    private void Start()
    {
        drop = GameObject.Find("DropCompuesto").GetComponent<TMP_Dropdown>();
        txtInst = GameObject.Find("txtTarget").GetComponent<TextMeshProUGUI>();
        txtInst.text = string.Empty;
    }

    public void OnChange()
    {
        StartCoroutine("HidePanel");
    }

    public void OnUpdate()
    {
        txtInst.text = drop.captionText.text;
    }

    IEnumerator HidePanel()
    {
        yield return new WaitForSeconds(2);
        GameObject.Find("Panel3").GetComponent<RectTransform>().localScale = Vector3.zero;
    }
}