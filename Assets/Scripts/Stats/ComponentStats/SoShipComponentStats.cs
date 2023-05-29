using System;
using UnityEngine;

namespace Stats.ComponentStats
{
    public abstract class SoShipComponentStats : ScriptableObject
    {
        [field: Header("Organization")]
        [field: SerializeField] public PartType Type { get; private set; } // You're my type
        [field: SerializeField] public PartSize Size { get; private set; } 
    
        [field: Header("Core Stats")]
        [field: SerializeField, TextArea] public string Description { get; private set; }
        [field: SerializeField] public float Weight { get; private set; }
        [field: SerializeField, Min(0)] public int CurrencyCost { get; private set; }
        [field: SerializeField] public MaterialCost [] MaterialCosts { get; private set; }
        
        
        [Serializable]
        public struct MaterialCost
        {
            [field: SerializeField] public EOreType Type { get; private set; }
            [field: SerializeField] public float Amount { get; private set; }
        }
    }
    
    public enum PartType
    {
        Body,
        Defense,
        Fin,
        Generator,
        HeavyWeapon,
        LightWeapon,
        Thruster,
        Wing
    }

    public enum PartSize
    {
        NotPlaceable,
        Small,
        Medium,
        Large,
        VeryLarge
    }
}
