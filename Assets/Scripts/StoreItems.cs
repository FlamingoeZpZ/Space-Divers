using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StoreItems : MonoBehaviour
{
    [SerializeField] private RectTransform storeItemPrefab;
    [SerializeField] private Section[] storeSections;
    [SerializeField] private ShipComponent[] purchasableComponents;
    [SerializeField] private Material overlay;
    [SerializeField] private Material mainMat;

    // Start is called before the first frame update
    void Start()
    {
        int l = purchasableComponents.Length;

        RectTransform viewport = GetComponent<RectTransform>();
        int layer = LayerMask.NameToLayer("UI"); 
        viewport.sizeDelta = new Vector2(800, l * storeItemPrefab.sizeDelta.y + storeSections.Length * 200) ;
        int[] elems = new int[storeSections.Length];
        foreach (ShipComponent item in purchasableComponents)
        {

            int i = 0;
            int p = 1;
            int d = (int)item.MyType;
            while (p < d)
            {
                i++;
                p <<= 1;
                print(p + " Comp " + d);
            }
            
            print("ADDING ELEMENT TO: " + i + " : " + d);
            elems[i]++;

            RectTransform rt = Instantiate(storeItemPrefab, storeSections[i].GetComponent<Transform>()); // Comes instansiated with all parts.
            
            //Header Text
            rt.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = item.name;

            Transform t  = Instantiate(item, rt.GetChild(1).GetChild(0)).transform;
            t.localScale *= 500;
            t.gameObject.layer = layer;
            t.GetComponent<MeshRenderer>().materials = new []{mainMat, overlay};
            Transform stats = rt.GetChild(2);
            stats.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = item.Cost.ToString();
            stats.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = item.Health.ToString();
            stats.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = item.Shield.ToString();
            stats.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = item.Speed.ToString();
            stats.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = item.Turning.ToString();
            stats.GetChild(5).GetChild(0).GetComponent<TextMeshProUGUI>().text = item.Damage.ToString();
            
            storeSections[i].AddElement(rt, item);
        }
    }

    public void ValidateShop(PartType type)
    {
        int p = 1;
        int d = (int)type;
        foreach (Section section in storeSections)
        {
            if ((p & d) != 0)
            {
                section.gameObject.SetActive(true);
                section.Validate();
                
            }
            else
            {
                section.gameObject.SetActive(false);
            }
            p <<= 1;
        }
    }
}
