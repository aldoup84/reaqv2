using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class DatosEj2{
    private static DatosEj2 instance;   
    List<Elemento> lista;
    List<Compuesto> listaCompuestos;

    private DatosEj2(){
        CrearListaElementos();
        CrearListaCompuestos();
    }

    public static DatosEj2 Instance{
        get{
            if (instance == null) {
                instance = new DatosEj2();
            }
            return instance;
        }
    }
    public async Task<List<Elemento>> CrearListaElementos(){
        lista = await FBManager.getInstance().OnLoadIDElementos();
        //Debug.Log(lista.Count());
        return lista;
    }

    public async Task<List<Compuesto>> CrearListaCompuestos()
    {
        listaCompuestos = await FBManager.getInstance().OnLoadIDCompuestos();
        //     Debug.Log(listaCompuestos.Count());
        return listaCompuestos;
    }
    public List<Elemento> RecuperarLista(){
        return lista;
    }

    public List<Compuesto> RecuperarListaCompuesto(){
        return listaCompuestos;
    }

    public Elemento getAyuda(string marcador){
        var temp = lista.Where(x => x.descripcion == marcador);
        return temp.FirstOrDefault();
    }
   

    public Compuesto GetInformacionCompuesto(string marcador)
    {
        var temp = listaCompuestos.Where(x => x.Descripcion == marcador);
        return temp.FirstOrDefault();
    }

}