using UnityEngine;

namespace Stats
{
    [CreateAssetMenu(fileName = "WeaponStats", order = 1, menuName = "SpaceDivers/WeaponStats")]
    public class WeaponStats : ScriptableObject
    {
        [field: Header("Damage")] 
        [field: SerializeField] public float timeBetweenShots;
        [field: SerializeField] public bool isTargeting { get; private set; }
        [field: SerializeField] public bool isAutomatic  { get; private set; }
        [field: SerializeField] public float chargeDelay { get; private set; }

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