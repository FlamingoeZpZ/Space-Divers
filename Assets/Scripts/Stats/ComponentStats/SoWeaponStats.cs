
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
    }
    
    public enum EAmmoType
    {
        Bullet,
        Energy,
        Rocket
    }
}
