using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileStats", order = 1, menuName = "SpaceDivers/ProjectileStats")]
public class ProjectileStats : ScriptableObject
{
    [field: SerializeField] public Color Color { get; private set; }

    [field: Header("Damage")]
    [field: SerializeField] public float Damage { get; private set; }
    [field: SerializeField] public float CriticalMultiplier { get; private set; }
    [field: SerializeField, Range(0,1)] public float CriticalChance { get; private set; }
    
    [field: Tooltip("If something is explosive it will play an effect and destroy some objects")]
    [field: SerializeField, Min(0.1f)] public float ExplosiveRadius { get; private set; }
    [field: Tooltip("If explosive, and harvestable, creates objects that can be collected... if this is too advanced, don't bother")]
    [field: SerializeField] public bool IsHarvestable { get; private set; }

    [field: SerializeField] public bool ExplodeOnDeath { get; private set; }

    //Targeting
    [field: Header("Targeting")]
    [field: SerializeField] public float Speed { get; private set; }
    [field: SerializeField] public float LifeTime { get; private set; }
    [field: SerializeField, Min(0)] public float TurningRate { get; private set; }
    [field: SerializeField] public bool IsHoming { get; private set; }
    [field: SerializeField] private AnimationCurve acceleration;
    public float Acceleration(float life) => acceleration.Evaluate(life / LifeTime);



}
