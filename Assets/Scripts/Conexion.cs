using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Conexion : MonoBehaviour
{
    InputField AddUser, AddName, addPass, AddEscuela, AddGrupo, AddEmail;
    private string pass = string.Empty;
    private int derecho;

    private void Start(){
        AddUser = GameObject.Find("fbUser").GetComponent<InputField>();
        AddName = GameObject.Find("fbNombre").GetComponent<InputField>();
        AddEmail = GameObject.Find("fbEmail").GetComponent<InputField>();
        addPass = GameObject.Find("fbPass").GetComponent<InputField>();
        AddGrupo = GameObject.Find("InputGrupo").GetComponent<InputField>();
        AddEscuela = GameObject.Find("InputEscuela").GetComponent<InputField>();
        derecho = 1;
    }

    public IEnumerator Mostrar(string mensaje){
        GameObject.Find("pnlMensaje").GetComponent<RectTransform>().localScale = new Vector3(0.7f, 0.5f, 0.5f);
        GameObject.Find("txtMensaje").GetComponent<TextMeshProUGUI>().text = mensaje;
        yield return new WaitForSeconds(3);
        GameObject.Find("pnlMensaje").GetComponent<RectTransform>().localScale = Vector3.zero;
    }

    public void Salir(){
        Application.Quit();
    }
    public void OcultarLogIn(){
        GameObject.Find("LoginPanel").GetComponent<RectTransform>().localScale = Vector3.zero;
        ClearInputs();
    }

    public void MostrarLogIn(){
        GameObject.Find("PanelRegistro").GetComponent<RectTransform>().localScale = Vector3.zero;
        GameObject.Find("LoginPanel").GetComponent<RectTransform>().localScale = new Vector3(0.6f, 0.5f, 0.5f);
      //  GameObject.Find("txtUsuario").GetComponent<InputField>().text = GameObject.Find("fbUser").GetComponent<InputField>().text;
      //  GameObject.Find("txtPass").GetComponent<InputField>().text = GameObject.Find("fbPass").GetComponent<InputField>().text;
    }

    public void MostrarRegistro(){
        GameObject.Find("LoginPanel").GetComponent<RectTransform>().localScale = Vector3.zero;
        GameObject.Find("PanelRegistro").GetComponent<RectTransform>().localScale = new Vector3(0.6f, 0.5f, 0.5f);
        ClearInputs();
    }

    public void OcultarRegistro(){
        GameObject.Find("PanelRegistro").GetComponent<RectTransform>().localScale = Vector3.zero;
    }

    public void ClearInputs(){
        this.addPass.text = "";
        this.AddEmail.text = "";
        this.AddUser.text = "";
        this.AddEscuela.text = "";
        this.AddGrupo.text = "";
        this.AddName.text = "";
    }
}

