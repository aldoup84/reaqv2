using Mono.Data.Sqlite;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class DatosCHONPS : MonoBehaviour{
    private static DatosCHONPS instance;
    
    List<Pregunta> lista;
    int indTemp = 0;

    private DatosCHONPS() {
        CrearListaPreguntas();
    }

    public static DatosCHONPS Instance {
        get {
            if (instance == null) {
                GameObject obj = new GameObject();
                instance = obj.AddComponent<DatosCHONPS>();
            }
            return instance;
        }
    }
    public async Task<List<Pregunta>> CrearListaPreguntas(){
        lista = await FBManager.getInstance().OnLoadPreguntas();
        Debug.Log(lista.Count());
        return lista;
    }

    public List<Pregunta> RecuperarLista() {
        return lista;
    }

    public Pregunta GetAyuda(string marcador) {
        var temp = lista.Where(x => x.Marcador == marcador);
        return temp.FirstOrDefault();
    }

    public Pregunta GetInformacionPregunta(string marcador) {
        Debug.Log("Marcador:  " + marcador);
        var temp = lista.Where(x => x.Marcador == marcador);
        return temp.FirstOrDefault();
    }

    public Pregunta GetInformacionPregunta(List<Pregunta> lista, string marcador)
    {
        var temp = lista.Where(x => x.Marcador == marcador);
        return temp.ElementAt(indTemp);
    }

    public int UpdateIndex()
    {
        if (indTemp < 2)
            indTemp++;
        else
            indTemp = 0;
        return indTemp;
    }

    public void ResetIndex()
    {
        this.indTemp = 0;
    }

    public Pregunta GetFirstQ() {
        return lista.FirstOrDefault();
    }
}