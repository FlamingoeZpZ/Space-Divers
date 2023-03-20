using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Application = UnityEngine.Application;

[DefaultExecutionOrder(-20)]
public class Settings : MonoBehaviour
{
    public static Gradient verticalityGradient { get; private set; }
    public static Color enemyTargetingPlayer { get; set; }
    public static Color enemyLostPlayer { get;  set; }
    
    [Header("Radar")]
    
    [SerializeField] private Material gradientMaterial;
    [SerializeField] private Material myShipMaterial;
    [SerializeField] private Material hiddenShipMaterial;
    
    private readonly int belowColor = Shader.PropertyToID("_BelowColor");
    private readonly int aboveColor = Shader.PropertyToID("_AboveColor");
    private readonly int shipColA = Shader.PropertyToID("_ColorA");
    private readonly int shipColB = Shader.PropertyToID("_ColorB");
    private readonly int shipColC = Shader.PropertyToID("_ColorC");
    private readonly int shipColEmissive = Shader.PropertyToID("_EmissiveColor");
    private readonly int shipColIntensity = Shader.PropertyToID("_Intensity");

    private string SaveFolder; 

    public static Settings instance { get; private set; }
    
    //public static Action OnSettingsSaved;
    
    //This should only be in the title screen
    private void Start()
    {
        
       
        string st= SaveFolder  = Application.persistentDataPath + "/Ships";
        SaveFolder += '/';
        if(!Directory.Exists(st))
        {    
            //if it doesn't, create it
            Directory.CreateDirectory(st);
            //This is pretty much game first load.
            SaveShip(0, Resources.Load("Bodies/Body") as Transform); // Save ship for first time.   
        }

        print("Target directory: " + SaveFolder);
        
        instance = this;
        
        //LoadGameInfo();
        //LoadSettings();
        LoadShip(0);
        
    }

    private void LoadSettings()
    {
        Color aboveY = ((uint)PlayerPrefs.GetInt("AboveCol", 65535)).ToColor();
        Color belowY = ((uint)PlayerPrefs.GetInt("BelowCol", -16776961)).ToColor();
        gradientMaterial.SetColor(belowColor, belowY); // Default of red
        gradientMaterial.SetColor(aboveColor, aboveY); // Default of blue
        verticalityGradient = new ()
        {
            colorKeys = new GradientColorKey[]
            {
                new (belowY, 0),
                new (aboveY, 1),
            }
        };
        enemyTargetingPlayer  = ((uint)PlayerPrefs.GetInt("enemyTargetingPlayer", 2139029759)).ToColor(); // Default orange
        enemyLostPlayer = ((uint)PlayerPrefs.GetInt("enemyLostPlayer", -65281)).ToColor(); // default yellow
    }

    private void LoadShip(int i)
    {
        StreamReader sr = new StreamReader(SaveFolder + "shipData" + i + ".dat");

        int[] elems = { shipColA, shipColB, shipColC, shipColEmissive };
        foreach (int t in elems)
        {
            Color c = uint.Parse(sr.ReadLine()).ToColor();
            hiddenShipMaterial.SetColor(t, c);
            myShipMaterial.SetColor(t, c);
        }

        float intensity = float.Parse(sr.ReadLine());
        myShipMaterial.SetFloat(shipColIntensity, intensity);
        hiddenShipMaterial.SetFloat(shipColIntensity, intensity);

        sr.Close();
    }


    private void LoadGameInfo()
    {
        StreamReader sr = new StreamReader(SaveFolder + "gameInfo.dat");
        
        sr.Close();
    }

    public void SaveGameInfo()
    {
        StreamWriter sw = new StreamWriter(SaveFolder + "gameInfo.dat");

        sw.Close();
    }

    
    public void SaveShip(int i, Transform bodyRoot)
    {
        StreamWriter sw = new StreamWriter(SaveFolder + "shipData" + i + ".dat");
        
        //----Save Color Data---
        //First save material
        //if(AssetDatabase.Contains())
        //AssetDatabase.CreateAsset(new Material(), "");
        
        //Then save colors
        sw.WriteLine(myShipMaterial.GetColor(shipColA).ToRgba());
        sw.WriteLine(myShipMaterial.GetColor(shipColB).ToRgba());
        sw.WriteLine(myShipMaterial.GetColor(shipColC).ToRgba());
        sw.WriteLine(myShipMaterial.GetColor(shipColEmissive).ToRgba());
        sw.WriteLine(myShipMaterial.GetFloat(shipColIntensity));
        
        //Save the body
        sw.WriteLine(bodyRoot.GetComponent<ShipComponent>().MyType+"/"+ bodyRoot.name);
        
        //First child is reserved for colliders
        Queue<Transform> parts = new Queue<Transform>();
        parts.Enqueue(bodyRoot);

        while (parts.Count != 0)
        {
            Transform t = parts.Peek();
            sw.WriteLine(t.GetComponent<ShipComponent>().MyType + "/" + t.name);

            for (int j = 1; j < t.childCount; ++j)
            {
                Transform n = t.GetChild(j);
                if(n.childCount != 0)
                    parts.Enqueue(n.GetChild(0));
            }

            parts.Dequeue();
        }

        /*
        for(int n = 0; n < part.childCount; ++n)
        {
            for (int j = 1; j < childCount; ++j)
            {
                
                    if (part.childCount == 0)
                    {
                        sw.WriteLine(-1);
                    }
            }
            //Semi recursive. If we've
            part = bodyRoot.GetChild(j);
        } */

        sw.Close();
    }
    
    public void SaveSettings()
    {
        PlayerPrefs.SetInt("BelowCol", (int)gradientMaterial.GetColor(belowColor).ToRgba());
        PlayerPrefs.SetInt("AboveCol", (int)gradientMaterial.GetColor(aboveColor).ToRgba());
        PlayerPrefs.SetInt("enemyTargetingPlayer", (int)enemyTargetingPlayer.ToRgba());
        PlayerPrefs.SetInt("enemyLostPlayer", (int)enemyLostPlayer.ToRgba());
        PlayerPrefs.Save();
    }
}

public static class Utilities
{
    public static uint ToRgba(this Color c)
    {
        return ((uint)(c.r*255) << 24) + ((uint)(c.g*255)<< 16) + ((uint)(c.b*255) << 8) + (uint)(c.a*255);
    }

    public static Color ToColor(this uint c)
    {
        //It's important when reading this, to read the bits not the value.
        uint r = c >> 24;
        uint g = (c  >> 16) - (r << 8);
        uint b = (c  >> 8) - (r << 16) - (g<< 8);
        uint a = c - (r << 24) - (g << 16) - (b<<8);
        return new Color(r, g, b, a)/255f;
    }
}

