using Mono.Data.Sqlite;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;

public class DatosLewis1 : MonoBehaviour
{
    private static DatosLewis1 instance;
    private string connection;
    private IDbConnection dbCon1;
    private IDbCommand dbCmd1;
    private IDataReader reader1;
    List<Electron> lista;

    private DatosLewis1()
    {
        CrearListaElementos();
    }

    public static DatosLewis1 Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new DatosLewis1();
            }
            return instance;
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

    public List<Electron> CrearListaElementos()
    {
        lista = new List<Electron>();
        AbrirConexion();
        dbCmd1 = dbCon1.CreateCommand(); //   La consulta de la tabla en la base de datos

        dbCmd1.CommandText = "select * from EnlacesIonicos order by Dificultad";
        reader1 = dbCmd1.ExecuteReader();     //Ejecuta la consulta
        while (reader1.Read())
        {
            Electron r3 = new Electron
            {
                IdMarcador = reader1.GetInt32(0),
                Compuesto = reader1.GetString(1),
                Dificultad = reader1.GetInt32(2),
                Formula = reader1.GetString(3),
                DivFormula = reader1.GetString(4),
                Elem1 = reader1.GetString(5),
                ElecElem1 = reader1.GetInt32(6),
                Elem2 = reader1.GetString(7),
                ElecElem2 = reader1.GetInt32(8),
                Elem3 = reader1.GetString(9),
                ElecElem3 = reader1.GetInt32(10)
            };
            lista.Add(r3);
        }
        reader1.Close();
        reader1 = null;
        CerrarConexion();
        string stri = "";
        lista.ForEach(item => stri += item.Compuesto + ", ");
        Debug.Log(stri);
        return lista;
    }

    public List<Electron> RecuperarLista()
    {
        return lista;
    }

    public Electron GetAyuda(string compuesto)
    {
        var temp = lista.Where(x => x.Compuesto == compuesto);
        return temp.FirstOrDefault();
    }

    public Electron GetInformacionCompuesto(int dificultad)
    {
        var temp = lista.Where(x => x.Dificultad == dificultad);
        return temp.FirstOrDefault();
    }
}