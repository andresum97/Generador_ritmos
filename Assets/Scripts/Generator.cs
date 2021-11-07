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

    public int BPM;
    private int seed;
    private int subdivisionBase = 4;
    private float soundPerSecond;
    private float sonidosPiano;
    private float time;
    private float time2;
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
    

    //Para acordes
    private int tocandoAhora;

    public bool playSound;



    // private int cantTime = 100;
    // private bool subdivision2 = false;

    List<int> filler = new List<int>();
    List<int> rythm = new List<int>();
    List<int> metric = new List<int>();

    List<string> notasParaAcordes = new List<string>();
    List<int> acordesFinales = new List<int>();
    List<float> longitudAcordes = new List<float>();

    List<int> compas = new List<int>(); //Se tratabajara con 16 valores para evitar usar decimales como 1/2
    List<string> NOTAS = new List<string>() {"C","C#","D","D#","E","F","F#","G","G#","A","A#","B"};
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



    // Start is called before the first frame update
    void Start()
    {
        BPM = 120; //Int32.Parse(BPM_text.text);
        seed = 123;
        Player = false; // coroutine = player()
        int grado = 3;
        string nota = "E";
        acordeActual = 0;
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
        filler.Clear();
        rythm.Clear();
        metric.Clear();
        createFill();
        GenerarEscala();
    }


    public void createFill()
    {
        int typeOfNote = Random.Range(0,3);

        if(typeOfNote == 0) //Notas negras
        {
            for(int i = 0; i < subdivisionBase; i++)
            {
                metric.Add(1);
                for (int j = 0; j < 3; j++)
                {
                    metric.Add(0);
                }
            }
        }
        
        else
        if(typeOfNote == 1) //Notas corcheas
        {
            for (int i = 0; i < subdivisionBase*2; i++)
            {
                metric.Add(1);
                metric.Add(0);
            }
        }
        else //Notas semicorcheas
        {
            for(int i = 0; i < subdivisionBase*4; i++)
            {   
                metric.Add(1);
            }
        }
        
        //Se llena el ritmo
        for (int i = 0; i < subdivisionBase; i++)
        {
            rythm.Add(1);
            for (int j = 0; j < 3; j++)
            {
                rythm.Add(0);
            }
        }

        for(int i = 0; i < metric.Count; i++)
        {
            if(metric[i] == 0)
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
            if(metric[i] == 1)
            {
                filler.Add(0);
            }
        }
        filler_text.text = "Relleno: "+ string.Join(", ",filler);
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
        List<int> NotasSeleccionadas = new List<int>();
        string notaBase = note_text.text;
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
        }
        verificacionAcordes(escala);
    }


    void verificacionAcordes(List<string> escala)
    {
        notasParaAcordes.Clear();
        longitudAcordes.Clear();
        acordesFinales.Clear();
        string actual = "subdominante";
        numCompases = 8f;
        for(int i = 0; i < 7; i++)
        {
            List<string> temp = new List<string>();
            notasParaAcordes.Add(escala[i]);
            notasParaAcordes.Add(escala[(i+2)%escala.Count]);
            notasParaAcordes.Add(escala[(i+4)%escala.Count]);


            temp.Add(notasParaAcordes[i*3]);
            temp.Add(notasParaAcordes[i*3+1]);
            temp.Add(notasParaAcordes[i*3+2]);
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
                        }
                    }
                    else
                    {
                        if(numCompases - 0.5f >= 0)
                        {
                            numCompases -= 0.5f;
                            longitudAcordes.Add(0.5f);
                        }
                    }
                }
            }
        }
        foreach(var item in longitudAcordes)
        {
            Debug.Log("longitud de item "+item);
        }

        //Guardar acordes para tocarlos
        for(int i = 0; i<longitudAcordes.Count; i++)
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
            acordesFinales.Add(NOTAS.IndexOf(nota));
            acordesFinales.Add(NOTAS.IndexOf(nota2));
            acordesFinales.Add(NOTAS.IndexOf(nota3));
        }
        foreach (var item in acordesFinales)
        {
            Debug.Log("Item acordes finales "+item);
        }
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
        if(Player)
        {
            BPM = int.Parse(BPM_text.text);
            soundPerSecond = (60.0f / (BPM*4));
            sonidosPiano = (60.0f/BPM);
            time += Time.deltaTime;
            time2 += Time.deltaTime;
            if(time >= soundPerSecond)
            {
                if(beat == subdivisionBase*4)
                {
                    beat = 0;
                }
                if(metric[beat] == 1)
                {
                    sounds[0].Play();
                }
                if(filler[beat] == 1)
                {
                    sounds[1].Play();
                }
                if(rythm[beat] == 1)
                {
                    sounds[2].Play();
                }
                time = 0.0f;
                beat += 1;
            }
            if(time2 <= (sonidosPiano*longitudAcordes[acordeActual]))
            {
                Debug.Log("Ingreso al if de update ");
                if(acordeActual == longitudAcordes.Count-1)
                {
                    acordeActual = 0;
                }
                if(playSound == false)
                {
                    playSound = true;
                    acordes[acordesFinales[acordeActual]].Play();
                    acordes[acordesFinales[acordeActual+1]].Play();
                    acordes[acordesFinales[acordeActual+2]].Play();
                }                
            }
            else
                if(time2 > (sonidosPiano * longitudAcordes[acordeActual]))
                {
                    Debug.Log("Ingreso al else del update");
                    acordes[acordesFinales[acordeActual]].Stop();
                    acordes[acordesFinales[acordeActual+1]].Stop();
                    acordes[acordesFinales[acordeActual+2]].Stop();
                    time2 = 0f;
                    playSound = false;
                    acordeActual += 1;
                }
        }
        else
        {
            beat = 0;
            acordeActual = 0;
        }
    }

    public void StartPlayer()
    {
        Player = !Player;
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
