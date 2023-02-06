using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    
   [SerializeField] private Button settingsButton;


   [Header("Radar")] 
   [SerializeField] private RectTransform radar;

   public static RectTransform Radar;
   public static bool VerticalityDetection;
   public static float RadarDist;
   
   [SerializeField] private RectTransform enemyBlip;
   [SerializeField] private float radarDist;
   [SerializeField] private bool verticalityDetection;

   [Header("Targeting")] 
   [SerializeField] private Image crossHairCenter;
   [SerializeField] private Color targetFound;
   [SerializeField] private Color targetLost;
   [SerializeField] private Color defaultColor;
   [SerializeField] private float blinkDelay = 0.2f;
   [SerializeField] private int blinkTimes = 4;
   private Transform target;
   private Transform crossHairTrans;
   
   [Header("Health")]
   [SerializeField] private RectTransform healthRoot;
   [SerializeField] private Color healthColor;
   [SerializeField] private Slider healthLeft;
   [SerializeField] private Slider healthRight;

   private Transform parent;
   
    // Start is called before the first frame update
    void Awake()
    {
        Radar = radar;
        VerticalityDetection = verticalityDetection;
        RadarDist = radarDist;
        cam = Camera.main;
        Vector2 w = healthRoot.rect.size;
        w.x /= 2;
        RectTransform left = healthLeft.GetComponent<RectTransform>();
        left.sizeDelta = w;
        RectTransform right = healthRight.GetComponent<RectTransform>();
        right.sizeDelta = w;
        healthLeft.transform.GetChild(1).GetChild(0).GetComponent<Image>().color = healthColor;
        healthRight.transform.GetChild(1).GetChild(0).GetComponent<Image>().color = healthColor;
        crossHairTrans = crossHairCenter.transform;

        parent = transform.parent;

    }

    private Vector3 old;
    // Update is called once per frame
    void LateUpdate()
    {
        //First get distance.
        Vector3 parentPos = parent.position;
        Vector3 parentForward = parent.forward;
        
        
        foreach (Blip e in Enemy.Blips.Values)
        {
            e.UpdatePosition(parentPos);
        }
        
        
        
        Vector3 v = parentPos + parentForward * 50;
        if ((v - old).sqrMagnitude > 0)
        {
            old = v;
            crossHairTrans.parent.position = cam.WorldToScreenPoint(v);
        }

        if (!target) return;
        crossHairTrans.position = (Vector2)cam.WorldToScreenPoint(target.position);
    }

    public void UpdateHealth(float currentHealth)
    {
        healthLeft.value = currentHealth;
        healthRight.value = currentHealth;
    }

    private Coroutine blinking;
    private Camera cam;

    public void SetTarget(Transform prvTarget)
    {
        target = prvTarget;
            
        if (blinking != null)
            StopCoroutine(blinking);
        if (prvTarget)
        {
            blinking = StartCoroutine(HandleTargetBlinking(targetFound));
            return;
        }

        crossHairTrans.localPosition = Vector3.zero;
        blinking = StartCoroutine(HandleTargetBlinking(targetLost));
    }

    private IEnumerator HandleTargetBlinking(Color col)
    {
        int i = 0;
        while (i++ < blinkTimes)
        {
            crossHairCenter.color = col;
            yield return new WaitForSeconds(blinkDelay);
            crossHairCenter.color = defaultColor;
            yield return new WaitForSeconds(blinkDelay);
        }
    }
}

public readonly struct Blip
{
    private readonly Transform coreComponent;
    private readonly RectTransform blipComponent;
    public Blip(Transform core, RectTransform blip)
    {
        coreComponent = core;
        blipComponent = Object.Instantiate(blip, PlayerUI.Radar);
        Debug.Log("I've attached a blip to: " + PlayerUI.Radar.gameObject.name);
    }
    public void DestroyBlip()
    {
        Debug.Log("Destroying Blip");
        Object.Destroy(blipComponent.gameObject);
        Debug.Log(Enemy.Blips.Count);
    }
    
    public void UpdatePosition(Vector3 start)
    {
        Vector3 dif = coreComponent.position - start;

        if (dif.sqrMagnitude < PlayerUI.RadarDist)
        {
             
        }
    }
}
