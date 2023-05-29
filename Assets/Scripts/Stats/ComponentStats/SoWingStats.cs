
using UnityEngine;

namespace Stats.ComponentStats
{
    [CreateAssetMenu(fileName = "WingStats", order = 1, menuName = "SpaceDivers/ComponentStats/WingStats")]
    public class SoWingStats : SoShipComponentStats
    {
        [field: Header("SpecificStats")]
        [field: SerializeField] public float MaxSpeedMultiplier { get; private set; }
        [field: SerializeField] public float TurningSpeedMultiplier { get; private set; }
        [field: SerializeField] public float MaxHealthIncrease { get; private set; }
    }
}
