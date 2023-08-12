using System.Globalization;
using UnityEngine;

namespace Stats.ComponentStats
{
    [CreateAssetMenu(fileName = "GenericStats", order = 1, menuName = "SpaceDivers/ComponentStats/GenericStats")]
    public  class SoGenericStats : SoShipComponentStats
    {
        [field: Header("SpecificStats")]
        [field: SerializeField] public float Rate  { get; private set; }
        [field: SerializeField] public ERateType RateType  { get; private set; }
        [field: SerializeField] public float Capacity  { get; private set; }
        
        [field: SerializeField] public ERateType CapacityType  { get; private set; }
        [field: SerializeField] public float Cost  { get; private set; }
        [field: SerializeField] public ERateType CostType  { get; private set; }
        protected override void Awake()
        {
            base.Awake();
            Displays[1] = Rate.ToString(CultureInfo.InvariantCulture);
            DisplayWords[1] = string.Concat(RateType.ToString("G"), " Rate");
            Displays[2] = Capacity.ToString(CultureInfo.InvariantCulture);
            DisplayWords[2] = string.Concat(CapacityType.ToString("G"), " Capacity");
            Displays[3] =  Cost == 0 ? "" : Cost.ToString(CultureInfo.InvariantCulture);
            DisplayWords[3] = string.Concat(CostType.ToString("G"), " Cost");
        }
    }

    public enum ERateType
    {
        Ore,
        Energy,
        Health,
        Rocket,
        Bullet,
        Shield
    }

}
