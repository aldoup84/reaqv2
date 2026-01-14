public class Ejercicio
{
    public string folio, app, ejercicio, dificultad, fecha, idUsuario, hora;
    public float tiempo;
    public int aciertos, errores, ayudas, puntos, nivel;
    public Ejercicio() { }
    public Ejercicio(string folio, string app, string ejercicio, string idUsuario, string dificultad, int aciertos, int errores, int ayudas, float tiempo,
        int puntos, int nivel, string fecha, string hora)
    {

        this.folio = folio;
        this.app = app;
        this.ejercicio = ejercicio;
        this.idUsuario = idUsuario;
        this.dificultad = dificultad;
        this.aciertos = aciertos;
        this.errores = errores;
        this.ayudas = ayudas;
        this.tiempo = tiempo;
        this.puntos = puntos;
        this.nivel = nivel;
        this.fecha = fecha;
        this.hora = hora;
    }
}