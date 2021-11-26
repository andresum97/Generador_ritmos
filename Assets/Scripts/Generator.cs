using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using Random=UnityEngine.Random;

public class Generator : MonoBehaviour
{
   
    // private double metric;

    //Sonidos
    public AudioSource[] sounds;
    public AudioSource[] acordes;

    public AudioSource metricSound;
    public AudioSource rythmSound;
    public AudioSource fillerSound;


    public int BPM;
    private int seed;
    private int subdivisionBase = 4;
    private float soundPerSecond;
    private float sonidosPiano;
    private float time;
    private float time2;
    private float time3;
    private int cont = 0;
    private int beat = 0;

    private float numCompases = 8f;
    private int acordeActual;


    private double interval;
    public Text BPM_text;
    // public Text metricaObject;
    private bool subdivision = false;

    public Text seed_text;
    public Text rythm_text;
    public Text filler_text;
    public Text note_text;

    public Text nota_base;
    

    //Para acordes
    private int tocandoAhoraAcorde;
    private int tocandoAhoraMelodia;
    private int estructura;
    private int tocandoAhoraEstructura;

    public bool playSound;
    private bool playChord;
    private bool playingMelody;

    List<string> notasFinal = new List<string>();
    List<AudioSource> notasTonos = new List<AudioSource>();
    List<int> notasFinalMusica = new List<int>();
    List<int> notasMelodiaMusica = new List<int>();

    List<float> longitudMelodia = new List<float>();



    Dictionary<int, List<int>> DictFiller = new Dictionary<int, List<int>>();
    Dictionary<int, List<int>> DictMetrica = new Dictionary<int, List<int>>();
    Dictionary<int, List<int>> DictRitmo = new Dictionary<int, List<int>>();
    Dictionary<int, List<int>> DictAcorde = new Dictionary<int, List<int>>();
    Dictionary<int, List<float>> DictAcordeLongitud = new Dictionary<int, List<float>>();
    Dictionary<int, List<int>> DictMelodia = new Dictionary<int, List<int>>();
    Dictionary<int, List<float>> DictMelodiaLongitud = new Dictionary<int, List<float>>();


    // private int cantTime = 100;
    // private bool subdivision2 = false;

    List<int> filler = new List<int>();
    List<int> rythm = new List<int>();
    List<int> metric = new List<int>();

    List<int> listaEstructura = new List<int>();

    List<string> notasParaAcordes = new List<string>();
    List<int> acordesFinales = new List<int>();
    List<float> longitudAcordes = new List<float>();

    List<int> compas = new List<int>(); //Se tratabajara con 16 valores para evitar usar decimales como 1/2
    List<string> NOTAS = new List<string>() {"C","C#","D","D#","E","F","F#","G","G#","A","A#","B","C5","C#5","D5","D#5 ","E5","F5","F#5","G5","G#5","A5","A#5","B5"};
    List<int> FORMULA = new List<int>() {2,2,1,2,2,2,1};
    List<int> ACORDESF = new List<int>() {0,2,4};
    List<int> ACORDESMAYOR = new List<int>() {4,7};
    List<int> ACORDESMENOR = new List<int>() {3,7};
    List<int> ACORDESDIS = new List<int>() {3,6};
    List<int> ACORDESAUM = new List<int>() {4,8};
    List<int> TONICA = new List<int>() {1,3,6};
    List<int> SUBDOMINANTE = new List<int>() {2,4};
    List<int> DOMINANTE = new List<int>() {5,7};
        
    private int[] cantSubdivision = {3,4}; // 3/4, 4/4
    

    // private IEnumerator coroutine;
    // bool en vez de IEnumerator
    public bool Player;

    public Dictionary<List<int>,List<float>> formas = new Dictionary<List<int>,List<float>>();
    public List<List<int>> formasLista = new List<List<int>>();
    private List<int> calidad = new List<int>();



    // Start is called before the first frame update
    void Start()
    {
        BPM = 60; //Int32.Parse(BPM_text.text);
        seed = 123;
        Player = false; // coroutine = player()
        int grado = 3;
        string nota = "E";
        acordeActual = 0;
        tocandoAhoraEstructura = 0;
        var resultados = calculoEscala(nota,grado);
        calculoAcordes(resultados.Item1,resultados.Item2,resultados.Item3);
    }

