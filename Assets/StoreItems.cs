using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreItems : MonoBehaviour
{
    [SerializeField] private RectTransform storeItemPrefab;
    [SerializeField] private RectTransform viewport;
    [SerializeField] private ShipComponent[] purchasableComponents;

    private ShipComponent[] _storeItems;
    
    
    // Start is called before the first frame update
    void Start()
    {
        int l = purchasableComponents.Length;
        _storeItems = new ShipComponent[l];

        viewport.sizeDelta = new Vector2(storeItemPrefab.sizeDelta.x * l,viewport.sizeDelta.y);
        
        for (int i = 0; i < l; ++i)
        {
            RectTransform host = Instantiate(storeItemPrefab, viewport);
            Transform parent = host.GetChild(1);
            
            ShipComponent comp = Instantiate(purchasableComponents[i], parent);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
