using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class ComponentPlacementPoint : MonoBehaviour
{
    [field: SerializeField] public PartType PlaceableTypes { get; private set; }
    private MeshRenderer _mr;
   

    private void Awake()
    {
        _mr = GetComponent<MeshRenderer>();
        
    }


    public void ToggleDisplay(bool t)
    {
        _mr.enabled = t;
    }

    public void SetMat(Material m)
    {
        _mr.material = m;
    }

}
