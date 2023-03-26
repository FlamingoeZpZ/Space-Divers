using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class PlayerUI : MonoBehaviour
{
    
   [SerializeField] private Button settingsButton;

   [Header("Radar")] 
   [SerializeField] private RectTransform enemyBlipParent;

   public static RectTransform EnemyBlipParent { get; private set; }
   public static bool VerticalityDetection;
   public static float RadarDist;
   
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

   [Header("For Marks")] [SerializeField] private Button LUTButton;
   
   public static int Balance;
   private Transform parent;
   private static bool fromWarp;
   
    // Start is called before the first frame update
    void Awake()
    {
        EnemyBlipParent = enemyBlipParent;
        VerticalityDetection = verticalityDetection;
        RadarDist = radarDist;
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

        LUTButton.onClick.AddListener(()=>
        { 
            if(DEBUGLutManager.Instance) DEBUGLutManager.Instance.RotateLut();
        });

        SceneManager.sceneUnloaded += (scene) =>
        {
            if (scene.buildIndex == 0 || !fromWarp)
            {
                fromWarp = false;
                Shader.SetGlobalVector(movementID, new Vector4(0,0,0,0));
                return;
            }
            
           StartCoroutine(Warp(-1));
        };
    }



    private Vector3 old;

    private Vector3 parentForward;
    // Update is called once per frame

    
    private int warpTo ;
    [Header("Warping")]
    [SerializeField] private GameObject warpButton;
    [SerializeField] private TextMeshProUGUI warpText;
    [SerializeField] private AnimationCurve warpCurve;
    [SerializeField] private float warpDur = 0.8f;
    private readonly int movementID = Shader.PropertyToID("_Motion");
    void LateUpdate()
    {
        //First get distance.
        Vector3 parentPos = parent.position;
        parentForward = parent.forward;
        
        enemyBlipParent.eulerAngles = new Vector3(0,0,parent.eulerAngles.y);
        
        foreach (Blip e in Enemy.Blips.Values)
        {
            e.UpdatePosition(parentPos);
        }

        Vector3 v = parentPos + parentForward * 50;
        if ((v - old).sqrMagnitude > 0)
        {
            old = v;
            crossHairTrans.parent.position = Camera.main.WorldToScreenPoint(v);
        }
        foreach (KeyValuePair<int, Vector3> planet in PlanetSystem.PlanetDirs)
        {
            if(warpTo != 0 && planet.Key != warpTo) continue;
            if (Vector3.Dot(parentForward, planet.Value) + 0.01f > 1)
            {

                warpTo = planet.Key;
                warpButton.SetActive(true);
                String s = SceneUtility.GetScenePathByBuildIndex(warpTo);
                warpText.text = s.Substring(14, s.LastIndexOf('.')-14);
            }
            else 
            {
                warpTo = 0;
                warpButton.SetActive(false);
            }
        }
        if (!target) return;
        crossHairTrans.position = (Vector2)Camera.main.WorldToScreenPoint(target.position);

        
    }

    public void InitiateWarp()
    {
        
        //Lock player movement
        
        
        //Make player immortal
        
        
        //Play warp
        StartCoroutine(Warp(1));
    }

    private IEnumerator Warp(int dir)
    {
       print("called");
        float cT = dir==1?0:warpDur;
        while (cT <= warpDur && cT >= 0)
        {
            Shader.SetGlobalVector(movementID, parentForward * -warpCurve.Evaluate(cT/warpDur));
            cT += Time.deltaTime * dir;
            yield return null;
        }

        fromWarp = true;



        if (dir == 1)
        {
            Enemy.Blips.Clear();
            Enemy.Targeting.Clear();
            int prv = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadSceneAsync(warpTo);
            SceneManager.UnloadSceneAsync(prv);
        }

    }


    public void UpdateHealth(float currentHealth)
    {
        healthLeft.value = currentHealth;
        healthRight.value = currentHealth;
    }

    private Coroutine blinking;

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
    private readonly GameObject blipObject;
    private readonly Image blipImage;
    private readonly Image blipBaseImage;
    private readonly uint blipID;

    public Blip(Transform core, RectTransform blip, uint id)
    {
        coreComponent = core;
        blipComponent = Object.Instantiate(blip, PlayerUI.EnemyBlipParent);
        blipObject = blipComponent.gameObject;
        blipBaseImage = blipComponent.GetComponent<Image>();
        blipImage = blipComponent.GetChild(0).GetComponent<Image>();
        blipID = id;
    }
    public void DestroyBlip()
    {
        Debug.Log("Destroying Blip");
        Object.Destroy(blipObject);
        Debug.Log(Enemy.Blips.Count);
    }
    
    public void UpdatePosition(Vector3 start)
    {
        Vector3 dif = coreComponent.position - start;
        float mag = dif.magnitude;
        //if the enemy is in range of the players' "RADAR DISTANCE"
        //then display the enemy...
        if (mag < PlayerUI.RadarDist)
        {
            //Enable the blip
            blipObject.SetActive(true);
            
            //Convert the blip position to local space...
            Vector3 pos = new Vector3(dif.x, dif.z);

            //Keep the blips in bounds...
            if (pos.sqrMagnitude > 6400) pos = pos.normalized * 80;

            blipComponent.localPosition = pos;

            //let 0.5 be the center.
            //First, normalize the y., then we apply an offset
            //
            if(blipImage == null) Debug.Log("blipImage is null");
            if(Settings.verticalityGradient == null) Debug.Log("verticalityGradient is null");
            
            blipImage.color = Settings.verticalityGradient.Evaluate((dif.y / mag + 1)/2);
            blipBaseImage.color = Enemy.Targeting[blipID] ? Settings.enemyTargetingPlayer : Settings.enemyLostPlayer;
            //blipComponent.position = coreComponent.position
        }
        else
        {
            //Blip out of range (play animation?)
            blipObject.SetActive(false);
        }
    }
}
