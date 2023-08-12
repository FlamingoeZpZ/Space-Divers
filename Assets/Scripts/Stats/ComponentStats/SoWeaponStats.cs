
using System.Globalization;
using UnityEngine;

namespace Stats.ComponentStats
{
    [CreateAssetMenu(fileName = "WeaponStats", order = 1, menuName = "SpaceDivers/ComponentStats/WeaponStats")]
    public class SoWeaponStats : SoShipComponentStats
    {
        [field: Header("Damage")] 
        [field: SerializeField] public ProjectileStats projectile  { get; private set; }
        [field: SerializeField] public float chargeDelay { get; private set; }
        [field: SerializeField] public float timeBetweenShots { get; private set; }

        [field: Header("Cost")]
        [field: SerializeField] public float fireCost { get; private set; }
        [field: SerializeField] public EAmmoType ammoType { get; private set; }


        protected override void Awake()
        {
            base.Awake();
            Displays[1] = projectile.Damage.ToString(CultureInfo.InvariantCulture);
            DisplayWords[1] = "Damage";
                
            DisplayWords[3] = ammoType.ToString("G");
                
            Displays[2] = timeBetweenShots.ToString(CultureInfo.InvariantCulture);
            DisplayWords[2] = string.Concat(DisplayWords[3], " rate");
                
            Displays[3] = fireCost.ToString(CultureInfo.InvariantCulture);
            DisplayWords[3] += " Cost";
        }
    }
    
    
    
    public enum EAmmoType
    {
        Bullet,
        Energy,
        Explosive
    }
}
