using System;
using Stats.ComponentStats;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class ComponentPlacementPoint : MonoBehaviour
{
    [field: SerializeField] public PartType PlaceableTypes { get; private set; }
    private MeshRenderer _mr;
    private Collider c;   

    private void Awake()
    {
        _mr = GetComponent<MeshRenderer>();
        c = GetComponent<Collider>();
    }

    public void ToggleDisplay(bool t)
    {
        c.enabled = t;
        _mr.enabled = t;
    }

    public void SetMat(Material m)
    {
        _mr.material = m;
    }

}
