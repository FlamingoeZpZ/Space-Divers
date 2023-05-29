using System;
using System.Collections;
using System.Collections.Generic;
using Stats.ComponentStats;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoreItems : MonoBehaviour
{
    [SerializeField] private RectTransform storeItemPrefab;
    [SerializeField] private Section[] storeSections;
    [SerializeField] private ShipComponent[] purchasableComponents;
    [SerializeField] private Material mainMat;

    
    public static StoreItems Instance { get; private set; }
    

    private RectTransform viewport;
    void Start()
    {
        Instance = this;
        int l = purchasableComponents.Length;

        viewport = GetComponent<RectTransform>();
        int layer = LayerMask.NameToLayer("UI"); 
        viewport.sizeDelta = new Vector2(800, l * storeItemPrefab.sizeDelta.y + storeSections.Length * 200) ;
        int[] elems = new int[storeSections.Length];
        foreach (ShipComponent item in purchasableComponents)
        {

            int i = 0;
            int p = 1;
            int d = (int)item.TypeInt;
            while (p < d)
            {
                i++;
                p <<= 1;
            }
            
            elems[i]++;

            RectTransform rt = Instantiate(storeItemPrefab, storeSections[i].GetComponent<Transform>()); // Comes instansiated with all parts.
            
            
            //Header Text
            rt.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = item.name;

            GameObject g  = Instantiate(item, rt.GetChild(1).GetChild(1)).gameObject;
            Transform t = g.transform;
            rt.GetComponent<Button>().onClick.AddListener(() => TogglePlacementPonits.PlaceNode(g));
            t.localScale *= 50;
            g.layer = layer;
            t.GetComponent<MeshRenderer>().material = mainMat;
            Transform stats = rt.GetChild(2);
            
            for (int m = 0; i < 4; ++i)
            {
                stats.GetChild(m).GetChild(0).GetComponent<TextMeshProUGUI>().text = item.Displays[m];
            }
            //TODO implement
            
            
            
            stats.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = item.ResourceCostCompiled;
            stats.GetChild(5).GetChild(0).GetComponent<TextMeshProUGUI>().text = item.CurrencyCost.ToString();

            
            storeSections[i].AddElement(rt, item);
        }
        
        //Epic
         transform.parent.parent.parent.parent.parent.gameObject.SetActive(false);
    }

    public void ValidateShop(PartType type)
    {
        int d = (int)type;
        float len = 0;
        int p = 1;
        foreach (Section t in storeSections)
        {
            if ((d & p) == 0)
            {
                t.gameObject.SetActive(false);
                p <<= 1;
                continue;
            }

            p <<= 1;
            t.gameObject.SetActive(true);
            len += 200 + t.length;
        }
        viewport.sizeDelta = new Vector2(800, len) ;
    }
}
