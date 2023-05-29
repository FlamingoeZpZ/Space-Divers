using System.Globalization;
using Stats.ComponentStats;
using UnityEngine;

public class ShipComponent : MonoBehaviour
{
    [field: SerializeField] private SoShipComponentStats stats;
    public string [] Displays { get; private set;}
    public SoShipComponentStats.MaterialCost[] ResourceCost => stats.MaterialCosts;
    
    public string ResourceCostCompiled { get; private set; }
    public string Description => stats.Description;
    public int CurrencyCost => stats.CurrencyCost;
    public string TypeStr { get; private set; }
    public string SizeStr { get; private set; }
    public int TypeInt { get; private set; }
    public int SizeInt { get; private set; }


    private void Awake()
    {
        ResourceCostCompiled = "TODO: Add emojis to resource cost...";
        TypeStr = stats.Type.ToString("G");
        SizeStr = stats.Size.ToString("G");
        TypeInt = (int)stats.Type;
        SizeInt = (int)stats.Size;
        //DisplayCost = Rich text TMP...
        
        InitializeSpeciality();
    }

    private void InitializeSpeciality()
    {
        Displays = new string[4];
        Displays[0] = stats.Weight.ToString(CultureInfo.InvariantCulture);
        switch (stats)
        {
            case SoWeaponStats weaponStats: // Light weapons && Heavy Weapons
                
                Displays[1] = weaponStats.projectile.Damage.ToString(CultureInfo.InvariantCulture);
                Displays[2] = weaponStats.timeBetweenShots.ToString(CultureInfo.InvariantCulture);
                Displays[3] = weaponStats.fireCost.ToString(CultureInfo.InvariantCulture);
                break;
            case SoBodyStats bodyStats: // Bodies exclusively
                Displays = new string[7];
                Displays[0] = stats.Weight.ToString(CultureInfo.InvariantCulture); // oh well just do it twice ig
                Displays[1] = bodyStats.HealthCapacity.ToString(CultureInfo.InvariantCulture);
                Displays[2] = bodyStats.ShieldCapacity.ToString(CultureInfo.InvariantCulture);
                Displays[3] = bodyStats.EnergyCapacity.ToString(CultureInfo.InvariantCulture);
                Displays[4] = bodyStats.HealthRate.ToString(CultureInfo.InvariantCulture);
                Displays[5] = bodyStats.ShieldRate.ToString(CultureInfo.InvariantCulture);
                Displays[6] = bodyStats.EnergyRate.ToString(CultureInfo.InvariantCulture);
                break;
            case SoWingStats wingStats: //Wings and Fins
                Displays[1] = wingStats.MaxSpeedMultiplier.ToString(CultureInfo.InvariantCulture);
                Displays[2] = wingStats.TurningSpeedMultiplier.ToString(CultureInfo.InvariantCulture);
                Displays[3] = wingStats.MaxHealthIncrease.ToString(CultureInfo.InvariantCulture);
                break;
            case SoGenericStats genericStats: //Generative and Defensive Items
                Displays[1] = genericStats.Rate.ToString(CultureInfo.InvariantCulture);
                Displays[2] = genericStats.Capacity.ToString(CultureInfo.InvariantCulture);
                Displays[3] = genericStats.GetCost();
                break;
            case SoThrusterStats thrusterStats: // Thrusters
                Displays[1] = thrusterStats.MaxSpeedIncrease.ToString(CultureInfo.InvariantCulture);
                Displays[2] = thrusterStats.TurningSpeedIncrease.ToString(CultureInfo.InvariantCulture);
                Displays[3] = thrusterStats.EnergyCostRate.ToString(CultureInfo.InvariantCulture);
                break;
        }
    }

}
