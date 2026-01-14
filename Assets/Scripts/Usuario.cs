public class Usuario
{
    public string idUsuario, nombre, email, password, escuela, grupo, genero, tipoEscuela;
    public int edad;

    public Usuario(string idUsuario, string nombre, string email, string password, string escuela, string grupo, string genero, string tipoEscuela, int edad)
    {
        this.idUsuario = idUsuario;
        this.nombre = nombre;
        this.email = email;
        this.password = password;
        this.escuela = escuela;
        this.grupo = grupo;
        this.genero = genero;
        this.tipoEscuela = tipoEscuela;
        this.edad = edad;
    }
}