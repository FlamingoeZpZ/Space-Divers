using System.IO;
using UnityEngine;
using UnityEngine.WSA;
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

    private string SaveFolder; 

    //public static Action OnSettingsSaved;
    
    //This should only be in the title screen
    private void Start()
    {
        
       
        
        SaveFolder = Application.persistentDataPath + "/SavedInfo";
        if(!Directory.Exists(SaveFolder))
        {    
            //if it doesn't, create it
            Directory.CreateDirectory(SaveFolder);
        }

        SaveFolder += '/';
        SaveShip(0);
        
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

        string str = sr.ReadLine();
        int[] elems = { shipColA, shipColB, shipColC, shipColEmissive };
        int n = 0;
        while (str != null)
        {
            if (n < elems.Length)
            {
                int e = elems[n++];
                Color c = uint.Parse(str).ToColor();
                hiddenShipMaterial.SetColor(e, c);
                myShipMaterial.SetColor(e, c);
            }

            str = sr.ReadLine();
        }
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
    
    public void SaveShip(int i)
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