    // void Update()
    // {
    //     if(BPM != Int32.Parse(BPM_text.text))
    //     {
    //         interval = 60.0f / Int32.Parse(BPM_text.text);
    //     }
    //     BPM = Int32.Parse(BPM_text.text);
    // }

    public void createSeed()
    {
        // seed = (int)System.DateTime.Now.Ticks;
        seed_text.text = seed.ToString();
        Random.seed = int.Parse(seed_text.text);
        subdivisionBase = cantSubdivision[Random.Range(0,cantSubdivision.Length)];
        beat = 0;
        cont = 0;
        estructura = 0;
        playSound = false;
        playingMelody = false;
        // filler.Clear();
        // rythm.Clear();
        // metric.Clear();
        listaEstructura.Clear();
        listaEstructura.Add(0);
        DictAcorde.Clear();
        DictAcordeLongitud.Clear();
        DictMelodiaLongitud.Clear();
        DictMelodia.Clear();
        DictFiller.Clear();
        DictMetrica.Clear();
        DictRitmo.Clear();
    
        createFill();
        GenerarEscala();
        for(int i = 0; i < 3; i++){
            int random = Random.Range(0,2); //Random de estructuras
            if(random == 0)
            {
                estructura += 1;
                createFill();
                GenerarEscala();
                listaEstructura.Add(estructura);
            }
            else{
                int repetido = Random.Range(0,listaEstructura.Count);
                listaEstructura.Add(repetido);
            }
        }
    }


    public void createFill()
    {
        rythm = new List<int>();
        metric = new List<int>();
        filler = new List<int>();

        int typeOfNote = Random.Range(0,3);

        if(typeOfNote == 0) //Notas negras
        {
            for(int i = 0; i < subdivisionBase; i++)
            {
                rythm.Add(1);
                for (int j = 0; j < 3; j++)
                {
                    rythm.Add(0);
                }
            }
        }
        
        else
        if(typeOfNote == 1) //Notas corcheas
        {
            for (int i = 0; i < subdivisionBase*2; i++)
            {
                rythm.Add(1);
                rythm.Add(0);
            }
        }
        else //Notas semicorcheas
        {
            for(int i = 0; i < subdivisionBase*4; i++)
            {   
                rythm.Add(1);
            }
        }
        
        //Se llena el ritmo
        for (int i = 0; i < subdivisionBase; i++)
        {
            metric.Add(1);
            for (int j = 0; j < 3; j++)
            {
                metric.Add(0);
            }
        }

        for(int i = 0; i < rythm.Count; i++)
        {
            if(rythm[i] == 0)
            {
                if(Random.Range(0,2) == 1)
                {
                    filler.Add(1);
                }
                else
                {
                    filler.Add(0);
                }
            }
            else
            if(rythm[i] == 1)
            {
                filler.Add(0);
            }
        }
        filler_text.text = "Relleno: "+ string.Join(", ",filler);
        DictRitmo.Add(estructura, rythm);
        DictMetrica.Add(estructura, metric);
        DictFiller.Add(estructura, filler);
        Debug.Log("Filler "+filler);
        Debug.Log("Metrica "+metric);
        Debug.Log("Rythm "+rythm);
    }

    (List<string>,string,string) calculoEscala(string nota,int grado)
    {
        int indice = NOTAS.IndexOf(nota);
        List<string> resultado = new List<string>();
        int indice_escala = 0;
        int cont_tono = 0;
        resultado.Add(nota);
        foreach (var tono in FORMULA)
        {
            cont_tono = cont_tono + tono;
            indice_escala = indice + cont_tono;
            resultado.Add(NOTAS[indice_escala%12]);
        }
        Debug.Log("Resultado ->"+string.Join(", ",resultado));
        string rol = "";
        if(TONICA.Contains(grado)){
            rol = "Tonica";
        }
        else
        if(SUBDOMINANTE.Contains(grado)){
            rol = "Subdominante";
        }
        else
        if(DOMINANTE.Contains(grado)){
            rol = "Dominante";
        }

        string notaselec = resultado[grado-1];

        return (resultado, notaselec,rol);
    }

