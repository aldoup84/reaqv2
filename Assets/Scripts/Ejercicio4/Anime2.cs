using UnityEngine;
using UnityEngine.UI;

public class Anime2 : MonoBehaviour
{
  
    
    bool hide = false;

    
    public void HidePanel()
    {
        hide = !hide;
        Debug.Log("hide:  " + hide);
        if (hide)
        {
            GameObject.Find("Panel").GetComponent<RectTransform>().localScale = Vector3.zero;
            GameObject.Find("hideText").GetComponent<Text>().text = "Instrucciones";
        }
        else
        {
            GameObject.Find("Panel").GetComponent<RectTransform>().localScale = Vector3.one;
            GameObject.Find("hideText").GetComponent<Text>().text = "Ocultar";
        }
    }
}