using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;
using Application = UnityEngine.Application;

[DefaultExecutionOrder(-20)]
public class Settings : MonoBehaviour
{
    public static Gradient verticalityGradient { get; private set; }
    public static Color enemyTargetingPlayer { get; set; }
    public static Color enemyLostPlayer { get; set; }

    [Header("Radar")] [SerializeField] private Material gradientMaterial;
    [SerializeField] private Material myShipMaterial;
    [SerializeField] private Material hiddenShipMaterial;

    [Header("Stickers")] public Material[] stickerMats;
    [SerializeField] private StickerComponent stickerPrefab;


    private static readonly int BelowColor = Shader.PropertyToID("_BelowColor");
    private static readonly int AboveColor = Shader.PropertyToID("_AboveColor");
    public static readonly int ShipColA = Shader.PropertyToID("_ColorA");
    private static readonly int ShipColB = Shader.PropertyToID("_ColorB");
    private static readonly int ShipColC = Shader.PropertyToID("_ColorC");
    private static readonly int ShipColEmissive = Shader.PropertyToID("_EmissiveColor");
    private static readonly int ShipColIntensity = Shader.PropertyToID("_Intensity");
    public static readonly int TextureID = Shader.PropertyToID("_Texture");

    private int shipID;
    private string SaveFolder;

    public static Settings instance { get; private set; }

    //public static Action OnSettingsSaved;

    //This should only be in the title screen
    private void Start()
    {
        if (instance != this && instance)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        string st = SaveFolder = Application.persistentDataPath + "/Ships";
        SaveFolder += '/';
        if (!Directory.Exists(st))
        {
            //if it doesn't, create it
            Directory.CreateDirectory(st);
            //This is pretty much game first load.
            SaveShip();
        }

        print("Target directory: " + SaveFolder);



        //LoadGameInfo();
        LoadSettings();
        LoadShip(0);

    }

    private void LoadSettings()
    {
        Color aboveY = ((uint)PlayerPrefs.GetInt("AboveCol", 65535)).ToColor();
        Color belowY = ((uint)PlayerPrefs.GetInt("BelowCol", -16776961)).ToColor();
        gradientMaterial.SetColor(BelowColor, belowY); // Default of red
        gradientMaterial.SetColor(AboveColor, aboveY); // Default of blue
        verticalityGradient = new()
        {
            colorKeys = new GradientColorKey[]
            {
                new(belowY, 0),
                new(aboveY, 1),
            }
        };
        enemyTargetingPlayer =
            ((uint)PlayerPrefs.GetInt("enemyTargetingPlayer", 2139029759)).ToColor(); // Default orange
        enemyLostPlayer = ((uint)PlayerPrefs.GetInt("enemyLostPlayer", -65281)).ToColor(); // default yellow
    }

    private void LoadShip(int i)
    {

        PlayerPrefs.SetInt("LastShipKey", i);
        StreamReader sr = new StreamReader(SaveFolder + "shipData" + i + ".dat");

        int[] elems = { ShipColA, ShipColB, ShipColC, ShipColEmissive };
        foreach (int t in elems)
        {
            Color c = uint.Parse(sr.ReadLine()).ToColor();
            hiddenShipMaterial.SetColor(t, c);
            myShipMaterial.SetColor(t, c);
        }

        float intensity = float.Parse(sr.ReadLine());
        myShipMaterial.SetFloat(ShipColIntensity, intensity);
        hiddenShipMaterial.SetFloat(ShipColIntensity, intensity);

        string s = sr.ReadLine();
        if (s == null) return;

        //Clear the Host Object
        Transform b = ModularPlayerScript.Instance.transform.GetChild(0);
        int childCount = b.childCount;
        for (int n = 0; n < childCount; ++n)
        {
            Destroy(b.GetChild(n).gameObject);
        }
        //Create ship before parenting

        Stack<Node> nodes = new();
        int height = 0;

        Transform prv = Instantiate(Resources.Load<Transform>(s.Split(',')[1]), b);
        prv.name = prv.name.Substring(0, prv.name.Length - 7);

        nodes.Push(new Node(prv));

        while ((s = sr.ReadLine()) != null && nodes.Count != 0)
        {
            string[] data = s.Split(',');

            int v = int.Parse(data[0]);

            if (data.Length == 13) // sticker
            {
                StickerComponent sticker = Instantiate(stickerPrefab, prv.GetChild(0));
                sticker.transform.localPosition =
                    new Vector3(float.Parse(data[1]), float.Parse(data[2]), float.Parse(data[3]));
                sticker.transform.rotation = new Quaternion(float.Parse(data[4]), float.Parse(data[5]),
                    float.Parse(data[6]), float.Parse(data[7]));
                int id = int.Parse(data[10]);
                sticker.SetID(id);
                stickerMats[id].SetTexture(TextureID, Resources.Load<Texture2D>("Stickers/" + data[8]));
                stickerMats[id].SetColor(ShipColA, uint.Parse(data[9]).ToColor());
                DecalProjector dp = sticker.GetComponent<DecalProjector>();
                dp.material = stickerMats[id];
                dp.size = new Vector3(float.Parse(data[11]), float.Parse(data[12]), 5);

                continue;
            }

            // If the current node's height is larger than the previous one. Then we add a new node to the stack
            if (height < v)
            {
                print("Pushing: " + v);

                nodes.Push(new Node(prv));
                height = v;
            }
            //If the height is taller than the last node, then we must pop.
            else if (height > v)
            {
                int dif = height - v;
                print("Popping: " + dif);
                while (dif-- != 0) // Correct if we are up 3 and going down to 1.
                {
                    nodes.Pop();
                }

                height = v;
            }

            //If we've marked it as empty, stay true to that.
            if (data[1] == "Empty")
            {
                nodes.Peek().SkipIncrement();
                continue;
            }
            
            //print("Trying to create: " + data[1] + " on: " + nodes.Peek().GetChild());
            
            prv = Instantiate(Resources.Load<Transform>(data[1]), nodes.Peek().GetChild());
            prv.name = prv.name.Substring(0, prv.name.Length - 7);
            
            print(prv.name);
            
        }

        sr.Close();
    }

