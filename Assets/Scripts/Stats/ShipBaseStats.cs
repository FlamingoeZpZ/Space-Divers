using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShipBaseStats", order = 1, menuName = "SpaceDivers/ShipBaseStats")]
public class ShipBaseStats : ScriptableObject
{
    [field: Header("Info")]
    [field: SerializeField] public RectTransform blip { get; private set; }
    

    [field: Header("Handling")]
    [field: SerializeField] public float maxSpeed { get; private set; }
    [field: SerializeField] public float baseHandling { get; private set; }
    [field: SerializeField] public float maxRollAngle { get; private set; }
    [field: SerializeField] public float rollSpeed { get; private set; }
    [field: SerializeField] public float rollDecay { get; private set; }
    [field: SerializeField] public float timeBetweenDodges { get; private set; }
    
    [field: Header("Core Stats")]
    [field: SerializeField] public float maxHealth { get; private set; }
    [field: SerializeField, Tooltip("Laser resistance")] public float maxArmor { get; private set; }
    [field: SerializeField, Tooltip("Explosive Resistance")] public float maxShield { get; private set; }
    
    [field:Header("Ammo")]
    [field:SerializeField] public float maxBullets { get; private set; }
    [field:SerializeField] public float maxEnergy { get; private set; }
    [field:SerializeField] public float energyRechargeRate{ get; private set; }
    [field:SerializeField] public float maxRockets{ get; private set; }
   

    
}
