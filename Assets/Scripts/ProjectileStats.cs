using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileStats", order = 1, menuName = "SpaceDivers/ProjectileStats")]
public class ProjectileStats : ScriptableObject
{
    [field: Header("Damage")]
    [field: SerializeField] public float damage { get; private set; }
    [field: SerializeField] public float criticalMultiplier { get; private set; }
    [field: SerializeField, Range(0,1)] public float criticalChance { get; private set; }
    
    [field: Tooltip("If something is explosive it will play an effect and destroy some objects")]
    [field: SerializeField] public bool isExplosive { get; private set; }
    [field: Tooltip("If explosive, and harvestable, creates objects that can be collected... if this is too advanced, don't bother")]
    [field: SerializeField] public bool isHarvestable { get; private set; }


    //Targeting
    [field: Header("Targeting")]
    [field: SerializeField] public float speed { get; private set; }
    [field: SerializeField] public float lifeTime { get; private set; }
    [field: SerializeField] public bool isHoming { get; private set; }
    
}
