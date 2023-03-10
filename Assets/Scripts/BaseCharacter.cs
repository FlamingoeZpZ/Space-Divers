using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public abstract class BaseCharacter : MonoBehaviour, ITargetable
{
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
    protected Transform myTrans; //is this really faster?

    protected float roll;
    protected float curSpeed;

    [SerializeField] protected Vector2 direction;

    protected bool targetLocked;
    private Vector3 searchBoxExtent;
    protected virtual void Awake()
    {
        _currentHealth = stats.maxHealth;
        searchBoxExtent = new Vector3(viewDist.x, viewDist.x, viewDist.y);
        myTrans = transform;
        shipBody = transform.GetChild(0);
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
            w.TryShoot(null);
        }
    }



    //Problem: This assumes components aren't individualized.
    public virtual void UpdateHealth(Transform attacker, float damage)
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
        Vector3 fwd = myTrans.forward;
        RaycastHit[] hits = new RaycastHit[10];
        int c = Physics.BoxCastNonAlloc(eyePoint.position + fwd * viewDist.y, searchBoxExtent, fwd, hits,
            myTrans.rotation, viewDist.y, targetableLayers);
        #if UNITY_EDITOR
        ExtDebug.DrawBox(eyePoint.position + fwd * viewDist.y, searchBoxExtent, myTrans.rotation, Color.blue);
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
            if (w.stats.isAutomatic)
            {
                w.TryShoot(curTarget);
            }
        }
    }

    private bool bIsUpsideDown;

    protected virtual void Move()
    {
        //Handles movement logic.
        //Quaternion eulerAngles = myTrans.rotation;
        myTrans.position += curSpeed * Time.deltaTime * myTrans.forward;
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

        myTrans.rotation *= Quaternion.Euler(direction.x * s, direction.y * s,0);
        Vector3 n = myTrans.eulerAngles;

        /*
        if(direction == Vector2.zero && Mathf.Abs(n.z - 180) > 0.5f)
        {
            if (n.z > 180) n.z += Time.deltaTime * s;
            else if (n.z < 180) n.z -= stats.rollDecay * stats.baseHandling * Time.deltaTime;
        } */
        myTrans.eulerAngles = n;
        
        //eulerAngles.z = 0;
        
        
        
        //myTrans.eulerAngles += new Vector3(direction.x * s,direction.y * s,0);
        shipBody.localEulerAngles = new Vector3(0, 0, roll);
    }

    protected virtual void ShootAutomaticWeapons()
    {
        
    }

    public abstract void OnTargeted();

    public abstract void OnUnTargeted();
}