    void GenerarEscala()
    {
        notasFinalMusica = new List<int>();
        notasMelodiaMusica = new List<int>();
        // List<int> NotasSeleccionadas = new List<int>();
        string notaBase = NOTAS[Random.Range(0, NOTAS.Count)];
        note_text.text = "Nota Base: "+notaBase;
        Debug.Log("Llego aqui y el valor es "+notaBase);
        List<string> escala = new List<string>();
        int pos  = 0;
        if(NOTAS.Contains(notaBase))
        {
            pos = NOTAS.IndexOf(notaBase);
        }
        escala.Add(notaBase);
        for (int i = 1; i < 7; i++)
        {
            if (i == 3)
            {
                pos += 1;
            }
            else
            {
                pos += 2;
            }
            escala.Add(NOTAS[pos % NOTAS.Count]);
            Debug.Log("Escala ->"+escala[i]);
        }
        // //Primera forma
        // verificacionAcordes(escala);
        // //Siguiente creacion de formas
        // for(int i = 0; i < 3; i++)
        // {
        //     // Probabilidad de utilizar misma o diferente forma
        //     int prob = Random.RandomRange(0,10);
        //     //Utilizar un repetido
        //     if(prob < 4){
        //         int numElements = formas.Count;
        //         int prob2 = Random.RandomRange(0,numElements);
        //         formasLista.Add(formas.ElementAt(prob2).Key);
        //     }else
        //     {
        verificacionAcordes(escala);   
        // verificacionDuracionAcordes();
        // verificarAcordesTocados();
        //     }
        // }
        // foreach(KeyValuePair<List<int>,List<float>> ele in formas)
        // {
        //     Debug.Log("Key = {0} "+string.Join(", ",ele.Key)+" Value = "+string.Join(", ", ele.Value));
        // }
    }


