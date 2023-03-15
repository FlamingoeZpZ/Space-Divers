using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StoreItems : MonoBehaviour
{
    [SerializeField] private RectTransform storeItemPrefab;
    [SerializeField] private Section[] storeSections;
    [SerializeField] private ShipItem[] purchasableComponents;

    // Start is called before the first frame update
    void Start()
    {
        int l = purchasableComponents.Length;

        RectTransform viewport = GetComponent<RectTransform>();
        
        viewport.sizeDelta = new Vector2(storeItemPrefab.sizeDelta.x * l,viewport.sizeDelta.y);
        int[] elems = new int[storeSections.Length];
        Vector3 x = new Vector3(150, 0, -1000);
        foreach (ShipItem item in purchasableComponents)
        {

            int i = 0;
            int p = 1;
            int d = (int)item.InstanceComponent.GetPartType;
            while (p < d)
            {
                i++;
                p <<= 1;
            }
            
            print("ADDING ELEMENT TO: " + i);
            elems[i]++;

            RectTransform rt = Instantiate(storeItemPrefab, storeSections[i].GetComponent<Transform>()); // Comes instansiated with all parts.
            
            //Header Text
            rt.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = item.name;

            Instantiate(item.InstanceComponent, x, Quaternion.identity, rt.GetChild(1));

            Transform stats = rt.GetChild(2);
            stats.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = item.Cost.ToString();
            stats.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = item.Damage.ToString();
            stats.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = item.Health.ToString();
            stats.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = item.Shield.ToString();
            stats.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = item.Mobility.ToString();
            
            storeSections[i].AddElement(rt.gameObject, item);
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
