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

    public int BPM;
    private int seed;
    private int subdivisionBase = 4;
    private float soundPerSecond;
    private float time;
    private int cont = 0;
    private int beat = 0;


    private double interval;
    public Text BPM_text;
    // public Text metricaObject;
    private bool subdivision = false;

    public Text seed_text;
    public Text rythm_text;
    public Text filler_text;

    // private int cantTime = 100;
    // private bool subdivision2 = false;

    List<int> filler = new List<int>();
    List<int> rythm = new List<int>();
    List<int> metric = new List<int>();

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
        string nota = "D";
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
            time += Time.deltaTime;
            if(time >= soundPerSecond)
            {
                beat += 1;
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
            }
        }
        else beat = 0;
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
