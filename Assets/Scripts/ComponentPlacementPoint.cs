using System;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class ComponentPlacementPoint : MonoBehaviour
{
    private SphereCollider col;
    
    [Flags]
    private enum PartType
    {
        Special=0,
        Weapon=1,
        HeavyWeapon=2,
        Wing=4,
        Thruster = 8,
        BackThruster = 8
    }
    
    [SerializeField] private PartType placeableTypes;

    private void Start()
    {
        col = GetComponent<SphereCollider>();
    }

    private void OnDrawGizmos()
    {
        int p = (int)placeableTypes;
        if ((p & 1) == 1 && p > 1)
        {
            Gizmos.color = Color.white;
        }
        else
        {
            switch (placeableTypes)
            {
                case PartType.Special:
                    Gizmos.color = Color.black;
                    break;
                case PartType.Weapon:
                    Gizmos.color = Color.yellow;
                    break;
                case PartType.HeavyWeapon:
                    Gizmos.color = Color.red;
                    break;
                case PartType.Wing:
                    Gizmos.color = Color.green;
                    break;
                case PartType.Thruster:
                    Gizmos.color = Color.blue;
                    break;
            }
        }

        //Gizmos.DrawSphere(transform.position, col.radius);
    }
    
}
