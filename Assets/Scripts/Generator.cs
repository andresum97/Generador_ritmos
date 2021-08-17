using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class Generator : MonoBehaviour
{
   
    private double metric;
    public AudioSource[] sounds;
    private int BPM;
    private int cont;
    private double interval;
    private int seed;
    public Text BPM_text;
    // public Text metricaObject;
    private bool subdivision = false;
    public Text seed_text;
    public Text key_text;
    public Text Filler;
    private int cantTime = 100;
    private bool subdivision2 = false;
    private int[] cantSubdivision = {3,4};
    private int[] subdivisionBase = {4};

    private int[] subdivisionClave = {1,2,4};

    private IEnumerator coroutine;



    // Start is called before the first frame update
    void Start()
    {
        BPM = Int32.Parse(BPM_text.text);
        interval = 60.0f / BPM;
        coroutine = player();
    }

    // Update is called once per frame
    void Update()
    {
        if(BPM != Int32.Parse(BPM_text.text))
        {
            interval = 60.0f / Int32.Parse(BPM_text.text);
        }
        BPM = Int32.Parse(BPM_text.text);
    }

    public void createSeed()
    {
        seed = (int)System.DateTime.Now.Ticks;
        seed_text.text = "Semilla: "+seed.ToString();
    }

    public void generateRythm()
    {
        bool calc = true;
        System.Random rand_cantsub = new System.Random();
        int cant_subdiv = cantSubdivision[rand_cantsub.Next(cantSubdivision.Length)];
        metric = cant_subdiv;
        System.Random rand_base = new System.Random();
        int cant_base = subdivisionBase[rand_base.Next(subdivisionBase.Length)];
        System.Random rand_key = new System.Random();
        int cant_clave = subdivisionClave[rand_key.Next(subdivisionClave.Length)];

        Debug.Log($"cantidad_subdivision: {cant_subdiv}");
        Debug.Log($"subdivision_base: {cant_base}");
        Debug.Log($"subdivision_clave: {cant_clave}");
    

        List<int> list_clave = new List<int>();
        int[] subdivision_group = {2,3};
        System.Random possible_groups = new System.Random();
        while (calc)
        {
            int posibilities = subdivision_group[possible_groups.Next(subdivision_group.Length)];
            int total_keys = (list_clave.Sum())+posibilities;
            if(total_keys < cant_clave)
            {
                list_clave.Add(posibilities);
            }
            else
            if(total_keys > cant_clave)
            {
                list_clave = new List<int>();
            }
            else
            if(total_keys == cant_clave)
            {
                list_clave.Add(posibilities);
                calc = false;
            }
        }

        List<bool> key = new List<bool>();
        List<bool> inverted_key = new List<bool>();
        key = createFill(list_clave);
        Debug.Log("Key"+string.Join(", ", key.Select(b => b.ToString()).ToArray()));
        key_text.text = $"Clave: {cant_clave}";
        Filler.text = "Relleno: "+string.Join(", ", key.Select(b => b.ToString()).ToArray());
        foreach (bool i in key)
        {       
            print(i);
        }
        
        StartPlayer();
    }

    public List<bool> createFill(List<int> keyp)
    {
        List<bool> temp = new List<bool>();
        foreach (int i in keyp)
        {
            if(i == 2)
            {
                temp.Add(true);
                temp.Add(false);
            }
            else 
            if(i == 3)
            {
                temp.Add(true);
                temp.Add(false);
                temp.Add(false);
            }
        }

        return temp;
    }

    public void StartPlayer()
    {
        StartCoroutine(coroutine);
    }

    public void StopPlayer()
    {
        StopCoroutine(coroutine);
    }

    IEnumerator player()
    {
        while(Time.time < cantTime)
        {
            cont++;
            if(cont % metric == 1)
            {
                sounds[0].Play();
            }
            else
            {
                sounds[1].Play();
            }
            yield return new WaitForSecondsRealtime((float)interval);
        }
    }
}
