using System;
using System.Globalization;
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
        
        public string[] DisplayWords { get; protected set; }
        public string[] Displays { get; protected set; }

        protected virtual void Awake()
        {
            Debug.Log("Awaken");
            Displays = new string[4];
            DisplayWords  = new string[4];
            Displays[0] = Weight.ToString(CultureInfo.InvariantCulture);
            DisplayWords[0] = "Weight";
        }


        [Serializable]
        public struct MaterialCost
        {
            [field: SerializeField] public EOreType Type { get; private set; }
            [field: SerializeField] public float Amount { get; private set; }
        }
    }
    
    public enum PartType
    {
        Body=-1,
        Generator,
        Defense,
        LightWeapon,
        HeavyWeapon,
        Thruster,
        Fin,
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