    void verificacionAcordes(List<string> escala)
    {
        // notasParaAcordes.Clear();
        // longitudAcordes.Clear();
        // acordesFinales.Clear();
        string actual = "subdominante";
        int indice = 0;
        numCompases = 8f;
        notasParaAcordes = new List<string>();
        longitudAcordes = new List<float>();
        longitudMelodia = new List<float>();
        acordesFinales = new List<int>();
        for(int i = 0; i < 7; i++)
        {
            List<string> temp = new List<string>();
            notasParaAcordes.Add(escala[i]);
            if (i+2 > escala.Count)
            {
                notasParaAcordes.Add(escala[(i+2)%escala.Count]);
            }else{
                notasParaAcordes.Add(escala[(i+2)%escala.Count]);
            }
            if (i+4 > escala.Count)
            {
                notasParaAcordes.Add(escala[(i+4)%escala.Count]);
            }else
            {
                notasParaAcordes.Add(escala[(i+4)%escala.Count]);
            }

            temp.Add(notasParaAcordes[(i*3)]);
            temp.Add(notasParaAcordes[(i*3)+1]);
            temp.Add(notasParaAcordes[(i*3)+2]);
        }

        foreach(var item in notasParaAcordes)
        {
            Debug.Log("Notas obtenidas" + item.ToString());
        }

        Debug.Log("Numero de compases "+numCompases);

        while(numCompases > 0.0f)
        {
            // 1/4 de proabilidad de 4 compases
            if(Random.Range(0,4) == 3)
            {
                if(numCompases - 4 >=0)
                {
                    numCompases -= 4.0f;
                    longitudAcordes.Add(4f);
                    int random = Random.Range(0,4);
                    if (random == 0){
                        longitudMelodia.Add(2f);
                        longitudMelodia.Add(2f);
                    }else
                    if(random == 1){
                        longitudMelodia.Add(1f);
                        longitudMelodia.Add(1f);
                        longitudMelodia.Add(1f);
                        longitudMelodia.Add(1f);
                    }
                    else
                    if(random == 2){
                        longitudMelodia.Add(4f);
                    }else{
                        longitudMelodia.Add(0.5f);
                        longitudMelodia.Add(0.5f);
                        longitudMelodia.Add(0.5f);
                        longitudMelodia.Add(0.5f);
                        longitudMelodia.Add(0.5f);
                        longitudMelodia.Add(0.5f);
                        longitudMelodia.Add(0.5f);
                        longitudMelodia.Add(0.5f);
                    }
                }
            }
            else
            {
                // 1/3 de probabilidad de 2 compases
                if(Random.Range(0,3) == 2)
                {
                    if(numCompases - 2 >= 0)
                    {
                        numCompases -= 2.0f;
                        longitudAcordes.Add(2f);
                        int random = Random.Range(0,3);
                        if(random == 0)
                        {
                            longitudMelodia.Add(2f);
                        }
                        else
                        if(random == 1)
                        {
                            longitudMelodia.Add(1f);
                            longitudMelodia.Add(1f);
                        }
                        else
                        {
                            longitudMelodia.Add(0.5f);
                            longitudMelodia.Add(0.5f);
                            longitudMelodia.Add(0.5f);
                            longitudMelodia.Add(0.5f);
                        }
                    }
                }
                else
                {
                    // 1/2 de probabilidad de 1 compas
                    if(Random.Range(0,2) == 1)
                    {
                        if(numCompases -1 >= 0)
                        {
                            numCompases -= 1.0f;
                            longitudAcordes.Add(1f);
                            longitudMelodia.Add(1f);
                        }
                    }
                    else
                    {
                        if(numCompases - 0.5f >= 0)
                        {
                            numCompases -= 0.5f;
                            longitudAcordes.Add(0.5f);
                            longitudMelodia.Add(0.5f);
                        }
                    }
                }
            }
        }
        DictAcordeLongitud.Add(estructura, longitudAcordes);
        DictMelodiaLongitud.Add(estructura, longitudMelodia);

        foreach(var item in longitudAcordes)
        {
            Debug.Log("longitud de item "+item);
        }

        //Guardar acordes para tocarlos
        for(int i = 0; i<DictAcordeLongitud[estructura].Count; i++)
        {
            string nota = "";
            string nota2 = "";
            string nota3 = "";
            if(actual == "tonica")
            {
                int temp = Random.Range(0,4);
                if(temp == 0)
                {
                    int tonica = Random.Range(0,3);
                    if(tonica == 1)
                    {
                        nota = notasParaAcordes[0];
                        nota2 = notasParaAcordes[1];
                        nota3 = notasParaAcordes[2];
                    }
                    else
                    if(tonica == 2)
                    {
                        nota = notasParaAcordes[6];
                        nota2 = notasParaAcordes[7];
                        nota3 = notasParaAcordes[8];
                    }
                    else
                    {
                        nota = notasParaAcordes[15];
                        nota2 = notasParaAcordes[16];
                        nota3 = notasParaAcordes[17];
                    }
                    actual = "tonica";
                }
                //subdominante
                else
                if(temp >= 1 && temp < 3){
                    int subdominante = Random.Range(0,3);
                    if(subdominante == 1)
                    {
                        nota = notasParaAcordes[3];
                        nota2 = notasParaAcordes[4];
                        nota3 = notasParaAcordes[5];
                    }
                    else
                    {
                        nota = notasParaAcordes[9];
                        nota2 = notasParaAcordes[10];
                        nota3 = notasParaAcordes[11];
                    }
                    actual = "subdominante";
                }
                else
                {
                    nota = notasParaAcordes[12];
                    nota2 = notasParaAcordes[13];
                    nota3 = notasParaAcordes[14];
                    actual = "dominante";
                }
            }
            else
            if(actual == "subdominante")
            {
                int temp = Random.Range(0,5);
                //Tonica
                if(temp < 2)
                {
                    int tonica = Random.Range(0,3);
                    if(tonica == 1)
                    {
                        nota = notasParaAcordes[0];
                        nota2 = notasParaAcordes[1];
                        nota3 = notasParaAcordes[2];
                    }
                    else
                    if(tonica == 2)
                    {
                        nota = notasParaAcordes[6];
                        nota2 = notasParaAcordes[7];
                        nota3 = notasParaAcordes[8];
                    }
                    else
                    {
                        nota = notasParaAcordes[15];
                        nota2 = notasParaAcordes[16];
                        nota3 = notasParaAcordes[17];
                    }
                    actual = "tonica";
                }
                else
                if(temp == 2)
                {
                    int subdominante = Random.Range(0,3);
                    if(subdominante == 1)
                    {
                        nota = notasParaAcordes[3];
                        nota2 = notasParaAcordes[4];
                        nota3 = notasParaAcordes[5];
                    }
                    else
                    {
                        nota = notasParaAcordes[9];
                        nota2 = notasParaAcordes[10];
                        nota3 = notasParaAcordes[11];
                    }
                    actual = "subdominante";
                }
                else
                {
                    nota = notasParaAcordes[12];
                    nota2 = notasParaAcordes[13];
                    nota3 = notasParaAcordes[14];
                    actual = "dominante";
                }
            }
            else
            if(actual == "dominante")
            {
                int temp = Random.Range(0,4);
                if(temp == 0)
                {
                    int tonica = Random.Range(0,3);
                    if(tonica == 1)
                    {
                        nota = notasParaAcordes[0];
                        nota2 = notasParaAcordes[1];
                        nota3 = notasParaAcordes[2];
                    }
                    else
                    if(tonica == 2)
                    {
                        nota = notasParaAcordes[6];
                        nota2 = notasParaAcordes[7];
                        nota3 = notasParaAcordes[8];
                    }
                    else
                    {
                        nota = notasParaAcordes[15];
                        nota2 = notasParaAcordes[16];
                        nota3 = notasParaAcordes[17];
                    }
                    actual = "tonica";
                }
                else
                //subdominante
                if(temp >= 1 && temp < 3)
                {
                    int subdominante = Random.Range(0,3);
                    if(subdominante == 1)
                    {
                        nota = notasParaAcordes[3];
                        nota2 = notasParaAcordes[4];
                        nota3 = notasParaAcordes[5];
                    }
                    else
                    {
                        nota = notasParaAcordes[9];
                        nota2 = notasParaAcordes[10];
                        nota3 = notasParaAcordes[11];
                    }
                    actual = "subdominante";
                }
                else
                {
                    nota = notasParaAcordes[12];
                    nota2 = notasParaAcordes[13];
                    nota3 = notasParaAcordes[14];
                    actual = "dominante";
                }
            }
            Debug.Log("Nota 3->"+nota3);
            acordesFinales.Add(NOTAS.IndexOf(nota));
            acordesFinales.Add(NOTAS.IndexOf(nota2));
            acordesFinales.Add(NOTAS.IndexOf(nota3));

            float count = 0f;
            for (int j = 0; j < DictMelodiaLongitud[estructura].Count; j++)
            {
                if(DictAcordeLongitud[estructura][i] <= count)
                {
                    indice = j+1;
                    break;
                }
                int random = Random.Range(0,3);
                if(random == 0)
                {
                    notasMelodiaMusica.Add(NOTAS.IndexOf(nota));
                }
                else
                if(random == 1)
                {
                    notasMelodiaMusica.Add(NOTAS.IndexOf(nota2));
                }
                else
                if(random == 2)
                {
                    notasMelodiaMusica.Add(NOTAS.IndexOf(nota3));
                }
                count += DictMelodiaLongitud[estructura][(j+indice)%DictMelodiaLongitud[estructura].Count];
            }

        }
        DictAcorde.Add(estructura, acordesFinales);
        DictMelodia.Add(estructura, notasMelodiaMusica);
        Debug.Log("Notas para acorde size->"+notasParaAcordes.Count);
        foreach (var item in acordesFinales)
        {
            Debug.Log("Item acordes finales "+item);
        }
        // try
        // {
        //     formas.Add(acordesFinales,longitudAcordes);
        //     formasLista.Add(acordesFinales);
        // }
        // catch (System.Exception)
        // {
        // }
    }

