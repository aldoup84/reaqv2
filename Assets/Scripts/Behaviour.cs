public class Behaviour
{

    public string IdUser { get; set; }
    public string IdEjercicio { get; set; }
    public string Respuesta { get; set; }
    public string Fecha { get; set; }

    public string Hora { get; set; }
    public string app { get; set; }

    public Behaviour(string idUser, string idEjercicio, string respuesta, string fecha, string hora, string app)
    {
        IdUser = idUser;
        IdEjercicio = idEjercicio;
        Respuesta = respuesta;
        Fecha = fecha;
        Hora = hora;
        this.app = app;
    }
}
