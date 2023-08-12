using System.Globalization;
using Stats.ComponentStats;
using UnityEngine;

public class ShipComponent : MonoBehaviour
{
    [field: SerializeField] private SoShipComponentStats stats;
    
    public SoShipComponentStats.MaterialCost[] ResourceCost => stats.MaterialCosts;

    public string ResourceCostCompiled { get; private set; }
    public string Description => stats.Description;
    public int CurrencyCost => stats.CurrencyCost;

    public string[] Displays => stats.Displays;
    public string[] DisplayWords => stats.DisplayWords;
    
    

    public PartType PartType => stats.Type;
    public PartSize PartSize => stats.Size;

    private void OnDestroy()
    {
        if (transform.parent.TryGetComponent(out ComponentPlacementPoint p))
        {
            p.ToggleDisplay(true, true);
        }
    }
}