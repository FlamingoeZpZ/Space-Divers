using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private ProjectileStats stats;
    private int layer;
    private Transform target;
    private bool initialized;
    private Transform owner;
    private Vector3 direction;
    public bool isHoming => stats.isHoming;

    private void Awake()
    {
        Destroy(gameObject, stats.lifeTime);
    }

    /// <summary>
    /// Should be called AFTER setting transform information.
    /// </summary>
    /// <param name="allyLayer"></param>
    /// <param name="myOwner"></param>
    /// <param name="setTarget"></param>
    public void Init(int allyLayer, Transform myOwner, Transform setTarget = null)
    {
        //Prevent cheating? Idk if even necessary.
        if (initialized) return;
        initialized = true;

        layer = ~(1<<allyLayer);
        target = setTarget;
        direction = transform.forward;
        owner = myOwner;
    }

    // Update is called once per frame
    void Update()
    {
        if (stats.isHoming && target)
        {
            direction = Vector3.Normalize(target.position - transform.position);
        }

        transform.position += direction * stats.speed;
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit h, 2, layer))
        {
            if (h.transform.root.TryGetComponent(out BaseCharacter c))
            {
                c.UpdateHealth(owner,-stats.damage);
            }
            Destroy(gameObject); // Play particle effect here
        }
    }
}
