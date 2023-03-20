using System;
using System.Collections.Generic;
using UnityEngine;

public class Section : MonoBehaviour
{
    private List<GameObject> parents = new();
    private List<ShipComponent> items = new();

    private RectTransform myRT;
    private void Awake()
    {
        myRT = GetComponent<RectTransform>();
        gameObject.SetActive(false);
    }

    public void AddElement(RectTransform parent, ShipComponent item)
    {
        parents.Add(parent.gameObject);
        items.Add(item);
        myRT.sizeDelta += parent.sizeDelta;
    }

    public void RemoveElement(int idx)
    {
        parents.RemoveAt(idx);
        items.RemoveAt(idx);
    }

    public void Validate()
    {
        for (int i = 0; i < items.Count; ++i)
        {
            ShipComponent item = items[i];
            /*
            if (item.isUnlocked)
            {
                //parents[i].SetActive(false);
                continue;
            }
            */
            if (item.Cost > PlayerUI.Balance)
            {
                continue;
            }
            
        }
    }

}