    void calculoAcordes(List<string> notas,string notaselec, string rol)
    {
        string tipo = "";
        foreach(var nota in notas){
            int indice = notas.IndexOf(nota);
            List<string> resultado = new List<string>();
            int indice_escala = 0;
            foreach(var cont in ACORDESF){
                indice_escala = indice + cont;
                resultado.Add(notas[indice_escala%7]);
            }

            int indice_raiz = NOTAS.IndexOf(resultado[0]);

            int indice_tercera1 = -1;
            int indice_tercera2 = -1;
            cont = indice_raiz;
            while(indice_tercera1 == -1 || indice_tercera2 == -1){
                int i = cont % 12;
                if(resultado[1] == NOTAS[i])
                {
                    indice_tercera1 = cont;
                }
                if(resultado[2] == NOTAS[i])
                {
                    indice_tercera2 = cont;
                }
                cont++;
            }

            int primer_tercera = indice_tercera1 - indice_raiz;
            int segunda_tercera = indice_tercera2 - indice_raiz;
            
            List<int> temp = new List<int>() {primer_tercera, segunda_tercera};

            Debug.Log("Lista de temp "+string.Join(", ",temp));

            if(temp.Intersect(ACORDESMAYOR).Count() == 2)
            {
                Debug.Log("Ingreso aqui mayor");
                tipo = "mayor";
            }
            else
            if(temp.Intersect(ACORDESMENOR).Count() == 2)
            {
                Debug.Log("Ingreso aqui menor");
                tipo = "menor";
            }
            else
            if(temp.Intersect(ACORDESDIS).Count() == 2)
            {
                Debug.Log("Ingreso aqui dis");
                tipo = "disminuido";
            }
            else
            if(temp.Intersect(ACORDESAUM).Count() == 2)
            {
                Debug.Log("Ingreso aqui aum");
                tipo = "aumentado";
            }

            Debug.Log("El tipo es "+tipo);

            // if(nota == notaselec)
            // {
            Debug.Log("Acorde de "+nota+" : "+string.Join(", ",resultado)+ "| "+tipo+" y su rol es: "+rol);
            // }
            
        }
    }

