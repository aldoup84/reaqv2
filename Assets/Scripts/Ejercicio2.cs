public class Ejercicio2
{
    public string app, ejercicio, dificultad, fecha, idUsuario, hora;
    public float tiempo;
    public int aciertos, errores, ayudas, puntos, nivel;
    public Ejercicio2() { }
    public Ejercicio2(string app, string ejercicio, string idUsuario, string dificultad, int aciertos, int errores, int ayudas, float tiempo,
        int puntos, int nivel, string fecha, string hora)
    {
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