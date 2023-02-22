using UnityEngine;

[DefaultExecutionOrder(-20)]
public class Settings : MonoBehaviour
{
    public static Gradient verticalityGradient { get; private set; }
    public static Color enemyTargetingPlayer { get; private set; }
    public static Color enemyLostPlayer { get; private set; }
    
    
    
    [Header("Radar")]
    [SerializeField] private Color belowY;
    [SerializeField] private Color aboveY;
    [SerializeField] private Color targetFoundColor;
    [SerializeField] private Color targetLostColor;
    [SerializeField] private Material gradientMaterial;
    
    private readonly int belowColor = Shader.PropertyToID("_BelowColor");
    private readonly int aboveColor = Shader.PropertyToID("_AboveColor");

    //public static Action OnSettingsSaved;
    
    private void Awake()
    {
        enemyTargetingPlayer = targetFoundColor;
        enemyLostPlayer = targetLostColor;
        verticalityGradient = new ()
        {
            colorKeys = new GradientColorKey[]
            {
                new (belowY, 0),
                new (aboveY, 1),
            }
        };
        gradientMaterial.SetColor(belowColor,belowY);
        gradientMaterial.SetColor(aboveColor,aboveY);
    }

    private void ApplySettings()
    {
        //OnSettingsSaved?.Invoke();
        PlayerPrefs.Save();
    }
}
