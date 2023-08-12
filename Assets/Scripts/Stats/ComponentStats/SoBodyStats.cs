
using System.Globalization;
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

        protected override void Awake()
        {
            Displays = new string[7];
            DisplayWords = new string[7];
                
            Displays[0] = Weight.ToString(CultureInfo.InvariantCulture); // oh well just do it twice ig
            DisplayWords[0] = "Weight";
            Displays[1] = HealthCapacity.ToString(CultureInfo.InvariantCulture);
            DisplayWords[1] = "Health Capacity";
            Displays[2] = ShieldCapacity.ToString(CultureInfo.InvariantCulture);
            DisplayWords[2] = "Shield Capacity";
            Displays[3] = EnergyCapacity.ToString(CultureInfo.InvariantCulture);
            DisplayWords[3] = "Energy Capacity";
            Displays[4] = HealthRate.ToString(CultureInfo.InvariantCulture);
            DisplayWords[4] = "Health Rate";
            Displays[5] = ShieldRate.ToString(CultureInfo.InvariantCulture);
            DisplayWords[5] = "Shield Rate";
            Displays[6] = EnergyRate.ToString(CultureInfo.InvariantCulture);
            DisplayWords[6] = "Energy Rate";
        }
    }
    
    
}
