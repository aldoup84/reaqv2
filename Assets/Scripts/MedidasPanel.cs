public class MedidasPanel
{
    public string Resolucion { get; set; }
    public float height { get; set; }
    public float width { get; set; }
    public float Top { get; set; }
    public float Bottom { get; set; }
    public float Left { get; set; }
    public float Right { get; set; }

    public MedidasPanel(string resolucion, float height, float width, float left, float top, float right, float bottom)
    {
        Resolucion = resolucion;
        this.height = height;
        this.width = width;
        Left = left;
        Top = top;
        Right = right;
        Bottom = bottom;
    }
}
