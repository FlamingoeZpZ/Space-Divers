using System.Collections.Generic;
using UnityEngine;

public class Section : MonoBehaviour
{
    private List<GameObject> parents;
    private List<ShipItem> items;

    public void AddElement(GameObject parent, ShipItem item)
    {
        parents.Add(parent);
        items.Add(item);
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
            ShipItem item = items[i];
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
