
using System.Globalization;
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


        protected override void Awake()
        {
            base.Awake();
            Displays[1] = MaxSpeedMultiplier.ToString(CultureInfo.InvariantCulture);
            DisplayWords[1] = "Speed Multiplier";
            Displays[2] = TurningSpeedMultiplier.ToString(CultureInfo.InvariantCulture);
            DisplayWords[2] = "Turning Multiplier";
            Displays[3] = MaxHealthIncrease.ToString(CultureInfo.InvariantCulture);
            DisplayWords[3] = "Health Capacity";
        }
    }
}
