
using UnityEngine;

namespace Stats.ComponentStats
{
    [CreateAssetMenu(fileName = "BodyStats", order = 1, menuName = "SpaceDivers/ComponentStats/BodyStats")]
    public class SoBodyStats : SoShipComponentStats
    {
        [field: Header("SpecificStats")]
        [field: SerializeField] public float HealthCapacity { get; private set; }
        [field: SerializeField] public float ShieldCapacity { get; private set; }
        [field: SerializeField] public float EnergyCapacity { get; private set; }
        [field: SerializeField] public float HealthRate { get; private set; }
        [field: SerializeField] public float ShieldRate { get; private set; }
        [field: SerializeField] public float EnergyRate { get; private set; }

    }
}