    private class Node
    {
        private int index;
        private readonly Transform root;

        public Node(Transform root)
        {
            this.root = root;
            index = 0;
        }

        public Transform GetChild()
        {
            return root.GetChild(++index);
        }

        public void SkipIncrement()
        {
            index++;
        }

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


    public void SaveShip()
    {
        StreamWriter sw = new StreamWriter(SaveFolder + "shipData" + PlayerPrefs.GetInt("LastShipKey") + ".dat");
        sw.AutoFlush = true;

        //Then save colors
        sw.WriteLine(myShipMaterial.GetColor(ShipColA).ToRgba());
        sw.WriteLine(myShipMaterial.GetColor(ShipColB).ToRgba());
        sw.WriteLine(myShipMaterial.GetColor(ShipColC).ToRgba());
        sw.WriteLine(myShipMaterial.GetColor(ShipColEmissive).ToRgba());
        sw.WriteLine(myShipMaterial.GetFloat(ShipColIntensity));

        //Save the body
        SavePart( sw, ModularPlayerScript.Instance.transform.GetChild(0).GetChild(0), 0);

        sw.Close();
    }

    private void SavePart( StreamWriter sw, Transform t, int height)
    {
        
        if (t == null)
        {
            sw.WriteLine(height + ",Empty");
            return;
        }
        sw.WriteLine(height + "," + t.GetComponent<ShipComponent>().MyType + "/" +
                     t.name); //Store the name (for rebuilding)

        //Checking against collider
        for (int j = 0; j < t.GetChild(0).childCount; ++j)
        {
            Transform n = t.GetChild(0).GetChild(j);
            StickerComponent s = n.GetComponent<StickerComponent>();
            if(!s || !s.isSet) continue;
            DecalProjector dp = n.GetComponent<DecalProjector>();
            
            Vector3 position = n.localPosition;
            Quaternion rotation = n.rotation;
            sw.WriteLine(height + "," + position.x + "," + position.y + "," + position.z + "," +
                         rotation.x + "," + rotation.y + "," + rotation.z + "," + rotation.w + "," +
                         stickerMats[s.MyIdx].GetTexture(TextureID).name + "," + stickerMats[s.MyIdx].GetColor(ShipColA).ToRgba() +
                         "," + s.MyIdx+","+dp.size.x+","+dp.size.y);
        }
        

        //Go through each child, skipping the first which is a collider
        for (int j = 1; j < t.childCount; ++j)
        {
            Transform n = t.GetChild(j);
            //if(n.childCount == 0) //If the object has no children, then enqueue it.
            SavePart( sw, n.childCount == 0 ? null : n.GetChild(0), height + 1);
        }
        
    }
    
    


    public void SaveSettings()
    {
        PlayerPrefs.SetInt("BelowCol", (int)gradientMaterial.GetColor(BelowColor).ToRgba());
        PlayerPrefs.SetInt("AboveCol", (int)gradientMaterial.GetColor(AboveColor).ToRgba());
        PlayerPrefs.SetInt("enemyTargetingPlayer", (int)enemyTargetingPlayer.ToRgba());
        PlayerPrefs.SetInt("enemyLostPlayer", (int)enemyLostPlayer.ToRgba());
        PlayerPrefs.Save();
    }

    public void PurgeAndQuit()
    {
        Directory.Delete(SaveFolder, true);

        Application.Quit();
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
    
    public static bool IsPointerOverUIObject()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        return results.Count > 0;
    }
    
    
}

