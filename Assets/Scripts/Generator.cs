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
    
    private int[] cantSubdivision = {3,4}; // 3/4, 4/4
    

    // private IEnumerator coroutine;
    // bool en vez de IEnumerator
    public bool Player;



    // Start is called before the first frame update
    void Start()
    {
        BPM = 120; //Int32.Parse(BPM_text.text);
        seed = 123;
        Player = false; // coroutine = player();
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