/*
private string connection;
private IDbConnection dbCon;
private IDbCommand dbCmd;
private IDataReader reader;

InputField input1, AddUser, AddName, addPass, AddEscuela, AddGrupo, AddStatus, AddEmail;
private void Start(){
    AddUser = GameObject.Find("fbUser").GetComponent<InputField>();
    AddName = GameObject.Find("fbNombre").GetComponent<InputField>();
    AddEmail = GameObject.Find("fbEmail").GetComponent<InputField>();
    addPass = GameObject.Find("fbPass").GetComponent<InputField>();
    AddGrupo = GameObject.Find("InputGrupo").GetComponent<InputField>();
    AddEscuela = GameObject.Find("InputEscuela").GetComponent<InputField>();
    derecho = 1;
    AbrirConexion();
    GetIdSesion();
}

public void AbrirConexion(){
    string p = "BDQuimica.db";
    string filePath;
    if (Application.isMobilePlatform){
        filePath = Application.persistentDataPath + "/" + p;
    }
    else{
        filePath = Application.streamingAssetsPath + "/" + p;
    }
    if (!File.Exists(filePath)){
        CrearBaseDatos(filePath);
    }
    connection = "URI=file:" + filePath;
    Debug.Log("Conectando a la base de datos  " + connection);
    dbCon = new SqliteConnection(connection);
    dbCon.Open();
}

void CrearBaseDatos(string filePath)
{
    WWW loadDb = new WWW("jar:file//" + Application.dataPath + "!/assets/" + "BDQuimica.db");
    while (!loadDb.isDone) { }
    File.WriteAllBytes(filePath, loadDb.bytes);
    connection = "URI=file:" + filePath;
    dbCon = new SqliteConnection(connection);
    dbCon.Open();
    dbCmd = dbCon.CreateCommand();
    Debug.Log("Creando base de datos..");
    List<string> cmdText = new List<string>(){
        "CREATE TABLE IF NOT EXISTS 'SESION' ('IdUsuario' TEXT NOT NULL, 'Nombre' TEXT NOT NULL, 'LASTSESION' TEXT )",
        "CREATE TABLE IF NOT EXISTS 'Preguntas' ('IdPregunta' INTEGER NOT NULL, 'Pregunta'  TEXT NOT NULL, 'Respuesta' TEXT NOT NULL, 'Marcador'  TEXT NOT NULL, 'Dificultad' INTEGER, PRIMARY KEY('IdPregunta'))",
        "CREATE TABLE IF NOT EXISTS 'Usuarios' ('idUsuario'	TEXT NOT NULL UNIQUE, 'nombre' TEXT NOT NULL, 'pass' TEXT NOT NULL, 'status' INTEGER NOT NULL, 'escuela' TEXT, 'grupo'	TEXT, email text not null, PRIMARY KEY('idUsuario'))",
        "CREATE TABLE IF NOT EXISTS 'Enlaces' ('IdEnlace' INT NOT NULL, 'Descripcion' TEXT NOT NULL, 'Tipo'  TEXT, 'Formula' TEXT, 'Markers' TEXT, 'Elem1' TEXT NOT NULL, 'Elect1' REAL, 'Elem2' TEXT, 'Elect2' REAL, 'Elem3' TEXT, 'Elect3' REAL)",
        "CREATE TABLE IF NOT EXISTS 'Helps' ('idAyuda' INTEGER NOT NULL, 'idEjercicio' INTEGER NOT NULL, 'Nivel'	INTEGER NOT NULL, 'Descripcion'	TEXT NOT NULL, 'Tipo' TEXT, 'Compuesto' TEXT, PRIMARY KEY('idAyuda'))",
        "CREATE TABLE IF NOT EXISTS 'EnlacesIonicos' ('id' INTEGER NOT NULL UNIQUE, 'Compuesto' TEXT NOT NULL UNIQUE, 'Dificultad' INTEGER NOT NULL, 'Formula' TEXT NOT NULL, 'divFormula' TEXT NOT NULL, 'Elem1' TEXT, 'ElecElem1'	INTEGER, 'Elem2'	TEXT, 'ElecElem2' INTEGER, 'Elem3' TEXT, 'ElecElem3' INTEGER, PRIMARY KEY('id'));",
        "CREATE TABLE IF NOT EXISTS 'IdenEQ' ('numElemento' INTEGER NOT NULL UNIQUE, 'simbolo' TEXT NOT NULL, 'Nombre' TEXT NOT NULL UNIQUE, 'PesoA' REAL NOT NULL, 'nAtomos' INT, 'Dificultad' INT, 'Ayuda' TEXT, 'nElecVal' INT, 'nPeriodo' INT, 'nGrupo' INT, 'Categoria' TEXT, 'Electronegatividad' REAL, PRIMARY KEY('numElemento'))",
        "CREATE TABLE IF NOT EXISTS 'Lewis' ('Marcador' TEXT NOT NULL UNIQUE, 'Descripcion' TEXT NOT NULL, 'Electrones' INTEGER NOT NULL, 'Prefab' TEXT NOT NULL, 'Grupo' INTEGER NOT NULL, 'Compuesto' TEXT, 'Dificultad' INTEGER, 'Formula' TEXT, 'DivFormula' TEXT, PRIMARY KEY('Marcador'))",
        "CREATE TABLE IF NOT EXISTS 'IdenCompuestos' ('idCompuesto' INTEGER NOT NULL UNIQUE, 'Descripcion' TEXT NOT NULL, 'Formula' TEXT NOT NULL, 'Elemento1' TEXT NOT NULL, 'AtomElem1' INTEGER NOT NULL, 'Elemento2' TEXT NOT NULL, 'AtomElem2' INTEGER NOT NULL, 'Elemento3' TEXT, 'AtomElem3' INTEGER,'Informacion' TEXT NOT NULL)",
        "CREATE TABLE IF NOT EXISTS 'Reacciones' ('IdReaccion' INTEGER NOT NULL, 'Descripcion' TEXT NOT NULL, 'Elem1' TEXT not null, 'Elem2' TEXT NOT NULL, 'Elem3' TEXT, 'Efecto' TEXT NOT NULL)",
        "CREATE TABLE IF NOT EXISTS 'Ejercicios' ('Folio' INTEGER NOT NULL, 'idSesion' INTEGER, 'idEjercicio' TEXT NOT NULL, 'idUsuario' TEXT NOT NULL, 'aciertos' INTEGER,'errores' INTEGER, 'ayudas' INTEGER, 'tiempo' INTEGER, 'puntos' INTEGER, 'nivel' REAL, 'dificultad' TEXT, 'Fecha' TEXT)",
        "INSERT INTO 'SESION' ('IdUsuario', 'Nombre', 'LASTSESION') VALUES ('admin', 'Administrador', '" + System.DateTime.Now.Date.ToShortDateString() + "')",
        "INSERT INTO 'Enlaces' VALUES (1,'Acido Clorhidrico','Covalente','H Cl','Hidrogeno-Cloro','Hidrogeno',0.8,'Cloro',3,'-',0) ",
        "INSERT INTO 'Enlaces' VALUES (2, 'Agua', 'Covalente', 'H2 O', 'Hidrogeno-Oxigeno', 'Hidrogeno', 0.8, 'Oxigeno', 3.5, '-', 0)",
        "INSERT INTO 'Enlaces' VALUES (3, 'Dioxido de Carbono', 'Covalente', 'C O2', 'Carbono-Oxigeno', 'Carbono', 2.5, 'Oxigeno', 3.5, '-', 0)",
        "INSERT INTO 'Enlaces' VALUES (4, 'Amoniaco', 'Covalente', 'N H3', 'Nitrogeno-Oxigeno', 'Nitrogeno', 3, 'Hidrogeno', 0.8, '-', 0)",
        "INSERT INTO 'Enlaces' VALUES (5, 'Metano', 'Covalente', 'C H4', 'Carbono-Hidrogeno', 'Carbono', 2.5, 'Hidrogeno', 0.8, '-', 0)",
        "INSERT INTO 'Enlaces' VALUES (6, 'Oxido de Magnesio', 'Ionico', 'Mg O', 'Magnesio-Oxigeno', 'Magnesio', 1.2, 'Oxigeno', 3.5, '-', 0)",
        "INSERT INTO 'Enlaces' VALUES (7, 'Cloruro de Sodio', 'Ionico', 'Na Cl', 'Sodio-Cloro', 'Sodio', 0.9, 'Cloro', 3, '-', 0)",
        "INSERT INTO 'Enlaces' VALUES (8, 'Cloruro de Potasio', 'Ionico', 'K Cl', 'Potasio-Cloro', 'Potasio', 0.8, 'Cloro', 3, '-', 0)",
        "INSERT INTO 'Enlaces' VALUES (9, 'Fluoruro de Litio', 'Ionico', 'Li F', 'Fluor-Litio', 'Litio', 1, 'Fluor', 4, '-', 0)",
        "INSERT INTO 'Enlaces' VALUES (10, 'Oxido de Zinc', 'Ionico', 'Zn O', 'Zinc-Oxigeno', 'Zinc', 1.7, 'Oxigeno', 3.5, '-', 0)" ,
        "INSERT INTO 'Enlaces' VALUES (11, 'Acero', 'Metálico', 'C Fe', 'Carbono-Hierro', 'Carbono', 0, 'Hierro', 0, '-', 0)" ,
        "INSERT INTO 'Enlaces' VALUES (12, 'Laton', 'Metálico', 'Cu Zn', 'Cobre-Zinc', 'Cobre', 0, 'Zinc', 0, '-', 0)" ,
        "INSERT INTO 'Enlaces' VALUES (13, 'Oxido de Calcio', 'Metálico', 'Ca O', 'Calcio-Oxigeno', 'Calcio',0, 'Oxigeno', 0, '-', 0)" ,
        "INSERT INTO 'IdenEQ' VALUES (1, 'H', 'Hidrogeno', 1, 1, 1, 'El Hidrógeno es un gas incoloro, inodoro y muy reactivo que se halla en todos los componentes de la materia viva y en muchos minerales, siendo el elemento más abundante en el universo.', 1, 1, 1, 'No Metales', 0.8)",
        "INSERT INTO 'IdenEQ' VALUES (2, 'O', 'Oxigeno', 8, 16, 1, 'El Oxígeno es un elemento químico gaseoso, símbolo O, número atómico 8 y peso atómico 16. Es de gran interés por ser el elemento esencial en los procesos de respiración de la mayor parte de las células vivas y en los procesos de combustión. Es el elemento más abundante en la corteza terrestre.', 6, 2, 16, 'No Metales', 3.5) ",
        "INSERT INTO 'IdenEQ' VALUES (3, 'Na', 'Sodio', 11, 23, 1, 'El sodio (Na) es un metal alcalino blando, untuoso, de color plateado, muy abundante en la naturaleza, encontrándose en la sal marina y el mineral halita. Es muy reactivo, arde con llama amarilla, se oxida en presencia de oxígeno y reacciona violentamente con el agua.', 1, 3, 1, 'Alcalinos', 0.9) ",
        "INSERT INTO 'IdenEQ' VALUES (4, 'C', 'Carbono', 6, 12, 1, 'El Carbono (C) con número atómico 6 y masa atómica 12.01 es un no metal sólido que es el componente fundamental de los compuestos orgánicos y tiene la propiedad de enlazarse con otros átomos de carbono y otras sustancias para formar un número casi infinito de compuestos; en la naturaleza se presenta en tres formas: diamante, grafito y carbono amorfo o carbón.', 4, 2, 14, 'No Metales', 2.5) ",
        "INSERT INTO 'IdenEQ' VALUES (5, 'Cl', 'Cloro', 17, 35, 1, 'El Cloro(Cl), que en la tabla periódica de los elementos forma parte del conjunto de los halógenos, se presenta normalmente como un gas de tonalidad amarillenta o verdosa que se caracteriza por su toxicidad. Se trata de un elemento presente en cantidades copiosas en nuestro planeta.', 7, 2, 17, 'No Metales', 3.0) ",
        "INSERT INTO 'IdenEQ' VALUES (6, 'K', 'Potasio', 19, 39, 1, 'El Potasio (K) es un metal alcalino plateado, blando y ligero, que se oxida fácilmente y produce llama en contacto con el agua; se encuentra en grandes cantidades en la naturaleza en algunos minerales y en el tejido vegetal y animal, y es uno de los componentes fundamentales de los suelos fértiles; se usa en las células fotoeléctricas, y sus compuestos tienen numerosísimas aplicaciones.', 1, 4, 1, 'Alcalinos', 0.8)",
        "INSERT INTO 'IdenEQ' VALUES (7, 'Mg', 'Magnesio', 12, 24, 2, 'El Magnesio (Mg) con número atómico 12, masa atómica 24.31 es un metal alcalino de color blanco plateado, maleable y ligero, que existe en la naturaleza solamente en combinación química con otros elementos y es un componente esencial del tejido animal y vegetal; se usa en flashes fotográficos, señales luminosas, etc.', 2, 3, 2, 'Alcalinotérreos', 1.2)",
        "INSERT INTO 'IdenEQ' VALUES (8, 'N', 'Nitrogeno', 7, 14, 2, 'El Nitrógeno (N) con número atómico 7 y masa atómica 14; es un gas incoloro, inodoro e inerte, compone cuatro quintos del volumen del aire de la atmósfera y se usa para sintetizar amoníaco y otros productos, para fabricar explosivos, como refrigerante y como atmósfera inerte para conservar ciertos productos.', 5, 2, 15, 'No Metales', 3.0) ",
        "INSERT INTO 'IdenEQ' VALUES (9, 'F', 'Fluor', 9, 19, 2, 'El flúor (F) es un gas a temperatura ambiente, de color amarillo pálido, formado por moléculas diatómicas. Es el más electronegativo y reactivo de todos los elementos. En forma pura es altamente peligroso, causando graves quemaduras químicas al contacto con la piel.', 7, 2, 17, 'No Metales', 4.0) ",
        "INSERT INTO 'IdenEQ' VALUES (10, 'Li', 'Litio', 3, 7, 2, 'El Litio (Li) de número atómico 3 y masa atómica 6.94 es un metal alcalino blanco plateado, blando, dúctil y muy ligero, se corroe rápidamente al contacto con el aire y no existe en estado libre en la naturaleza, sino solamente en compuestos; se utiliza en la fabricación de acero, en esmaltes y lubricantes, y el carbonato de litio, en medicina.', 1, 2, 1, 'Alcalinos', 1.0)",
        "INSERT INTO 'IdenEQ' VALUES (11, 'Ca', 'Calcio', 20, 40, 3, 'El Calcio (Ca) de número atómico 20 y masa atómica es 40.078 es un metal blando, grisáceo, y es el quinto más abundante en masa de la corteza terrestre. También es el ion más abundante disuelto en el agua de mar, tanto como por su molaridad y como por su masa, después del sodio, cloruros, magnesio y sulfatos.', 2, 4, 2, 'Alcalinotérreos', 1.0) ",
        "INSERT INTO 'IdenEQ' VALUES (12, 'Zn', 'Zinc', 30, 65, 3, 'El Zinc (Zn) de número atómico 30 es un elemento que presenta cierto parecido con el magnesio, y con el cadmio de su grupo, pero del mercurio se aparta mucho por las singulares propiedades físicas y químicas de este (contracción lantánida y potentes efectos relativistas sobre orbitales de enlace). Es uno de los 23 elementos más abundante en la Tierra y una de sus aplicaciones más importantes es el galvanizado del acero.', 2, 4, 12, 'Metal de Traslación', 1.7)",
        "INSERT INTO 'IdenEQ' VALUES (13, 'Ag', 'Plata', 47, 107, 3, 'La Plata (Ag) con número atómico 47 es un metal de transición blanco, brillante, blando, dúctil, maleable. Se encuentra en la naturaleza formando parte de distintos minerales. Es muy común en la naturaleza, de la que representa una parte en 5 mil de corteza terrestre. La mayor parte de su producción se obtiene como subproducto del tratamiento de las minas de cobre, zinc, plomo y oro.', 1, 5, 11, 'Metal de Traslación', 1.9)",
        "INSERT INTO 'IdenEQ' VALUES (14, 'P', 'Fosforo', 15, 31, 3, 'El Fosforo (P) forma la base de gran número de compuestos, de los cuales los más importantes son los fosfatos. Los fosfatos desempeñan un papel esencial en los procesos de transferencia de energía, como el metabolismo, la fotosíntesis, la función nerviosa y la acción muscular. Los ácidos nucleicos, que entre otras cosas forman los cromosomas, son fosfatos, así como cierto número de coenzimas. Los esqueletos de los animales están formados por fosfato de calcio.', 5, 3, 15, 'No Metales', 2.1)",
        "INSERT INTO 'IdenEQ' VALUES (15, 'Cr', 'Cromo', 24, 52, 3, 'El Cromo (Cr) de número atómico 24 y masa atómica 51.996 es un metal del grupo de los elementos de transición, de color gris, muy duro, resistente e inoxidable, que se emplea en el cromado de objetos para hacerlos inoxidables, en refractantes, en la creación de aleaciones de hierro, níquel y cobalto, en la fabricación de herramientas de corte y en el acabado de vehículos.', 3, 4, 6, 'Metal de Traslación', 1.6) ",
        "INSERT INTO 'IdenEQ' VALUES (16, 'Pb', 'Plomo', 82, 207, 4, 'El Plomo (Pb) con un número atómico es 82. La elasticidad de este elemento depende de la temperatura ambiente, la cual extiende sus átomos. El plomo es un metal pesado de densidad relativa o gravedad específica 11,4 a 16 °C, de color plateado con tono azulado, que se empaña para adquirir un color gris mate. Es flexible, inelástico y se funde con facilidad.', 4, 6, 14, 'Metales', 1.8) ",
        "INSERT INTO 'IdenEQ' VALUES (17, 'Hg', 'Mercurio', 80, 200, 4, 'El Mercurio (Hg) es el único elemento metálico de aspecto plateado que es líquido en condiciones estándar de laboratorio; No es buen conductor del calor comparado con otros metales, aunque es buen conductor de la electricidad. Se alea fácilmente con muchos otros metales como el oro o la plata produciendo amalgamas, pero no con el hierro. Es insoluble en agua y soluble en ácido nítrico. ', 2, 6, 12, 'Metal de Traslación', 1.9) ",
        "INSERT INTO 'IdenEQ' VALUES (18, 'Al', 'Aluminio', 13, 27, 4, 'El Aluminio (Al) es un metal no ferromagnético. Es el tercer elemento más común encontrado en la corteza terrestre y se encuentran presentes en la mayoría de las rocas, de la vegetación y de los animales.​ Este metal posee una combinación de propiedades que lo hacen muy útil en ingeniería de materiales, tales como su baja densidad (2812,5 kg/m³) y su alta resistencia a la corrosión.', 3, 3, 13, 'Metales', 1.5) ",
        "INSERT INTO 'IdenEQ' VALUES (19, 'Fe', 'Hierro', 26, 56, 4, 'Es el cuarto elemento más abundante en la corteza terrestre, representando un 5 % y, entre los metales, solo el aluminio es más abundante; y es el primero más abundante en masa planetaria, debido a que el planeta en su núcleo, se concentra la mayor masa de hierro nativo equivalente a un 70 %. El núcleo de la Tierra está formado principalmente por hierro y níquel en forma metálica, generando al moverse un campo magnético. ', 3, 4, 8, 'Metal de Traslación', 1.8)",
        "INSERT INTO 'IdenEQ' VALUES (20, 'S', 'Azufre', 16, 32, 4, 'El Azufre (S) es un no metal de color amarillo pálido y olor desagradable, que se encuentra en la naturaleza tanto en forma libre como combinado con otros elementos; se usa para la obtención de ácido sulfúrico, para fabricar fósforos, caucho vulcanizado, tintes, pólvora, fungicidas, en fotografía para el fijado de negativos y positivos, y, en medicina para la elaboración de pomadas tópicas.', 6, 3, 16, 'No Metales', 2.5) ",
        "INSERT INTO 'IdenEQ' VALUES (21, 'He', 'Helio', 4, 2, 1, 'El helio presenta las propiedades de un gas noble. Es decir, es inerte (no reacciona) y al igual que estos, es un gas monoatómico incoloro e inodoro que cuenta con el menor punto de ebullición de todos los elementos químicos y solo puede ser licuado bajo presiones muy grandes y no puede ser congelado.', 8, 1, 18, 'Gases Nobles', 0.0)",
        "INSERT INTO 'IdenEQ' VALUES (22, 'As', 'Arsenico', 75, 33, 3, 'Es un elemento esencial para la vida y su deficiencia puede dar lugar a diversas complicaciones, sin embargo, no se conoce con precisión, la función biológica. La ingesta diaria de 12 a 15 μg puede consumirse sin problemas en la dieta diaria de carnes, pescados, vegetales y cereales, siendo los peces y crustáceos los que más contenido de arsénico presentan.', 5, 4, 15, 'Metaloides', 2.0) ",
        "INSERT INTO 'IdenEQ' VALUES (23, 'Xe', 'Xenon', 131, 54, 3, 'Gas noble inodoro, muy pesado, incoloro, el xenón está presente en la atmósfera terrestre solo en trazas y fue parte del primer compuesto de gas noble sintetizado.', 8, 5, 18, 'Gases Nobles', 2.6)",
        "INSERT INTO 'IdenCompuestos' VALUES (1, 'Hidroxido de Hidrogeno','H2O','Hidrogeno', 2, 'Oxigeno', 1, '-', 0, 'El hidróxido de Hidrógeno, mejor conocido como Agua, tiene la característica de ser incolora, inodora y de sabor agradable, está compuesta de dos átomos de hidrógeno y uno de oxígeno. El agua puede absorber grandes cantidades de calor que es utilizado para romper los puentes de hidrógeno, por lo que la temperatura se eleva muy lentamente. El agua tiene tres estados: Solido (Hielo), Líquido y Gas (vapor).')",
        "INSERT INTO 'IdenCompuestos' VALUES (2, 'Cloruro de Sodio', 'NaCl', 'Sodio', 1, 'Cloro', 1, '-', 0, 'El Cloruro de Sodio es una de las sales responsable de la salinidad del océano y del fluido extracelular de muchos organismos. También es el mayor componente de la sal comestible, comúnmente usada como condimento y conservante de comida. Está compuesto de cloro y sodio. Es producido en masa por la evaporación de agua de mar o salmuera de otros recursos, como lagos salados, y minando la roca de sal, llamada halita.')",
        "INSERT INTO 'IdenCompuestos' VALUES (3, 'Oxido Ferroso', 'FeO', 'Hierro', 1, 'Oxigeno', 1, '-', 0, 'Está compuesto por Hierro y Oxígeno. Es una sustancia que puede causar explosiones ya que literalmente entra en combustión. En su estado natural es conocido como hematita. También es purificado para su uso como soporte de almacenamiento magnético en audio e informática. ')" ,
        "INSERT INTO 'IdenCompuestos' VALUES (4, 'Acido Sulfurico', 'H2SO4', 'Azufre', 1, 'Hidrogeno', 2, 'Oxigeno', 4, 'El ácido sulfúrico (H2SO4) o sulfato de hidrógeno, es un líquido incoloro, viscoso y un ácido inorgánico fuerte. Es un ácido mineral y es uno de los 20 productos químicos más importantes en la industria química. El concentrado de H2SO4 es altamente corrosivo y puede dañar severamente los tejidos al contacto. Al ser un ácido fuerte, oxidante, agente corrosivo y deshidratante, es más peligroso que los otros ácidos minerales. Causa quemaduras químicas severas al contacto con la piel. El contacto con los ojos puede causar daño permanente y ceguera.')",
        "INSERT INTO 'IdenCompuestos' VALUES (5, 'Dioxido de Carbono', 'CO2', 'Carbono', 1, 'Oxigeno', 2, '-', 0, 'Es un gas incoloro y vital para la vida en la Tierra. Este compuesto químico se encuentra en la naturaleza y está compuesto de un átomo de carbono unido con enlaces covalentes dobles a dos átomos de oxígeno.  Dado que el CO2 es soluble en agua, ocurre naturalmente en aguas subterráneas, ríos, lagos, campos de hielo, glaciares y mares. Está presente en yacimientos de petróleo y gas natural. Se utiliza como agente extintor eliminando el oxígeno encontrado en ese espacio, e impidiendo que se genere una combustión. En la industria alimentaria, se utiliza en bebidas carbonatadas para darles efervescencia.')",
        "INSERT INTO 'IdenCompuestos' VALUES (6, 'Acido Clorhidrico', 'HCl', 'Hidrogeno', 1, 'Cloro', 1, '-', 0, 'El ácido clorhídrico (HCl) es un ácido corrosivo que se forma disolviendo cloruro de hidrógeno en el agua. También se conoce como ácido muriático. Se debe evitar el contacto con la piel, ojos y mucosas, debido a que es un ácido muy corrosivo. El ácido clorhídrico está presente en los jugos gástricos del estómago en el cuerpo humano, ayudando a realizar la digestión de los alimentos. A temperatura ambiente  es un gas ligeramente amarillo, corrosivo, no inflamable, más pesado que el aire, de olor fuertemente irritante.')",
        "INSERT INTO 'EnlacesIonicos' VALUES (1,'Cloruro de Sodio',1,'NaCl','Na-Cl','Na',1,'Cl',7,'-',0)",
        "INSERT INTO 'EnlacesIonicos' VALUES (2,'Cloruro de Calcio',1,'CaCl','Ca-Cl','Ca',1,'Cl',7,'-',0)",
        "INSERT INTO 'EnlacesIonicos' VALUES (4,'Floururo de Litio',1,'FLi','F-Li','F',1,'Li',7,'-',0)",
        "INSERT INTO 'EnlacesIonicos' VALUES (5,'Oxido de Boro',2,'BO2','B-O*2','B',1,'O',2,'-',0)",
        "INSERT INTO 'EnlacesIonicos' VALUES (6,'Hidroxido de Hidrogeno',2,'H2O','H*2-O','H',2,'O',1,'-',0)",
        "INSERT INTO 'EnlacesIonicos' VALUES (7,'Bioxido de Carbono',2,'CO2','C-O*2','C',4,'O',2,'-',0)",
        "INSERT INTO 'EnlacesIonicos' VALUES (8,'Amoniaco',3,'NH3','N-H*3','N',5,'H',3,'-',0)",
        "INSERT INTO 'EnlacesIonicos' VALUES (9,'Acido Sulfurico',3,'H2SO4','H*2-S-O*4','H',2,'S',1,'O',4)",
        "INSERT INTO 'Lewis' VALUES ('Sodio','Sodio',1,'Na',1,'Cloruro de Sodio',1,'NaCl','Na-Cl')",
        "INSERT INTO 'Lewis' VALUES ('Calcio','Calcio',2,'Ca',2,'Cloruro de Calcio',1,'CaCl','Ca-Cl')",
        "INSERT INTO 'Lewis' VALUES ('Boro','Boro',3,'B',3,'Oxido de Boro',2,'BO2','B-0*2')",
        "INSERT INTO 'Lewis' VALUES ('Carbono','Carbono',4,'C',4,'Bioxido de Carbono',2,'CO2','C-0*2')",
        "INSERT INTO 'Lewis' VALUES ('Nitrogeno','Nitrogeno',5,'N',5,'Amoniaco',3,'NH3','N-H*3')",
        "INSERT INTO 'Lewis' VALUES ('Oxigeno','Oxigeno',6,'O',6,'Bioxido de Carbono',2,'CO2','C-0*2')",
        "INSERT INTO 'Lewis' VALUES ('Flour','Flour',7,'F',7,'Floururo de Litio',1,'FLi','F-Li')",
        "INSERT INTO 'Lewis' VALUES ('Litio','Litio',1,'Li',1,'Floururo de Litio',1,'FLi','F-Li')",
        "INSERT INTO 'Lewis' VALUES ('Cloro','Cloro',7,'Cl',7,'Cloruro de Calcio',1,'CaCl','Ca-Cl')",
        "INSERT INTO 'Lewis' VALUES ('Azufre','Azufre',6,'S',6,'Acido Sulfurico',3,'H2SO4','H*2-S-O*4')",
        "INSERT INTO 'Lewis' VALUES ('Hidrogeno','Hidrogeno',1,'H',1,'Acido Sulfurico',3,'H2SO4','H*2-S-O*4')",
        "INSERT INTO 'Preguntas' VALUES (1,'Su número atómico es el 1; se encuentra presente en la atmósfera, pero en menor cantidad. Es esencial en los hidrocarburos y los ácidos.','Hidrogeno','Hidrogeno',1)",
        "INSERT INTO 'Preguntas' VALUES(2, 'Su número atómico es el 8. Es un elemento en la formación del agua, causante de la combustión y produce la energía del cuerpo.', 'Oxigeno', 'Oxigeno',1)",
        "INSERT INTO 'Preguntas' VALUES(3, 'Su número atómico es el 6. Es de gran importancia para la regulación del clima de la Tierra. El diamante y el grafito son formas de representarse.', 'Carbono', 'Carbono',1)",
        "INSERT INTO 'Preguntas' VALUES(4, 'Su número atómico es el 7. Es un gas incoloro, inodoro e inerte que destaca su presencia en proteínas, lípidos y ácidos nucleicos. Se usa para sintetizar el amoniaco, para fabricar explosivos o como refrigerante.', 'Nitrogeno', 'Nitrogeno',1)",
        "INSERT INTO 'Preguntas' VALUES(5, 'Su número atómico es el 15. Participa activamente en las relaciones energéticas que ocurren al interior de los organismos. Forman parte de los fosfolípidos.', 'Fosforo', 'Fosforo',1)",
        "INSERT INTO 'Preguntas' VALUES(6, 'Su número atómico es el 16; es un no metal de color amarillo pálido y olor desagradable que se encuentran en la naturaleza tanto en forma libre como combinado con otros elementos. Se usa para obtener ácido sulfúrico.', 'Azufre', 'Azufre',1)",
        "INSERT INTO 'HELPS' VALUES (1, 1, 1, 'Los enlaces covalentes están conformados a partir de un elemento metálico unido a uno no metálico', 'Covalente', 'Acido Clorhidrico')",
        "INSERT INTO 'HELPS' VALUES (2, 1, 2, 'El ácido clorhídrico contiene Cloro, además de ...','Covalente','Acido Clorhidrico')",
        "INSERT INTO 'HELPS' VALUES (3, 1, 3, 'El ácido clorhídrico contiene el elemento hidrógeno, además de ...','Covalente','Acido Clorhidrico')",
        "INSERT INTO 'HELPS' VALUES (4, 2, 1, 'Su nombre es Hidróxido de Hidrógeno, contiene Hidrógeno y ...','Covalente','Agua')",
        "INSERT INTO 'HELPS' VALUES (5, 2, 2, 'Se caracteriza por ser inodora, incolora, y de sabor agradable, contiene Oxígeno e ...','Covalente','Agua')",
        "INSERT INTO 'HELPS' VALUES (6, 2, 3, 'El agua esta compuesta por dos moleculas de Oxígeno y una molécula de ...','Covalente','Agua')",
        "INSERT INTO 'HELPS' VALUES (7, 3, 1, 'El Dióxido de carbono contiene dos moleculas de Oxígeno y una de ...','Covalente','Dioxido de Carbono')",
        "INSERT INTO 'HELPS' VALUES (8, 3, 2, 'El Dióxido de carbono contiene una molecula de Carbono y dos moleculas de ...','Covalente','Dioxido de Carbono')",
        "INSERT INTO 'HELPS' VALUES (9, 3, 3, 'Si el CO2 se inhala puede ser nocivo para la salud','Covalente','Dioxido de Carbono')",
        "INSERT INTO 'HELPS' VALUES (10,4, 1, 'El amoniaco esta compuesto por Nitrógeno y ...','Covalente','Amoniaco')",
        "INSERT INTO 'HELPS' VALUES (11,4, 2, 'El amoniaco esta compuesto por Hidrógeno y ...','Covalente','Amoniaco')",
        "INSERT INTO 'HELPS' VALUES (12,4, 3, 'El NH3 es usado frecuentemente como fertilizante agrícola','Covalente','Amoniaco')",
        "INSERT INTO 'HELPS' VALUES (13,5, 1, 'El Metano es un gas compuesto por carbono y ...','Covalente','Metano')",
        "INSERT INTO 'HELPS' VALUES (14,5,2,'El Metano es un gas compuesto por Hidrógeno y ...','Covalente','Metano')",
        "INSERT INTO 'HELPS' VALUES (15,5,3,'Este gas funciona como efecto invernadero. Su formula es CH4','Covalente','Metano')",
        "INSERT INTO 'HELPS' VALUES (16,6,1,'El óxido de magnesio, contiene un átomo de magnesio y uno de ...','Ionico','Oxido de Magnesio')",
        "INSERT INTO 'HELPS' VALUES (17,6,2,'El óxido de magnesio, contiene un átomo de Oxígeno y uno de ...','Ionico','Oxido de Magnesio')",
        "INSERT INTO 'HELPS' VALUES (18,6,3,'El óxido de magnesio funciona como antiácido, su formula es MgO','Ionico','Oxido de Magnesio')",
        "INSERT INTO 'HELPS' VALUES (19,7,1,'Los enlaces iónicos están conformados a partir de un elemento metálico unido a uno ','Ionico','Cloruro de Sodio')",
        "INSERT INTO 'HELPS' VALUES (20,7,2,'El Cloruro de sodio contiene el elemento Cloro y el elemento...','Ionico','Cloruro de Sodio')",
        "INSERT INTO 'HELPS' VALUES (21,7,3,'Es comunmente conocido como la Sal de Mesa, contiene sodio y ','Ionico','Cloruro de Sodio')",
        "INSERT INTO 'Helps' VALUES (22,8,1,'Este enlace contiene un átomo de potasio y uno de ','Ionico','Cloruro de Potasio')",
        "INSERT INTO 'Helps' VALUES (23,8,2,'Este enlace contiene un átomo de cloro y uno de ','Ionico','Cloruro de Potasio')",
        "INSERT INTO 'Helps' VALUES (24,8,3,'Uno de los elementos que forman este enlace está representado por la letra K, el otro es el cloro','Ionico','Cloruro de Potasio')",
        "INSERT INTO 'Helps' VALUES (25,9,1,'Este enlace esta formado por un átomo de Fluor y un átomo de ....','Ionico','Fluoruro de Litio')",
        "INSERT INTO 'Helps' VALUES (26,9,2,'Este enlace esta formado por un átomo de Litio y un átomo de ....','Ionico','Fluoruro de Litio')",
        "INSERT INTO 'Helps' VALUES (27,9,3,'El elemento F y el elemento litio  forman el Fluoruro de Litio','Ionico','Fluoruro de Litio')",
        "INSERT INTO 'Helps' VALUES (28,10,1,'El elemento Zinc forma parte de este enlace iónico en combinación con el O...','Ionico','Oxido de Zinc')",
        "INSERT INTO 'Helps' VALUES (29,10,2,'El elemento Oxígeno forma parte de este enlace iónico en combinación con el Zn...','Ionico','Oxido de Zinc')",
        "INSERT INTO 'Helps' VALUES (30,10,3,'ZnO es la formula que corresponde al enlace iónico Óxido de Zinc','Ionico','Oxido de Zinc')",
        "INSERT INTO 'HELPS' VALUES (31,11,1,'El acero esta formado de carbono y ','Metalico','Acero')",
        "INSERT INTO 'HELPS' VALUES (32,11,2,'El acero esta formado de hierro y  de ..','Metalico','Acero')",
        "INSERT INTO 'HELPS' VALUES (33,11,3,'La fórmula del acero es C-Fe. Su electronegatividad es 0','Metalico','Acero')",
        "INSERT INTO 'HELPS' VALUES (34,12,1,'Uno de los elementos que forma el Latón es el cobre, y el otro es','Metalico','Laton')",
        "INSERT INTO 'HELPS' VALUES (35,12,2,'Uno de los elementos que forma el Latón es el Zinc, y el otro es ','Metalico','Laton')",
        "INSERT INTO 'HELPS' VALUES (36,12,3,'Los elementos que forman el Latón son: el Cobre y el Zinc','Metalico','Laton')",
        "INSERT INTO 'HELPS' VALUES (37,13,1,'Es comúnmente conocida como cal, contiene Calcio y ','Metalico','Oxido de Calcio')",
        "INSERT INTO 'HELPS' VALUES (38,13,2,'Es comúnmente conocida como cal, contiene oxígeno y Ca','Metalico','Oxido de Calcio')",
        "INSERT INTO 'HELPS' VALUES (39,13,3,'Ca-O, el oxido de Calcio se usa en la construccion, se le conoce como cal','Metalico','Oxido de Calcio')",
        "INSERT INTO 'Usuarios' VALUES ('admin', 'Administrador', 'Alem', '1', 'TecNM', '1A','aldo.up@culiacan.tecnm.mx')",            
        "INSERT INTO 'Usuarios' VALUES ('temp', 'Invitado', '123123', '1', 'TecNM', '1A','invitado@culiacan.tecnm.mx')",           
        "INSERT INTO 'Reacciones' VALUES (1,'Explosion','Aluminio','Cloro','-','El aluminio combinado con cloro produce una leve explosión.')",
        "INSERT INTO 'Reacciones' VALUES (2,'Efervescente','Carbono','Sodio','-','El Bicarbonato de sodio es una sustancia efervescente.')"
    };
    foreach (var x in cmdText)
    {
        dbCmd.CommandText = x;
        dbCmd.ExecuteNonQuery();
    }
    CerrarConexion2();
    Debug.Log("Base de datos creada satisfactoriamente");
}

public void ResetearBD()
{
    AbrirConexion();
    dbCmd = dbCon.CreateCommand();
    List<string> cmdText = new List<string>(){
        "DROP TABLE USUARIOS",
        "CREATE TABLE IF NOT EXISTS 'Usuarios' ('idUsuario'	TEXT NOT NULL UNIQUE, 'nombre' TEXT NOT NULL, 'pass' TEXT NOT NULL, 'status' INTEGER NOT NULL, 'escuela' TEXT, 'grupo'	TEXT, email text not null, PRIMARY KEY('idUsuario'))",
        "INSERT INTO 'Usuarios' VALUES ('admin', 'Administrador', 'Alem', '1', 'TecNM', '1A','aldo.up@culiacan.tecnm.mx')",
        "INSERT INTO 'Usuarios' VALUES ('temp', 'Invitado', '123123', '1', 'TecNM', '1A','invitado@culiacan.tecnm.mx')",
        "INSERT INTO 'Usuarios' VALUES ('temp1', 'Invitado', '123123', '1', 'TecNM', '1A','invitado1@culiacan.tecnm.mx')",
        "INSERT INTO 'Usuarios' VALUES ('temp2', 'Invitado', '123123', '1', 'TecNM', '1A','invitado2@culiacan.tecnm.mx')",
        "INSERT INTO 'Usuarios' VALUES ('temp3', 'Invitado', '123123', '1', 'TecNM', '1A','invitado3@culiacan.tecnm.mx')",
        "INSERT INTO 'Usuarios' VALUES ('temp4', 'Invitado', '123123', '1', 'TecNM', '1A','invitado4@culiacan.tecnm.mx')",
        "INSERT INTO 'Usuarios' VALUES ('temp5', 'Invitado', '123123', '1', 'TecNM', '1A','invitado5@culiacan.tecnm.mx')"         
    };
    foreach (var x in cmdText)
    {
        dbCmd.CommandText = x;
        dbCmd.ExecuteNonQuery();
    }
    CerrarConexion2();
    string msg = "la base de datos se ha reiniciado";
    Debug.Log(msg);
    StartCoroutine(Mostrar(msg));
}

public void CerrarDataSet2()
{
    reader.Close();
    reader = null;
    CerrarConexion2();
}

public void CerrarConexion2()
{
    dbCmd.Dispose();
    dbCmd = null;
    dbCon.Close();
    dbCon = null;
}

public void Consultar()
{
    input1 = GameObject.Find("txtUsuario").GetComponent<InputField>();
    input1.text = input1.text.ToLower();
    if (input1.text.Equals(""))
    {
        Debug.Log("NO SE PUEDEN CONSULTAR CLAVES VACIOS");
        return;
    }
    AbrirConexion();
    dbCmd = dbCon.CreateCommand();
    dbCmd.CommandText = "Select * from Usuarios where idUsuario = '" + input1.text + "' or email = '" + input1.text + "'";    // Es la consulta en codigo SQl
    reader = dbCmd.ExecuteReader();
    while (reader.Read())
    {
        //Recupera los datos del DataSet
        string id = reader.GetString(0);
        string nombre = reader.GetString(1);
        pass = reader.GetString(2);
        derecho = reader.GetInt32(3);
        GameObject.Find("lblUser2").GetComponent<Text>().text = reader.GetString(0);
        GameObject.Find("lblNombre2").GetComponent<Text>().text = reader.GetString(1);
        GameObject.Find("lblEmail2").GetComponent<Text>().text = reader.GetString(6);
        GameObject.Find("txtPass").GetComponent<InputField>().ActivateInputField();
    }
    CerrarDataSet2();
}

public void Select()
{
    Consultar();
}

public bool RevisarInputs()
{
    bool resultado = false;
    if (AddUser.text.Equals("") || addPass.Equals("") || AddGrupo.Equals("") || AddEscuela.Equals("") || this.AddEmail.Equals("") || this.AddName.Equals(""))
    {
        resultado = true;
    }
    Debug.Log("Resultado" + resultado);
    return resultado;
}

public void Insert()
{
    if (RevisarInputs() == true || ExisteUsuario())
    {
        return;
    }

    AbrirConexion();
    dbCmd = dbCon.CreateCommand(); //   crea el comando para insertar los datos        
    dbCmd.CommandText = "INSERT INTO usuarios (idUsuario, nombre, pass, status, escuela, grupo, email) VALUES ('" +
        AddUser.text + "', '" + AddName.text + "', '" + addPass.text + "', 1, '" + AddEscuela.text + "', '" + AddGrupo.text + "', '" + AddEmail.text + "')";
    dbCmd.ExecuteNonQuery();
    CerrarConexion2(); // Cerrando DataSet		
    StartCoroutine(Mostrar("Registro guardado existosamente"));
    GameObject.Find("PanelRegistro").GetComponent<RectTransform>().localScale = Vector3.zero;
    GameObject.Find("LoginPanel").GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 0.5f);
}

public IEnumerator Mostrar(string mensaje)
{
    GameObject.Find("pnlMensaje").GetComponent<RectTransform>().localScale = new Vector3(0.7f, 0.5f, 0.5f);
    GameObject.Find("txtMensaje").GetComponent<TextMeshProUGUI>().text = mensaje;
    yield return new WaitForSeconds(3);
    GameObject.Find("pnlMensaje").GetComponent<RectTransform>().localScale = Vector3.zero;
}

public void Actualizar()
{
    if (RevisarInputs() == false)
    {
        return;
    }
    AbrirConexion();
    dbCmd = dbCon.CreateCommand(); //   La consulta de la tabla en la base de datos

    dbCmd.CommandText = "UPDATE usuarios SET nombre = '" + input1.GetComponent<Text>().text + "', NOMBRE = '" + input1.GetComponent<Text>().text + "' WHERE IdUsuario =  " + input1.GetComponent<Text>().text;
    dbCmd.ExecuteNonQuery();
    Debug.Log("Se ha Actualizado el registro");
    CerrarConexion2();
}

public void Recargar()
{
    SceneManager.LoadScene(0);
}

public void GetIdSesion()
{
    AbrirConexion();
    dbCmd = dbCon.CreateCommand(); //   La consulta de la tabla en la base de datos
    dbCmd.CommandText = "Select COUNT(*) from Sesion";
    reader = dbCmd.ExecuteReader();
    while (reader.Read())
    {
        PlayerPrefs.SetInt("idSesion", reader.GetInt32(0));
    }
    Debug.Log("Sesion: " + PlayerPrefs.GetInt("idSesion"));
    CerrarDataSet2();
}

public void LogIn()
{
    InputField txtUsuario = GameObject.Find("txtUsuario").GetComponent<InputField>();
    InputField txtPass = GameObject.Find("txtPass").GetComponent<InputField>();
    Text txtUser = GameObject.Find("lblUser2").GetComponent<Text>();
    Text txtEmail = GameObject.Find("lblEmail2").GetComponent<Text>();
    Text txtNom = GameObject.Find("lblNombre2").GetComponent<Text>();        

    if (txtUsuario.text.Equals("") || txtPass.text.Equals("")){
        // Debug.Log("Favor de escribir usuario y contraseña");
        return;
    }
    AbrirConexion();
    dbCmd = dbCon.CreateCommand(); //   La consulta de la tabla en la base de datos
    dbCmd.CommandText = "Select count(*) from Usuarios where idUsuario = '" + txtUsuario.text + "' and pass = '" + txtPass.text +
        "' or email = '" + txtUsuario.text + "' and pass = '" + txtPass.text + "'";

    int scalar = Convert.ToInt32(dbCmd.ExecuteScalar());
    if (scalar == 1)
    {
        dbCmd.CommandText = "Select * from Usuarios where idUsuario = '" + txtUsuario.text + "' and pass = '" + txtPass.text +
            "' or email = '" + txtUsuario.text + "' and pass = '" + txtPass.text + "'";
        reader = dbCmd.ExecuteReader();     //Ejecuta la consulta

        while (reader.Read())
        {
            string nombre = reader.GetString(1);
            derecho = reader.GetInt32(3);
            GameObject.Find("lblNombre2").GetComponent<Text>().text = nombre;
            // salvando datos en la memoria interna del dispositivo
            PlayerPrefs.SetString("id", reader.GetString(0));
            PlayerPrefs.SetString("nombre", reader.GetString(1));
            PlayerPrefs.SetString("EMAIL", reader.GetString(6));
            PlayerPrefs.SetInt("derechos", derecho);
            PlayerPrefs.SetString("device", SystemInfo.deviceName);
        }
        reader.Close();
        reader = null;

        dbCmd.CommandText = "INSERT INTO 'Sesion' ('IdUsuario', 'Nombre', 'LastSesion') VALUES ('" + txtUser.text + "', '" + txtNom.text + "', '" +
            System.DateTime.Now.Date.ToShortDateString().ToString() + "')";
        dbCmd.ExecuteNonQuery();

        dbCmd.CommandText = "Select COUNT(*) from Sesion";
        reader = dbCmd.ExecuteReader();
        while (reader.Read())
        {
            PlayerPrefs.SetInt("idSesion", reader.GetInt32(0));
        }
        CerrarDataSet2();
        GameObject.Find("LoginPanel").GetComponent<RectTransform>().localScale = Vector3.zero;
        /* 
        GameObject g = new GameObject();
        g.AddComponent<DBManager>();
        g.GetComponent<DBManager>().BtnGuardar(PlayerPrefs.GetString("id"), PlayerPrefs.GetString("nombre"), PlayerPrefs.GetString("EMAIL")); */
