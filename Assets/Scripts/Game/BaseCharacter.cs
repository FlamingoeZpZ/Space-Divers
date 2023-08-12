using System.Collections.Generic;
using Game;
using Stats.ComponentStats;
using UnityEngine;

public abstract class BaseCharacter : MonoBehaviour, ITargetable
{
    public float GetCurrentSpeed => (transform.position - prvPos).magnitude;

    private Vector3 prvPos;

    //Serialized for enemies, but accessible
    protected readonly List<Weapon> Weapons = new();
    protected bool hasTargetingCapability { get; private set; }
    protected bool hasAutomaticWeapons { get; private set; }
    
    protected bool CanShootAutoWeapons = true;


    protected float _currentHealth;
    [SerializeField] private LayerMask targetableLayers;


    [SerializeField] private Transform eyePoint;
    private Transform shipBody;
    [SerializeField] private Vector2 viewDist;

    [SerializeField] protected ShipBaseStats stats;
    
    protected Transform curTarget;

    protected float roll;
    protected float curSpeed;

    [SerializeField] protected Vector2 direction;

    protected bool targetLocked;
    private Vector3 searchBoxExtent;

    private float numBullets;
    private float numEnergy;
    private float numRockets;
    
    protected virtual void Awake()
    {
        _currentHealth = stats.maxHealth;
        searchBoxExtent = new Vector3(viewDist.x, viewDist.x, viewDist.y);
        shipBody = transform.GetChild(0);

        numBullets = stats.maxBullets;
        numEnergy = stats.maxEnergy;
        numRockets = stats.maxRockets;

    }

    public void AddWeapon(Weapon newWeapon, bool homing, bool automatic)
    {
        Weapons.Add(newWeapon);
        if (homing)
        {
            hasTargetingCapability = true;
        }

        if (automatic)
        {
            hasAutomaticWeapons = true;
        }
    }

    public void FireWeapons()
    {
        print(Weapons.Count);
        
        foreach (Weapon w in Weapons)
        {
            w.TryStartFiring(curTarget);
        }
    }

    public void StopFiringWeapons()
    {
        print(Weapons.Count);
        foreach (Weapon w in Weapons)
        {
            w.StopFiring();
        } 
    }


    //Problem: This assumes components aren't individualized.
    public virtual void UpdateHealth(BaseCharacter attacker, float damage)
    {
        _currentHealth += damage;

        if (_currentHealth < 0)
            Destroy(gameObject); // Temporary...
        else if (_currentHealth > stats.maxHealth)
            _currentHealth = stats.maxHealth;
    }
    
   
    protected virtual void Update()
    {
        //handles shooting logic
        //1) Raycast
        Move();
        Vector3 fwd = transform.forward;
        RaycastHit[] hits = new RaycastHit[10];
        int c = Physics.BoxCastNonAlloc(eyePoint.position + fwd * viewDist.y, searchBoxExtent, fwd, hits, transform.rotation, viewDist.y, targetableLayers);
        #if UNITY_EDITOR
        ExtDebug.DrawBox(eyePoint.position + fwd * viewDist.y, searchBoxExtent, transform.rotation, Color.blue);
        #endif
        //print(c);
        if(!targetLocked)
            curTarget = null;
        for (int i = 0; i < c; ++i)
        {
            //TODO: better enemy AI
            
            if ((1<<hits[i].transform.gameObject.layer & targetableLayers) != 0) // This may cause lag...
            {
                curTarget =  hits[i].transform;
                break;
            }
        }
        //If we have a target
        if (!curTarget || !hasAutomaticWeapons || !CanShootAutoWeapons)
            return;
        foreach (Weapon w in Weapons)
        {
            if (w.Stats.isAutomatic)
            {
                w.TryStartFiring(curTarget);
            }
        }
    }

    private bool bIsUpsideDown;

    protected virtual void Move()
    {
        //Handles movement logic.
        //Quaternion eulerAngles = myTrans.rotation;
        Vector3 position = transform.position;
        prvPos = position;
        position += curSpeed * Time.deltaTime * transform.forward;
        transform.position = position;
        float s = (stats.baseHandling + curSpeed) * Time.deltaTime;
        if (Mathf.Abs(direction.y) > 0) //if going right and left
        {
            float m = stats.rollSpeed * direction.y * s ;
            if (Mathf.Abs(roll - m) < stats.maxRollAngle)
                roll -= m;
        }
        else if(roll < -stats.rollDecay)
            roll += stats.rollDecay * stats.baseHandling * Time.deltaTime * s;
        else if (roll >stats.rollDecay)
            roll -= stats.rollDecay * stats.baseHandling * Time.deltaTime * s;

        transform.rotation *= Quaternion.Euler(direction.x * s, direction.y * s,0);
        Vector3 n = transform.eulerAngles;

        /*
        if(direction == Vector2.zero && Mathf.Abs(n.z - 180) > 0.5f)
        {
            if (n.z > 180) n.z += Time.deltaTime * s;
            else if (n.z < 180) n.z -= stats.rollDecay * stats.baseHandling * Time.deltaTime;
        } */
        transform.eulerAngles = n;
        
        //eulerAngles.z = 0;
        
        
        
        //myTrans.eulerAngles += new Vector3(direction.x * s,direction.y * s,0);
        shipBody.localEulerAngles = new Vector3(0, 0, roll);
    }

    protected virtual void ShootAutomaticWeapons()
    {
        
    }

    public abstract void OnTargeted();

    public abstract void OnUnTargeted();

    public bool CanShoot(EAmmoType statsAmmoType, float statsFireCost, bool spendAmmo = false)
    {
        print("Remaining Ammo: " + numEnergy);
        
        switch (statsAmmoType)
        {
            case EAmmoType.Bullet:
                if (numBullets >= statsFireCost)
                {
                    if (spendAmmo)
                        numBullets -= statsFireCost;
                    return true;
                }
                return false;
            case EAmmoType.Energy:
                if (numEnergy >= statsFireCost)
                {
                    if (spendAmmo)
                        numEnergy -= statsFireCost;
                    return true;
                }
                return false;
            case EAmmoType.Explosive:
                if (numRockets >= statsFireCost)
                {
                    if (spendAmmo)
                        numRockets -= statsFireCost;
                    return true;
                }
                return false;
        }
        return false;
    }
}
