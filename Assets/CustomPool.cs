using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class CustomPool : MonoBehaviour
{
    public static CustomPool Instance { get; private set; }

    [SerializeField] private VisualEffect explosionEffect;
    private const int StoredExplosions = 30;
    private readonly Queue<VisualEffect> explosionEffects = new();
    private readonly int explodeID = Shader.PropertyToID("Explode");
    private readonly int scaleID = Shader.PropertyToID("Scale");

    public void Start()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(Instance);

        for (int i = 0; i < StoredExplosions; ++i)
        {
            print("Creating Explosion");
            explosionEffects.Enqueue(Instantiate(explosionEffect,transform));
        }
    }

    public void Explode(float x, float y, float z, float scale = 5)
    {
        Explode(new Vector3(x,y,z), scale);
    }

    public void Explode(Vector3 pos, float scale = 5)
    {
        VisualEffect v = explosionEffects.Dequeue();
        v.SendEvent(explodeID);
        v.SetFloat(scaleID, scale);
        v.transform.position = pos;
        explosionEffects.Enqueue(v);
    }

}
