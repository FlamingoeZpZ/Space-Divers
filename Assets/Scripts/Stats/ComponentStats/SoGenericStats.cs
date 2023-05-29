using System;
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
        public string GetCost() => Cost == 0 ? "" : Cost.ToString(CultureInfo.InvariantCulture);
        
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
