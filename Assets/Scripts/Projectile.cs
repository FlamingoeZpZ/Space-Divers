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
    private Vector3 accel;
    public bool isHoming => stats.IsHoming;
    public Color Color => stats.Color;

    private Action onHit;
    
    private void Awake()
    {
        accel = transform.forward * stats.Speed;
        Destroy(gameObject, stats.LifeTime);
    }

    /// <summary>
    /// Should be called AFTER setting transform information.
    /// </summary>
    /// <param name="ignoreLayers"></param>
    /// <param name="myOwner"></param>
    /// <param name="hitAction"></param>
    /// <param name="setTarget"></param>
    public void Init(int ignoreLayers, Transform myOwner, Action hitAction, Transform setTarget = null)
    {
        //Prevent cheating? Idk if even necessary.
        if (initialized) return;
        initialized = true;
        onHit = hitAction;
        layer = ~ignoreLayers;
        target = setTarget;
        direction = transform.forward;
        owner = myOwner;
    }

    // Update is called once per frame
    void Update()
    {
        if (stats.IsHoming && target)
        {
            direction = Vector3.Normalize(target.position - transform.position);
        }

        if (stats.Accelerates)
        {
            accel +=  stats.Speed * Time.deltaTime *direction;
        }



        transform.position += accel;
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit h, 2, layer))
        {
            transform.forward = h.normal;
            transform.position = h.point;
            onHit.Invoke();
            if (h.transform.root.TryGetComponent(out BaseCharacter c))
            {
                c.UpdateHealth(owner,-stats.Damage);
            }
            Destroy(gameObject); // Play particle effect here
        }
    }
}
