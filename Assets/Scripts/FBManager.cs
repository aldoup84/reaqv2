using Firebase.Extensions;
using Firebase.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FBManager{
    private string authPath = "";
    private static FBManager manager = null;
    private FirebaseFirestore db;
    private static Usuario user;
    private static string idUsuario;
    protected  static List<Enlace> listC;
    protected static List<Enlace> listIon;
    protected static List<Enlace> listMetal;
    protected static List<Ayuda> listHC;
    protected static List<Ayuda> listHIon;
    protected static List<Ayuda> listHM;
    protected static List<React> listR;
    protected static List<Pregunta> listP;
    protected static List<Elemento> listE;
    protected static List<Compuesto> listComp;

    private FBManager(){
        //db = FirebaseFirestore.GetInstance(Firebase.FirebaseApp.DefaultInstance);
        db = FirebaseFirestore.DefaultInstance;
    }

    public static FBManager getInstance(){
        if (manager == null) {
            manager = new FBManager();
        }
        return manager;
    }

    public bool SaveExercise(Ejercicio2 session) {
        Dictionary<string, object> sessionDict = new Dictionary<string, object>{
            { "app", session.app}, { "ejercicio", session.ejercicio}, { "idUsuario", session.idUsuario}, { "dificultad", session.dificultad}, { "aciertos", session.aciertos},
            { "errores", session.errores}, { "ayudas", session.ayudas}, { "tiempo", session.tiempo}, { "puntos", session.puntos}, { "nivel", session.nivel }, { "fecha", session.fecha},
            { "hora", session.hora}
        };
        DocumentReference doc = db.Collection("Ejercicios3").Document();
        doc.SetAsync(sessionDict);
        Debug.Log("Guardando en Firestore");
        return true;
    }

    public bool SaveBehaviour(Behaviour beh) {
        Dictionary<string, object> sessionDict = new Dictionary<string, object>{
            { "idUser", beh.IdUser }, { "idEjercicio", beh.IdEjercicio }, { "respuesta", beh.Respuesta}, { "fecha", beh.Fecha}, { "hora", beh.Hora}, { "app", beh.app} 
        };
        DocumentReference doc = db.Collection("Comportamientos2").Document();
        doc.SetAsync(sessionDict);
        Debug.Log("Guardando en Firestore");
        return true;
    }

    public bool SaveUser(Usuario usr) {
        Dictionary<string, object> sessionDict = new Dictionary<string, object>{
            { "idUsuario", usr.idUsuario}, { "nombre", usr.nombre}, { "email", usr.email}, { "password", usr.password}, { "grupo", usr.grupo},
            { "escuela", usr.escuela}, { "tipoEscuela", usr.tipoEscuela}, { "genero", usr.genero}, { "edad", usr.edad}
        };
        DocumentReference doc = db.Collection("Usuarios3").Document(usr.email);
        doc.SetAsync(sessionDict);
        user = usr;
        Debug.Log("Guardando en Firestore");
        return true;
    }

    public bool OnLoadExercices() {
        DocumentReference docRef = db.Collection("Permisos").Document("auth");
        docRef.GetSnapshotAsync().ContinueWithOnMainThread(task => {
            DocumentSnapshot snapshot = task.Result;
            if (snapshot.Exists) {                
                Dictionary<string, object> activo = snapshot.ToDictionary();
                foreach (KeyValuePair<string, object> pair in activo) {
                    
                    if (pair.Key.Equals("Activo") && pair.Value.Equals("No")) {
                        GameObject.Find("btnOpcion2").GetComponent<Button>().interactable = false;
                        GameObject.Find("txtAuth2").GetComponent<TextMeshProUGUI>().text = pair.Value.ToString();
                    }
                    else if (pair.Key.Equals("Activo") && pair.Value.Equals("Si")) {
                        GameObject.Find("btnOpcion2").GetComponent<Button>().interactable = true;
                        GameObject.Find("txtAuth2").GetComponent<TextMeshProUGUI>().text = pair.Value.ToString();
                    }

                    if (pair.Key.Equals("password")) {
                        authPath = pair.Value.ToString();
                        GameObject.Find("txtAuth").GetComponent<TextMeshProUGUI>().text = authPath;
                   //     Debug.Log("password para desbloquear: " + authPath);
                    }
                }
            }
            else {
                Debug.Log(String.Format("Document {0} does not exist!", snapshot.Id));
            }
        });
        return true;
    }

    public async Task<bool> LoadUserDataAsync(){
        DocumentReference studentRef = db.Collection("Usuarios").Document(user.idUsuario);
        CollectionReference schoolsReference = db.Collection("Usuarios");
        QuerySnapshot snapshot = await schoolsReference.GetSnapshotAsync();
        return true;
    }

    public bool OnLoadUsers() {        
        DocumentReference docRef = db.Collection("Usuarios3").Document(PlayerPrefs.GetString("email"));
        docRef.GetSnapshotAsync().ContinueWithOnMainThread(task => {
            DocumentSnapshot snapshot = task.Result;
            if (snapshot.Exists) {
                Dictionary<string, object> activo = snapshot.ToDictionary();
                
                foreach (KeyValuePair<string, object> pair in activo){
                    PlayerPrefs.SetString("idUsuario", snapshot.GetValue<string>("idUsuario"));
                    idUsuario = snapshot.GetValue<string>("idUsuario");
                    PlayerPrefs.SetString("nombre", snapshot.GetValue<string>("nombre"));
                    //Debug.Log("Email: " + PlayerPrefs.GetString("email") + "        " + idUsuario);
                }
            }            
        });
        return true;
    }

    
    //Automatizacion
    public bool SaveEnlaces(Enlace enl) {
        Dictionary<string, object> sessionDict = new Dictionary<string, object>{
            { "IdEnlace", enl.IdEnlace},  { "Enlace", enl.Descripcion}, { "Tipo", enl.Tipo}, { "Formula", enl.Formula}, { "Markers", enl.Markers}, { "Elem1", enl.Elem1},
            { "Elect1", enl.Elect1}, { "Elem2", enl.Elem2}, { "Elect2", enl.Elect2}, { "Elem3", enl.Elem3}, { "Elect3", enl.Elect1}
        };
        DocumentReference doc = db.Collection("Enlaces").Document(enl.Descripcion);
        doc.SetAsync(sessionDict);        
        Debug.Log("Guardando los enlaces en Firestore");
        return true;
    }

    public bool SaveCompuesto(Compuesto c) {
        Dictionary<string, object> sessionDict = new Dictionary<string, object>{
            { "IdCompuesto", c.IdCompuesto}, { "Descripcion", c.Descripcion}, { "Formula", c.Formula}, { "Elemento1", c.Elemento1}, { "AtomoElem1", c.AtomoElem1},
            { "Elemento2", c.Elemento2}, { "AtomoElem2", c.AtomoElem2}, { "Elemento3", c.Elemento3}, { "AtomoElem3", c.AtomoElem3}, { "Informacion", c.Informacion}
        };
        DocumentReference doc = db.Collection("IdenCompuestos").Document(c.Descripcion);
        doc.SetAsync(sessionDict);
        Debug.Log("Guardando los enlaces en Firestore");
        return true;
    }

    public bool SaveIdenEQ(Elemento e) {      
        Dictionary<string, object> sessionDict = new Dictionary<string, object>{
            { "idElemBD", e.idElemBD}, { "simbolo", e.simbolo}, { "descripcion", e.descripcion}, { "numElemento", e.numElemento}, { "pesoAtomico", e.pesoAtomico}, { "dificultad", e.dificultad},
            { "ayuda", e.ayuda}, { "Valencia", e.Valencia}, { "Grupo", e.Grupo}, { "Periodo", e.Periodo}, { "Categoria", e.Categoria}, { "Electronegatividad", e.Electronegatividad}
        };
        DocumentReference doc = db.Collection("IdenEQ").Document(e.descripcion);
        doc.SetAsync(sessionDict);
        Debug.Log("Guardando los enlaces en Firestore");
        return true;
    }

    public bool SavePreguntas(Pregunta p) {
        Dictionary<string, object> sessionDict = new Dictionary<string, object>{
            { "IdPregunta", p.IdPregunta}, { "Question", p.Question}, { "Respuesta", p.Respuesta}, { "Marcador", p.Marcador}, { "Dificultad", p.Dificultad}
        };
        DocumentReference doc = db.Collection("Preguntas").Document(p.Marcador);
        doc.SetAsync(sessionDict);
        Debug.Log("Guardando los enlaces en Firestore");
        return true;

    }

    public bool SaveAyudas(Ayuda a) {
        Dictionary<string, object> sessionDict = new Dictionary<string, object>{
            { "IdAyuda", a.IdAyuda}, { "IdEjercicio", a.IdEjercicio}, { "IdNivel", a.IdNivel}, { "Descripcion", a.Descripcion}, { "Tipo", a.Tipo}, { "Compuesto", a.Compuesto}
        };
        DocumentReference doc = db.Collection("Helps").Document(a.IdAyuda);
        doc.SetAsync(sessionDict);
        Debug.Log("Guardando los enlaces en Firestore");
        return true;
    }

    public bool SaveReaction(React r) {
        Dictionary<string, object> sessionDict = new Dictionary<string, object>{
            { "IdReaccion", r.IdReaccion}, { "Descripcion", r.Descripcion}, { "Elem1", r.Elem1}, { "Elem2", r.Elem2}, { "Elem3", r.Elem3}, { "Efecto", r.Efecto}
        };
        DocumentReference doc = db.Collection("Reacciones").Document(r.Descripcion);
        doc.SetAsync(sessionDict);
        Debug.Log("Guardando los enlaces en Firestore");
        return true;
    }

    public async Task<List<Enlace>> OnLoadBonds(string tipoEnlace){
        listC = new List<Enlace>();
        listC.Clear();
        CollectionReference citiesRef = db.Collection("Enlaces");
        Query query = citiesRef.WhereEqualTo("Tipo", tipoEnlace);
        QuerySnapshot snapshot =  await query.GetSnapshotAsync();
        
        foreach (DocumentSnapshot ds in snapshot.Documents) {
            //    Debug.Log(ds.GetValue<string>("Enlace"));
            listC.Add(new Enlace {
                IdEnlace = ds.GetValue<int>("IdEnlace"),  Descripcion = ds.GetValue<string>("Enlace"), Tipo = ds.GetValue<string>("Tipo"), Formula = ds.GetValue<string>("Formula"),
                Markers = ds.GetValue<string>("Markers"), Elem1 = ds.GetValue<string>("Elem1"), Elect1 = ds.GetValue<float>("Elect1"), Elem2 = ds.GetValue<string>("Elem2"),
                Elect2 = ds.GetValue<float>("Elect2"), Elem3 = ds.GetValue<string>("Elem3"), Elect3 = ds.GetValue<float>("Elect3")
            });            
        }     
        Debug.Log("Se han descargado " + listC.Count() + " elementos desde firestore");
        return listC;
    }

    public async Task<List<Ayuda>> OnLoadHelps(string tipoEnlace){
        listHC = new List<Ayuda>();
        listHC.Clear();
        CollectionReference citiesRef = db.Collection("Helps");
        Query query = citiesRef.WhereEqualTo("Tipo", tipoEnlace);
        QuerySnapshot snapshot = await query.GetSnapshotAsync();
        foreach (DocumentSnapshot ds in snapshot.Documents) {
        
            listHC.Add(new Ayuda {
            IdAyuda = ds.GetValue<string>("IdAyuda"),
            IdEjercicio = ds.GetValue<string>("IdEjercicio"),
            Descripcion = ds.GetValue<string>("Descripcion"),
            IdNivel = ds.GetValue<string>("IdNivel"),
            Tipo = ds.GetValue<string>("Tipo"),
            Compuesto = ds.GetValue<string>("Compuesto") 
            });
        }
        //Debug.Log("Se han descargado " + listHC.Count() + " elementos desde firestore del tipo: " + tipoEnlace);
        return listHC;
    }

    public async Task<List<React>> OnLoadReacciones(){
        listR = new List<React>();
        listR.Clear();
        CollectionReference citiesRef = db.Collection("Reacciones");
        Query query = citiesRef.WhereEqualTo("Elem3", "-");
        QuerySnapshot snapshot = await query.GetSnapshotAsync();
        foreach (DocumentSnapshot ds in snapshot.Documents){

            listR.Add(new React {
                IdReaccion = ds.GetValue<int>("IdReaccion"),             
                Descripcion = ds.GetValue<string>("Descripcion"),
                Efecto = ds.GetValue<string>("Efecto"),
                Elem1 = ds.GetValue<string>("Elem1"),
                Elem2 = ds.GetValue<string>("Elem2"),
                Elem3 = ds.GetValue<string>("Elem3")
            });
        }
        //Debug.Log("Se han descargado " + listHC.Count() + " elementos desde firestore del tipo: " + tipoEnlace);
        return listR;
    }

public async Task<List<Elemento>> OnLoadIDElementos(){
        listE = new List<Elemento>();
        listE.Clear();
        CollectionReference citiesRef = db.Collection("IdenEQ");       
        QuerySnapshot snapshot = await citiesRef.GetSnapshotAsync();
        foreach (DocumentSnapshot ds in snapshot.Documents) {

            listE.Add(new Elemento {
                idElemBD = ds.GetValue<int>("idElemBD"),
                simbolo = ds.GetValue<string>("simbolo"),
                descripcion = ds.GetValue<string>("descripcion"),
                numElemento = ds.GetValue<int>("numElemento"),
                pesoAtomico = ds.GetValue<int>("pesoAtomico"),
                dificultad = ds.GetValue<int>("dificultad"),
                ayuda = ds.GetValue<string>("ayuda"),
                Valencia = ds.GetValue<int>("Valencia"),
                Grupo = ds.GetValue<int>("Grupo"),
                Periodo = ds.GetValue<int>("Periodo"),
                Categoria = ds.GetValue<string>("Categoria"),
                Electronegatividad = ds.GetValue<string>("Electronegatividad")
            });
        }
        return listE;
    }
    public async Task<List<Compuesto>> OnLoadIDCompuestos(){
        listComp = new List<Compuesto>();
        listComp.Clear();
        CollectionReference citiesRef = db.Collection("IdenCompuestos");
        QuerySnapshot snapshot = await citiesRef.GetSnapshotAsync();
      
        // { "Elemento2", c.Elemento2}, { "AtomoElem2", c.AtomoElem2}, { "Elemento3", c.Elemento3}, { "AtomoElem3", c.AtomoElem3}, { "Informacion", c.Informacion}

        foreach (DocumentSnapshot ds in snapshot.Documents) {
            listComp.Add(new Compuesto {
                IdCompuesto = ds.GetValue<int>("IdCompuesto"),
                Descripcion = ds.GetValue<string>("Descripcion"),
                Formula = ds.GetValue<string>("Formula"),
                Elemento1 = ds.GetValue<string>("Elemento1"),
                AtomoElem1 = ds.GetValue<int>("AtomoElem1"),
                Elemento2 = ds.GetValue<string>("Elemento2"),
                AtomoElem2 = ds.GetValue<int>("AtomoElem2"),
                Elemento3 = ds.GetValue<string>("Elemento3"),
                AtomoElem3 = ds.GetValue<int>("AtomoElem3"),
                Informacion = ds.GetValue<string>("Informacion")
            });
        }
        return listComp;
    }

    public async Task<List<Pregunta>> OnLoadPreguntas()
    {
        listP = new List<Pregunta>();
        listP.Clear();
        CollectionReference citiesRef = db.Collection("Preguntas");
        QuerySnapshot snapshot = await citiesRef.GetSnapshotAsync();
        foreach (DocumentSnapshot ds in snapshot.Documents) {
            //"", p.IdPregunta}, { "", p.Question}, { "", p.Respuesta}, { "", p.Marcador}, { "", p.Dificultad}
            listP.Add(new Pregunta {
                IdPregunta = ds.GetValue<int>("IdPregunta"),
                Question = ds.GetValue<string>("Question"),
                Respuesta = ds.GetValue<string>("Respuesta"),
                Marcador = ds.GetValue<string>("Marcador"),
                Dificultad = ds.GetValue<int>("Dificultad")
            });
        }
        //Debug.Log("Se han descargado " + listHC.Count() + " elementos desde firestore del tipo: " + tipoEnlace);
        return listP;
    }

    public Enlace GetInformacion(string descripcion) {
        var temp = listC.Where(x => x.Descripcion == descripcion);       
        return temp.FirstOrDefault();
    }

    public Enlace GetInformacionIonic(string descripcion){
        var temp = listC.Where(x => x.Descripcion == descripcion);
        return temp.FirstOrDefault();
    }

    public Pregunta GetInformacionChonps(string descripcion){
        var temp = listP.Where(x => x.Question == descripcion);
        return temp.FirstOrDefault();
    }

    public Enlace GetInformacionMetal(string descripcion){
        var temp = listC.Where(x => x.Descripcion == descripcion);
        return temp.FirstOrDefault();
    }

    public Enlace GetInformacionComp(string descripcion){
        var temp = listC.Where(x => x.Descripcion == descripcion);
        return temp.FirstOrDefault();
    }

    public Elemento GetIDElemEQ(string descripcion){
        var temp = listE.Where(x => x.descripcion == descripcion);
        return temp.FirstOrDefault();
    }
    public Compuesto GetIDComp(string descripcion)
    {
        var temp = listComp.Where(x => x.Descripcion == descripcion);
        return temp.FirstOrDefault();
    }
}