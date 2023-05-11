using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponStats", order = 1, menuName = "SpaceDivers/WeaponStats")]
public class WeaponStats : ScriptableObject
{
    [field: Header("Damage")] 
    [field: SerializeField] public float timeBetweenShots;
    [field: SerializeField] public bool isTargeting { get; private set; }
    [field: SerializeField] public bool isAutomatic { get; private set; }
    
}
