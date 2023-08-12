using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Section : MonoBehaviour
{
    private readonly List<GameObject> parents = new();
    private readonly List<ShipComponent> items = new();

    private RectTransform myRT;
    private RectTransform parentRT;
    public float length { get; private set; }

    private readonly Color hiddenColor = Color.grey;
    private readonly Color showingColor = Color.white;
    private GameObject isHiddenArrow;
    private GameObject isVisableArrow;
    private Image img;
    private bool isShowing;
    private GameObject itemHolder;
    private Vector2 LenComp;
    private void Awake()
    {
        myRT = GetComponent<RectTransform>();
        isHiddenArrow = transform.GetChild(0).GetChild(1).gameObject;
        isVisableArrow = transform.GetChild(0).GetChild(2).gameObject;
        img = transform.GetChild(0).GetComponent<Image>();
        parentRT = transform.parent.GetComponent<RectTransform>();
        itemHolder = transform.GetChild(1).gameObject;
        isShowing = false;
        ToggleVisibility(isShowing);
    }

    public void AddElement(RectTransform parent, ShipComponent item)
    {
        
        parents.Add(parent.gameObject);
        items.Add(item);
        if ((parents.Count & 1) == 1)
        {
            length += 290;
            //myRT.sizeDelta = new Vector2(0, length);
            LenComp = new Vector2(0, length);
        }


    }

    public void RemoveElement(int idx)
    {
        parents.RemoveAt(idx);
        items.RemoveAt(idx);
    }

    private void OnEnable()
    {
        print("ImplementCostHiding");
        for (int i = 0; i < items.Count; ++i)
        {
            ShipComponent item = items[i];
            if (item.CurrencyCost > PlayerUI.Balance)
            {
                continue;
            }
        }
    }

    public void ToggleVisibility()
    {
        isShowing = !isShowing;
        ToggleVisibility(isShowing);
    }

    public void ToggleVisibility(bool state)
    {
        if (state)
        {
            img.color = showingColor;
            isHiddenArrow.SetActive(false); 
            isVisableArrow.SetActive(true); 
            itemHolder.SetActive(true);
            myRT.sizeDelta += LenComp;
            parentRT.sizeDelta += LenComp;
        }
        else
        {
            img.color = hiddenColor;
            isHiddenArrow.SetActive(true); 
            isVisableArrow.SetActive(false); 
            itemHolder.SetActive(false);
            myRT.sizeDelta -= LenComp;
            parentRT.sizeDelta -= LenComp;
        }
    }
}