/*
        SceneManager.LoadScene("menu2");
    }
    else
    {
        StartCoroutine(Mostrar("Usuario o contraseña incorrecta"));
    }
}

public void Ver()
{
    ExisteUsuario();
}

public bool ExisteUsuario()
{
    bool res = false;
    //  input1 = GameObject.Find("AddUsuario").GetComponent<InputField>();
    if (this.AddUser.text.Equals(""))
    {
        Debug.Log("NO SE PUEDEN CONSULTAR CLAVES VACIOS");
        return true;
    }
    AbrirConexion();
    dbCmd = dbCon.CreateCommand();
    dbCmd.CommandText = "Select count(*) from Usuarios where idUsuario = '" + AddUser.text + "' or email = '" + AddUser.text + "'";    // Es la consulta en codigo SQl

    int scalar = Convert.ToInt32(dbCmd.ExecuteScalar());
    if (scalar == 0)
    {
        res = false;
    }
    else
    {
        StartCoroutine(Mostrar("Usuario ya registrado"));
        res = true;
    }
    CerrarConexion2();
    return res;
}

public void Salir()
{
    Application.Quit();
}
public void OcultarLogIn()
{
    GameObject.Find("LoginPanel").GetComponent<RectTransform>().localScale = Vector3.zero;
    ClearInputs();
}

public void MostrarLogIn()
{
    GameObject.Find("PanelRegistro").GetComponent<RectTransform>().localScale = Vector3.zero;
    GameObject.Find("LoginPanel").GetComponent<RectTransform>().localScale = new Vector3(0.6f, 0.5f, 0.5f);
    GameObject.Find("txtUsuario").GetComponent<InputField>().text = GameObject.Find("fbUser").GetComponent<InputField>().text;
    GameObject.Find("txtPass").GetComponent<InputField>().text = GameObject.Find("fbPass").GetComponent<InputField>().text;
}

public void MostrarRegistro()
{
    GameObject.Find("LoginPanel").GetComponent<RectTransform>().localScale = Vector3.zero;
    GameObject.Find("PanelRegistro").GetComponent<RectTransform>().localScale = new Vector3(0.6f, 0.5f, 0.5f);
    ClearInputs();
}

public void OcultarRegistro()
{
    GameObject.Find("PanelRegistro").GetComponent<RectTransform>().localScale = Vector3.zero;
}

public void ClearInputs()
{
    this.addPass.text = "";
    this.AddEmail.text = "";
    this.AddUser.text = "";
    this.AddEscuela.text = "";
    this.AddGrupo.text = "";
    this.AddName.text = "";
}
/*
public void GetNUser()
{
    AbrirConexion();
    dbCmd = dbCon.CreateCommand(); //   La consulta de la tabla en la base de datos
    dbCmd.CommandText = "Select count(*) from Usuarios";
    int scalar = Convert.ToInt32(dbCmd.ExecuteScalar());
    scalar++;
    Debug.Log("Usuarios registrados" + scalar);
    GameObject.Find("AddUsuario").GetComponent<InputField>().text = scalar.ToString();
    dbCmd.Dispose();
    dbCmd = null;
    dbCon.Close();
    dbCon = null;
}
}
*/