using TMPro;
using UnityEngine;

public class InfoEj2 : MonoBehaviour
{

    TMP_Dropdown drop;
    TextMeshProUGUI txtInst;

    private void Start()
    {
        drop = GameObject.Find("Dropdown").GetComponent<TMP_Dropdown>();
        txtInst = GameObject.Find("txtElem2A").GetComponent<TextMeshProUGUI>();
    }

    public void OnChange()
    {
        txtInst.text = drop.captionText.text;
    }
}
