
using System;
using UnityEngine;

namespace Stats
{
    [CreateAssetMenu(fileName = "OreStats", order = 1, menuName = "SpaceDivers/OreStats")]
    public class OreStats : ScriptableObject
    {
        [field: SerializeField, Min(0)] public Amounts [] Ores { get; private set; }
        [field: SerializeField, Min(0)] public float Health { get; private set; }

        [Serializable]
        public struct Amounts
        {
            [field: SerializeField, Min(0)] public EOreType OreType { get; private set; }
            [field: SerializeField, Min(0)] public int MinOre { get; private set; }
            [field: SerializeField, Min(0)] public int MaxOre { get; private set; }
        }

    }

    public enum EOreType
    {
        Iron,
        Tungsten,
        Titanium,
        Brillion
    }
    

}
