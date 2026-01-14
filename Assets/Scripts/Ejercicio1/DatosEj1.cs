using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
public class DatosEj1{
    private static DatosEj1 instance;
    List<Elemento> lista;
    List<Compuesto> listaCompuestos;

    private DatosEj1(){
        CrearListaElementos();
        CrearListaCompuestos();
    }


    public static DatosEj1 Instance{
        get{
            if (instance == null) {
                instance = new DatosEj1();
            }
            return instance;
        }
    }

    public async Task<List<Elemento>> CrearListaElementos(){
        lista = await FBManager.getInstance().OnLoadIDElementos();
        //Debug.Log(lista.Count());
        return lista;
    }

    public async Task<List<Compuesto>> CrearListaCompuestos(){
        listaCompuestos = await FBManager.getInstance().OnLoadIDCompuestos();
   //     Debug.Log(listaCompuestos.Count());
        return listaCompuestos;
    }

    public Ayuda GetAyudaCov(List<Ayuda> listHelps, string compuesto, int indTemp)
    {
        var temp = listHelps.Where(x => x.Compuesto == compuesto);
        return temp.ElementAt(indTemp);
    }

    public List<Elemento> RecuperarLista(){
        return lista;
    }
    public List<Compuesto> RecuperarLista2()
    {
        return listaCompuestos;
    }

    public Elemento getAyuda(string marcador)
    {
        var temp = lista.Where(x => x.descripcion == marcador);
        return temp.FirstOrDefault();
    }

    public Compuesto GetInformacionCompuesto(string marcador)
    {
        var temp = listaCompuestos.Where(x => x.Descripcion == marcador);
        return temp.FirstOrDefault();
    }
}