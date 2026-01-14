using Mono.Data.Sqlite;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class DatosEj7 : MonoBehaviour
{
    private static DatosEj7 instance;
    List<React> listReact;

    private DatosEj7() {
        CrearListaReacciones();
    }

    public static DatosEj7 Instance {
        get {
            if (instance == null) {
                GameObject obj = new GameObject();
                instance = obj.AddComponent<DatosEj7>();
            }
            return instance;
        }
    }

    public async Task<List<React>> CrearListaReacciones() {
        listReact = await FBManager.getInstance().OnLoadReacciones();
        Debug.Log(listReact.Count());
        return listReact;
    }

   
    public List<React> RecuperarLista() {
        return listReact;
    }

    public React GetAyuda(string marcador) {
        var temp = listReact.Where(x => x.Descripcion == marcador);
        return temp.FirstOrDefault();
    }

    public React GetInfoReaccion(string compuesto) {
        var temp = listReact.Where(x => x.Descripcion == compuesto);
        return temp.FirstOrDefault();
    }

    public React GetInfoReaccion(List<React > l1, string compuesto){
        var temp = l1.Where(x => x.Descripcion == compuesto);
        return temp.FirstOrDefault();
    }
}