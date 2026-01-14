using Mono.Data.Sqlite;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class DatosEj4 : MonoBehaviour {
    private static DatosEj4 instance;
    /*
    private string connection;
    private IDbConnection dbCon1;
    private IDbCommand dbCmd1;
    private IDataReader reader1;
    */
    List<Enlace> listEnlaces;
    List<Ayuda> listHelps;
    int indTemp = 0;

    private DatosEj4() {
        CrearListaEnlaces();
    }

    public static DatosEj4 Instance {
        get {
            if (instance == null) {
                GameObject obj = new GameObject();
                instance = obj.AddComponent<DatosEj4>();
            }
            return instance;
        }
    }

    public async Task<List<Enlace>> CrearListaEnlaces() {
        listEnlaces = await FBManager.getInstance().OnLoadBonds("Ionico");
        Debug.Log(listEnlaces.Count());
        return listEnlaces;
    }
    public async Task<List<Ayuda>> CrearListaAyudas() {
        listHelps = await FBManager.getInstance().OnLoadHelps("Ionico");
        Debug.Log(listHelps.Count());
        return listHelps;
    }

    public Ayuda GetAyudaCov(List<Ayuda> listHelps, string compuesto, int indTemp) {
        var temp = listHelps.Where(x => x.Compuesto == compuesto);
        return temp.ElementAt(indTemp);
    }

    public List<Enlace> RecuperarLista() {
        return listEnlaces;
    }

    public Enlace GetAyudaEnlace(string marcador) {
        var temp = listEnlaces.Where(x => x.Descripcion == marcador);
        return temp.FirstOrDefault();
    }

    public Ayuda GetAyuda(string compuesto) {
        var temp = listHelps.Where(x => x.Compuesto == compuesto);
        System.Random rnd = new System.Random();
        temp = temp.OrderBy(item => Random.value).ToList(); //ordenando la lista       
        return temp.FirstOrDefault();
    }

    public int UpdateIndex() {
        if (indTemp < 2)
            indTemp++;
        else
            indTemp = 0;
        return indTemp;
    }

    public void ResetIndex() {
        this.indTemp = 0;
    }

    public Ayuda GetAyuda(string compuesto, int indTemp) {
        var temp = listHelps.Where(x => x.Compuesto == compuesto);
        return temp.ElementAt(indTemp);
    }

    public Ayuda GetAyuda2(string compuesto, int indice) {
        var temp = listHelps.Where(x => x.Compuesto == compuesto);
        System.Random rnd = new System.Random();
        temp = temp.OrderBy(item => Random.value).ToList(); //ordenando la lista       
        return temp.FirstOrDefault();
    }

    public Enlace GetInformacionEnlaces(List<Enlace> l1, string compuesto) {
        var temp = l1.Where(x => x.Descripcion == compuesto);
        return temp.FirstOrDefault();
    }

    public Enlace GetInformacionEnlaces(string compuesto) {
        var temp = listEnlaces.Where(x => x.Descripcion == compuesto);
        return temp.FirstOrDefault();
    }
}

/*
 public List<Ayuda> CrearListaHelps()
    {
        listHelps = new List<Ayuda>();
        AbrirConexion();
        dbCmd1 = dbCon1.CreateCommand(); //   La consulta de la tabla en la base de datos

        dbCmd1.CommandText = "select * from Helps where Tipo = 'Ionico' order by IdAyuda";
        reader1 = dbCmd1.ExecuteReader();     //Ejecuta la consulta
        while (reader1.Read())
        {
            Ayuda h = new Ayuda
            {
                IdAyuda = reader1.GetInt32(0).ToString(),
                IdEjercicio = reader1.GetInt32(1).ToString(),
                IdNivel = reader1.GetInt32(2).ToString(),
                Descripcion = reader1.GetString(3),
                Tipo = reader1.GetString(4),
                Compuesto = reader1.GetString(5)
            };
            listHelps.Add(h);
        }
        Debug.Log(listHelps.Count);
        reader1.Close();
        reader1 = null;
        CerrarConexion();
        return listHelps;
    }
}
 
    public void AbrirConexion()
    {
        string p = "BDQuimica.db";
        string filePath;
        if (Application.isMobilePlatform)
        {
            filePath = Application.persistentDataPath + "/" + p;
        }
        else
        {
            filePath = Application.streamingAssetsPath + "/" + p;
        }
        connection = "URI=file:" + filePath;
        Debug.Log("Conectando a la base de datos  " + connection);
        dbCon1 = new SqliteConnection(connection);
        dbCon1.Open();
    }

    public void CerrarConexion()
    {
        dbCmd1.Dispose();
        dbCmd1 = null;
        dbCon1.Close();
        dbCon1 = null;
        Debug.Log("Cerrando Conexion");
    }
    /*
    public List<Enlace> CrearListaEnlaces()
    {
        listEnlaces = new List<Enlace>();
        AbrirConexion();
        dbCmd1 = dbCon1.CreateCommand(); //   La consulta de la tabla en la base de datos

        dbCmd1.CommandText = "select * from Enlaces where Tipo = 'Ionico' order by IdEnlace";
        reader1 = dbCmd1.ExecuteReader();     //Ejecuta la consulta
        while (reader1.Read())
        {
            Enlace r3 = new Enlace
            {
                IdEnlace = reader1.GetInt32(0),
                Descripcion = reader1.GetString(1),
                Tipo = reader1.GetString(2),
                Formula = reader1.GetString(3),
                Markers = reader1.GetString(4),
                Elem1 = reader1.GetString(5),
                Elect1 = reader1.GetFloat(6),
                Elem2 = reader1.GetString(7),
                Elect2 = reader1.GetFloat(8),
                Elem3 = reader1.GetString(9),
                Elect3 = reader1.GetFloat(10)
            };
            listEnlaces.Add(r3);
        }
        reader1.Close();
        reader1 = null;
        CerrarConexion();
        string stri = "";
        listEnlaces.ForEach(item => stri += item.Descripcion + ", ");
        return listEnlaces;
    }
*/