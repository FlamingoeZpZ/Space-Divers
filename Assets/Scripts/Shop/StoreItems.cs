using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoreItems : MonoBehaviour
{
    [SerializeField] private RectTransform storeItemPrefab;
    private Section[] storeSections;
    [SerializeField] private ShipComponent[] purchasableComponents;
    [SerializeField] private Material mainMat;
    [SerializeField] private InfoAndHandling handler;
    [SerializeField] private TogglePlacementPonits placementPoints;
    
    private readonly Dictionary<int, ComponentPlacementPoint> points = new();

    public static StoreItems Instance { get; private set; }
    
    private void Awake()
    {
        Instance = this;

    }

    void Start()
    {
        //I don't trust get component in children to not search unnecessary items...
        storeSections = new Section[transform.childCount];
        for(int i = 0; i < transform.childCount; ++ i)
            storeSections[i] = transform.GetChild(i).GetComponent<Section>();
        
        
        int l = purchasableComponents.Length;
        int layer = LayerMask.NameToLayer("UI"); 
        print($"Setting size: {l} + {storeSections.Length * 60}");
        int[] elems = new int[storeSections.Length];
        foreach (ShipComponent item in purchasableComponents)
        {
            int i = (int)item.PartType;
            //print("Adding to: " + item.name + ",  " + item.TypeStr);
            
            elems[i]++;

            RectTransform rt = Instantiate(storeItemPrefab, storeSections[i].GetComponent<Transform>().GetChild(1)); // Comes instansiated with all parts.
            
            
            //Header Text
            rt.GetChild(0).GetChild(3).GetComponent<TextMeshProUGUI>().text = item.name;
            
            GameObject g  = Instantiate(item, rt.GetChild(0).GetChild(1)).gameObject;
            
            Transform t = g.transform;
            //rt.GetComponent<Button>().onClick.AddListener(() => TogglePlacementPonits.PlaceNode(g));
            t.localScale *= 50;
            MeshRenderer[] r = t.GetComponentsInChildren<MeshRenderer>();
            g.layer = layer;
            foreach (MeshRenderer n in r)
            {
                if(n.gameObject.layer == LayerMask.NameToLayer(("Player"))) 
                    n.material = mainMat;
                n.gameObject.layer = layer;
               
            }
            rt.GetChild(0).GetChild(2).GetComponent<Button>().onClick.AddListener(() =>
            {
                print("activated");
                int n = (int)item.PartSize;
                foreach (ComponentPlacementPoint p in points.Values)
                {
                    p.Display(n);
                }
                handler.gameObject.SetActive(true);
                handler.SetItem(g, item, false);
                placementPoints.SetCursorObject(g);
            });
            storeSections[i].AddElement(rt, item);
        }
    }


    private static int id = 0;
    public int BindPlacementPoint(ComponentPlacementPoint componentPlacementPoint)
    {
        points.Add(id, componentPlacementPoint);
        return id++;
    }

    public void RemovePlacementPoint(int id)
    {
        points.Remove(id);
    }

    public void Close()
    {
        handler.gameObject.SetActive(false);
        foreach (ComponentPlacementPoint p in points.Values)
        {
            p.Display(0);
        }
    }
}
