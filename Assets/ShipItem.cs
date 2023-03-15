using UnityEngine;

public class ShipItem : ScriptableObject
{
    [field: SerializeField] public ShipComponent InstanceComponent { get; private set; }
    [field: SerializeField] public int Cost { get; private set; }
    [field: SerializeField] public int Damage { get; private set; }
    [field: SerializeField] public int Health { get; private set; }
    [field: SerializeField] public int Shield { get; private set; }
    [field: SerializeField] public int Mobility { get; private set; }
}
