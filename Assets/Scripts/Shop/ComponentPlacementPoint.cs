using System;
using Stats.ComponentStats;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class ComponentPlacementPoint : MonoBehaviour
{ 
    [field: SerializeField] private PartSize placableSize;
    private MeshRenderer mr;
    private Collider c;
    private int id;

    private void Awake()
    {
        mr = GetComponent<MeshRenderer>();
        c = GetComponent<Collider>();
    }
    private void Start()
    {
        if (transform.root.gameObject.layer != LayerMask.NameToLayer("PlayerRoot"))
        {
            enabled = false;
            return;
        }

        enabled = true;
        id = StoreItems.Instance.BindPlacementPoint(this);
    }

    private void OnDestroy()
    {
        StoreItems.Instance.RemovePlacementPoint(id);
    }

    //Required for when placing point.
    public void ForceActivate()
    {
        enabled = true;
        id = StoreItems.Instance.BindPlacementPoint(this);
    }

    public void ToggleDisplay(bool t, bool forced = false)
    {
        bool n = transform.childCount == 0 || forced;
        c.enabled = t && n;
        mr.enabled = t  && n;
    }

    public void SetMat(Material m)
    {
        mr.material = m;
    }

    public void Display(int comp)
    {
        if(comp == 0)
            ToggleDisplay(false);
        ToggleDisplay(comp <= (int)placableSize);
    }


}
