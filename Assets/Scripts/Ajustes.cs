using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Ajustes : MonoBehaviour
{
    List<MedidasPanel> lista;
    public RectTransform btn, btn2, btn3;
    private void Start()
    {
        Canvas.ForceUpdateCanvases();
        lista = new List<MedidasPanel>{
                new MedidasPanel("2960 x 1440", 2960f,  1440f,  712f,   -216f,  -34f,   -452f),
                new MedidasPanel("1024 x 768",  1024f,  768f,   270f,   -58f,   -342f,  -269f),
                new MedidasPanel("800 x 600",   800f,   480f,   533f,   -57.5f, -70f,   -270f),
                new MedidasPanel("1280 x 720", 1280f,   720f,   609f,   -72f,   -7f,    -293f),
                new MedidasPanel("1920 x 1080", 1920f,  1080f,  609f,   -72f,   -7f,    -293f),
                new MedidasPanel("2160 x 1080", 2160f,  1080f,  683f,   -197f,  -25f,   -418f),
                new MedidasPanel("2560 x 1440", 2560f,  1440f,  423f,    276f, 1435f,    545f),
                new MedidasPanel("2960 x 1440", 2960f,  1440f,  684f,   -207f,  -28f,   -459f),
                new MedidasPanel("16:09",       16f,    9f,     590f,   -95f,   -26f,   -295f),
                new MedidasPanel("18:09",       18f,    9f,     689f,   -198f,  -26f,   -418f)
             };
        Ajustar();
    }
    public void Ajustar()
    {
        //  Debug.Log(btn.localPosition + "  Escala: " + btn.localScale);
        string r = Screen.width + " x " + Screen.height;
        MedidasPanel m = this.Medidas(r);

        btn.offsetMin = new Vector2(m.Left, m.Bottom);
        btn.offsetMax = new Vector2(-m.Right, -m.Top);
        btn2.offsetMin = new Vector2(m.Left, m.Bottom);
        btn2.offsetMax = new Vector2(-m.Right, -m.Top);
        btn3.offsetMin = new Vector2(m.Left, m.Bottom);
        btn3.offsetMax = new Vector2(-m.Right, -m.Top);
    }

    public MedidasPanel Medidas(string resolucion)
    {
        var x = lista.Where(y => y.Resolucion.Equals(resolucion));
        return x.FirstOrDefault();
    }
}
