using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class DatosEj6 : MonoBehaviour {
    private static DatosEj6 instance;  
    List<Enlace> listEnlaces;
    List<Ayuda> listHelps;
    List<Ayuda> listHelp2;
    int indTemp = 0;

    private DatosEj6() {
        CrearListaEnlaces();
    }

    public static DatosEj6 Instance {
        get {
            if (instance == null) {
                GameObject obj = new GameObject();
                instance = obj.AddComponent<DatosEj6>();
            }
            return instance;
        }
    }

    public async Task<List<Enlace>> CrearListaEnlaces() {
        listEnlaces = await FBManager.getInstance().OnLoadBonds("Metálico");
        Debug.Log(listEnlaces.Count());
        return listEnlaces;
    }

    public async Task<List<Ayuda>> CrearListaAyudas() {
        listHelps = await FBManager.getInstance().OnLoadHelps("Metalico");
        Debug.Log(listHelps.Count());
        return listHelps;
    }

    public Ayuda GetAyudaMet(List<Ayuda> listHelps, string compuesto, int indTemp)
    {
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

    public Ayuda GetAyuda(string compuesto, int indTemp){
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
        Debug.Log("Amtes de continuar  " + l1.Count());
        var temp = l1.Where(x => x.Descripcion == compuesto);
        return temp.FirstOrDefault();
    }

    public Enlace GetInformacionEnlaces(string compuesto) {
        Debug.Log("COmpuesto: " + compuesto);
        var temp = listEnlaces.Where(x => x.Descripcion == compuesto);
        return temp.FirstOrDefault();
    }
}