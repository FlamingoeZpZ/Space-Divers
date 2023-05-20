using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Weapon : MonoBehaviour
{
    //First add self to owner
    [field: SerializeField] public WeaponStats Stats { get; private set; }
    [SerializeField] private Projectile projectile; // Eventually this will be moved elsewhere to a SO.
    [SerializeField] private VisualEffect onHitFX;
    [SerializeField] private int toSpawn = 3;

    //Array pool structure... Is this smart or even necessary??
    private VisualEffect[] onHitEffects;
    private int index;


    private int ignoreLayer;
    private Transform parent;
    private VisualEffect vfx;

    private static readonly int ShootID = Shader.PropertyToID("Fire");
    private static readonly int LoopDurID = Shader.PropertyToID("LoopDur");
    private static readonly int ExplosionSparksID = Shader.PropertyToID("SecondarySize");
    private static readonly int ColorID = Shader.PropertyToID("Color");

    [SerializeField] private bool useLoopDur;
    [SerializeField] private bool useColor = true;
    [SerializeField] private bool useSecondaryAmount;
    
    [SerializeField] private float loopDur;
    [SerializeField] private Vector2 secondaryAmount;

    private bool canShoot = true;
    
    
    
    
    private void Start() //Should be start, This is a function that depends other objects.
    {
        //Player has two layers in order for placements. This is essential to prevent collisions with all parts of the player, while still working for enemies
        int rL = 1 << transform.root.gameObject.layer;
        ignoreLayer = 1 << gameObject.layer;
        print("layerCheck: " + rL + " + " + ignoreLayer);
        
        if (rL != ignoreLayer)
            ignoreLayer += rL;
        
        

        if (transform.parent.parent.TryGetComponent(out Turret t))
        {
            parent = transform.parent.parent;
            t.AppendWeapon(this);
            
        }
        else if (transform.root.TryGetComponent(out BaseCharacter ch))
        {
            parent = transform.root;
            ch.AddWeapon(this, Stats.isTargeting || projectile.isHoming, Stats.isAutomatic);
        }

        vfx = transform.GetChild(0).GetComponent<VisualEffect>();
        Color c = projectile.Color;
        vfx.SetVector4(ColorID, c);

        onHitEffects = new VisualEffect[toSpawn];

        for (int i = 0; i < toSpawn; ++i)
        {
            onHitEffects[i] = Instantiate(onHitFX, GameManager.instance.bulletParent);
            if(useColor)
                onHitEffects[i].SetVector4(ColorID, c);
            if(useLoopDur)
                onHitEffects[i].SetFloat(LoopDurID, loopDur);
            if(useSecondaryAmount)
                onHitEffects[i].SetVector2(ExplosionSparksID, secondaryAmount);
        }
    }

    private void MyProjectileHit(Transform other)
    {
        print("Summoned Projectile");
        Transform t = onHitEffects[index].transform;
        t.position = other.position;
        t.forward = other.forward;
        onHitEffects[index++].SendEvent(ShootID);
        if (index == toSpawn)
            index = 0;
    }

    /// <summary>
    /// Shoots towards a target
    /// </summary>
    /// <param name="target"></param>
    public void TryShoot(Transform target)
    {

        if (!canShoot) return;
        StartCoroutine(ShootDelay());
        Vector3 tp = transform.position;
        Projectile instance = Instantiate(projectile, tp, transform.rotation,
            GameManager.instance.bulletParent);
        
        print("activated VFX?");
        vfx.SendEvent(ShootID);
        
        //NO CODE BELOW
        Transform iT = instance.transform;
        if (target)
        {
            
            if (projectile.isHoming)
            {
                iT.forward = iT.forward;
                instance.Init(ignoreLayer, parent, () => MyProjectileHit(iT), target);
                return;
            }
            if (Stats.isTargeting)
            {
                iT.forward = (target.position - tp).normalized;
            }
        }
       
        instance.Init(ignoreLayer, parent, () => MyProjectileHit(iT));
    }

    private void OnDestroy()
    {
        foreach (VisualEffect v in onHitEffects)
        {
          Destroy(v.gameObject,projectile.Lifetime + 1); // 1s Wait for it to finish (Just in case)  
        }
    }

    private IEnumerator ShootDelay()
    {
        canShoot = false;
        yield return new WaitForSeconds(Stats.timeBetweenShots);
        canShoot = true;
    }
}
