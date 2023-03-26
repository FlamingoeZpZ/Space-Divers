using System;
using UnityEngine;

public class ShipComponent : MonoBehaviour
{
    [field: SerializeField] public PartType MyType { get; private set; } // You're my type
    
    //Advantage to having it here: less clutter, easier to code, makes it so individual parts have an easier time remembering shit
    //Disadvantage: SOs mean that we can conform all their stats
    [field: SerializeField] public int Cost { get; private set; }
    [field: SerializeField] public int Damage { get; private set; }
    [field: SerializeField] public int Health { get; private set; }
    [field: SerializeField] public int Shield { get; private set; }
    [field: SerializeField] public int Speed { get; private set; }
    [field: SerializeField] public int Turning { get; private set; }

    private void Awake()
    {
        print("Added");
    }
}
[Flags, Serializable]
public enum PartType
{
    Special=1,
    Weapon=2,
    HeavyWeapon=4,
    Wing=8,
    ShortWing=16,
    Thruster = 32,
    Bodies = 64,
}