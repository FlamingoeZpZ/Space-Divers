using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    //First add self to owner
    [field: SerializeField] public WeaponStats stats { get; private set; }
    [SerializeField] private Projectile projectile; // Eventually this will be moved elsewhere to a SO.
    private int myLayer;
    private Transform parent;

    private bool canShoot = true;
    
    private void Awake()
    {
        myLayer = gameObject.layer; 
        parent = transform.root;
        if (parent.TryGetComponent(out BaseCharacter ch))
        {
            ch.AddWeapon(this, stats.isTargeting || projectile.isHoming, stats.isAutomatic);
        }

        
        
    }

    /// <summary>
    /// Shoots towards a target
    /// </summary>
    /// <param name="target"></param>
    public void TryShoot(Transform target)
    {

        if (!canShoot) return;
        StartCoroutine(ShootDelay());
        
        
        
        Projectile instance = Instantiate(projectile, transform.position, transform.rotation,
            GameManager.instance.bulletParent);
        if (target)
        {
            if (projectile.isHoming)
            {
                instance.transform.forward = transform.forward;
                instance.Init(myLayer, parent, target);
                return;
            }
            if (stats.isTargeting)
            {
                instance.transform.forward = (target.position - transform.position).normalized;
            }
        }

        instance.Init(myLayer, parent);
    }

    private IEnumerator ShootDelay()
    {
        canShoot = false;
        yield return new WaitForSeconds(stats.timeBetweenShots);
        canShoot = true;
    }
}
