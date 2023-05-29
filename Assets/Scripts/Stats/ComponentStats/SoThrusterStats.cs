
using UnityEngine;

namespace Stats.ComponentStats
{
    [CreateAssetMenu(fileName = "ThrusterStats", order = 1, menuName = "SpaceDivers/ComponentStats/ThrusterStats")]
    public class SoThrusterStats : SoShipComponentStats
    {
        [field: Header("SpecificStats")]
        [field: SerializeField] public float MaxSpeedIncrease { get; private set; }
        [field: SerializeField] public float TurningSpeedIncrease { get; private set; }
        [field: SerializeField] public float EnergyCostRate { get; private set; }
    }
}
