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
    public bool isHoming => stats.IsHoming;
    public Color Color => stats.Color;
    public float Lifetime => stats.LifeTime;

    private Action onHit;

    private bool hasExploded;
    
    private void Awake()
    {
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

    private readonly Collider[] hits = new Collider[8];
    private float lifeTime;
    // Update is called once per frame
    void Update()
    {
        if (stats.IsHoming && target)
        {
            direction = Vector3.Lerp(transform.forward, Vector3.Normalize(target.position - transform.position), Time.deltaTime * stats.TurningRate);
        }

        lifeTime += Time.deltaTime;
        print("Accel: " + stats.Acceleration(lifeTime));
        transform.position += stats.Acceleration(lifeTime) * stats.Speed * Time.deltaTime *direction;
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit h, 2, layer))
        {
            transform.forward = h.normal;
            transform.position = h.point;
            Explode();
            Destroy(gameObject); // Play particle effect here
        }
    }

    private void Explode()
    {
        onHit.Invoke();
        int num = Physics.OverlapSphereNonAlloc(transform.position, stats.ExplosiveRadius, hits);
        for (int i = 0; i < num; ++ i) {
            if (hits[i].transform.root.TryGetComponent(out BaseCharacter c))
            {
                c.UpdateHealth(owner, -stats.Damage);
            }
        }
        hasExploded = true;
    }

    private void OnDestroy()
    {
        if (!hasExploded && stats.ExplodeOnDeath)
        {
            Explode();
        }
    }
}
