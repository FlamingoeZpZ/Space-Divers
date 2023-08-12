
using System.Globalization;
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

        protected override void Awake()
        {
            base.Awake();
            Displays[1] = MaxSpeedIncrease.ToString(CultureInfo.InvariantCulture);               
            DisplayWords[1] = "Speed Increase";
            Displays[2] = TurningSpeedIncrease.ToString(CultureInfo.InvariantCulture);
            DisplayWords[2] = "Turning Increase";
            Displays[3] = EnergyCostRate.ToString(CultureInfo.InvariantCulture);
            DisplayWords[3] = "Energy Rate";
        }
    }
}