    void crearCompas()
    {
        List<int> opcionesCompas = new List<int>() {8,4,2,1};
        var res = 0;
        while(res < 16){
            var valorRandom = Random.Range(0,opcionesCompas.Count());
            if((res+opcionesCompas[valorRandom])<16){
                Debug.Log("Toca probar aqui");
            }
        }
    }
        

    // public void StartPlayer()
    // {
    //     BPM = int.Parse(BPM_text.text);
    // }

    // Update is called once per frame
    void Update()
    {
        // for(int i = 0; i < listaEstructura.Count; i++){
        //         Debug.Log("Lista estructura i"+listaEstructura[i]);
        // }
        if(Player)
        {
            BPM = int.Parse(BPM_text.text);
            soundPerSecond = (60.0f / (BPM*4));
            sonidosPiano = (60.0f/BPM);
            time += Time.deltaTime;
            time2 += Time.deltaTime;
            time3 += Time.deltaTime;
            if(time >= soundPerSecond)
            {
                if(beat == DictFiller[listaEstructura[tocandoAhoraEstructura]].Count)
                {
                    beat = 0;

                    // for (int i = 0; i < DictFiller[listaEstructura[tocandoAhoraEstructura]].Count; i++)
                    // {

                    // }
                }
                if(DictFiller[listaEstructura[tocandoAhoraEstructura]][beat%DictFiller[listaEstructura[tocandoAhoraEstructura]].Count] == 1)
                {
                    // sounds[0].Play();
                    fillerSound.Play();
                }
                if(DictMetrica[listaEstructura[tocandoAhoraEstructura]][beat%DictMetrica[listaEstructura[tocandoAhoraEstructura]].Count] == 1)
                {
                    metricSound.Play();
                }
                if(DictRitmo[listaEstructura[tocandoAhoraEstructura]][beat%DictRitmo[listaEstructura[tocandoAhoraEstructura]].Count] == 1)
                {
                    rythmSound.Play();
                }
                time = 0.0f;
                beat += 1;
            }
            if(time2 <= (sonidosPiano*DictAcordeLongitud[listaEstructura[tocandoAhoraEstructura]][acordeActual%DictAcordeLongitud[listaEstructura[tocandoAhoraEstructura]].Count]))
            {
                Debug.Log("Ingreso al if de update ");
                if(acordeActual == DictAcordeLongitud[listaEstructura[tocandoAhoraEstructura]].Count-1)
                {
                    acordeActual = 0;
                    tocandoAhoraEstructura += 1;
                    tocandoAhoraMelodia = 0;
                    beat = 0;
                    if(tocandoAhoraEstructura == listaEstructura.Count)
                    {
                        tocandoAhoraEstructura = 0;
                    }
                }
                if(playSound == false)
                {
                    playSound = true;
                    nota_base.text = "Nota Acorde Base: "+NOTAS[DictAcorde[listaEstructura[tocandoAhoraEstructura]][acordeActual*3]%DictAcorde[listaEstructura[tocandoAhoraEstructura]].Count]; 
                    acordes[DictAcorde[listaEstructura[tocandoAhoraEstructura]][acordeActual*3]%DictAcorde[listaEstructura[tocandoAhoraEstructura]].Count].Play();
                    acordes[DictAcorde[listaEstructura[tocandoAhoraEstructura]][(acordeActual*3)+1]%DictAcorde[listaEstructura[tocandoAhoraEstructura]].Count].Play();
                    acordes[DictAcorde[listaEstructura[tocandoAhoraEstructura]][(acordeActual*3)+2]%DictAcorde[listaEstructura[tocandoAhoraEstructura]].Count].Play();
                    // acordes[acordesFinales[acordeActual]].Play();
                    // acordes[acordesFinales[acordeActual+1]].Play();
                    // acordes[acordesFinales[acordeActual+2]].Play();
                }                
            }
            else
            if(time2 > (sonidosPiano * DictAcordeLongitud[listaEstructura[tocandoAhoraEstructura]][acordeActual%DictAcordeLongitud[listaEstructura[tocandoAhoraEstructura]].Count]))
            {
                Debug.Log("Ingreso al else del update");
                acordes[DictAcorde[listaEstructura[tocandoAhoraEstructura]][acordeActual*3]%DictAcorde[listaEstructura[tocandoAhoraEstructura]].Count].Stop();
                acordes[DictAcorde[listaEstructura[tocandoAhoraEstructura]][(acordeActual*3)+1]%DictAcorde[listaEstructura[tocandoAhoraEstructura]].Count].Stop();
                acordes[DictAcorde[listaEstructura[tocandoAhoraEstructura]][(acordeActual*3)+2]%DictAcorde[listaEstructura[tocandoAhoraEstructura]].Count].Stop();
                // acordes[acordesFinales[acordeActual]].Stop();
                // acordes[acordesFinales[acordeActual+1]].Stop();
                // acordes[acordesFinales[acordeActual+2]].Stop();
                time2 = 0f;
                playSound = false;
                acordeActual += 1;
            }

            if(time3 <= (sonidosPiano*DictMelodiaLongitud[listaEstructura[tocandoAhoraEstructura]][tocandoAhoraMelodia % DictMelodiaLongitud[listaEstructura[tocandoAhoraEstructura]].Count]))
            {
                if (tocandoAhoraMelodia == DictMelodiaLongitud[listaEstructura[tocandoAhoraEstructura]].Count-1)
                {
                    tocandoAhoraMelodia = 0;
                }
                if (playingMelody == false)
                {
                    playingMelody = true;
                    acordes[DictMelodia[listaEstructura[tocandoAhoraEstructura]][tocandoAhoraMelodia]%DictMelodia[listaEstructura[tocandoAhoraEstructura]].Count].Play();
                }
            }
            else
            if(time3 > (sonidosPiano * DictMelodiaLongitud[listaEstructura[tocandoAhoraEstructura]][acordeActual % DictMelodiaLongitud[listaEstructura[tocandoAhoraEstructura]].Count]))
            {
                acordes[DictMelodia[listaEstructura[tocandoAhoraEstructura]][tocandoAhoraMelodia]%DictMelodia[listaEstructura[tocandoAhoraEstructura]].Count].Stop();
                playingMelody = false;
                time3 = 0f;
                tocandoAhoraMelodia += 1;
            }
        }
        else
        {
            beat = 0;
            acordeActual = 0;
            tocandoAhoraEstructura = 0;
            tocandoAhoraMelodia = 0;
            tocandoAhoraAcorde = 0;
        }
    }

    public void StartPlayer()
    {
        Player = !Player;
        if(Player == false){
            playingMelody = false;
            playSound = false;
        }
        // StartCoroutine(coroutine);
    }

    // public void StopPlayer()
    // {
    //     Player = false;
    //     // StopCoroutine(coroutine);
    // }

    // IEnumerator player()
    // {
    //     while(Time.time < cantTime)
    //     {
    //         cont++;
    //         if(cont % metric == 1)
    //         {
    //             sounds[0].Play();
    //         }
    //         else
    //         {
    //             sounds[1].Play();
    //         }
    //         yield return new WaitForSecondsRealtime((float)interval);
    //     }
    // }
}
