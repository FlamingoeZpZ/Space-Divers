using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DEBUGLutManager : MonoBehaviour
{
    private int lutIdx = -1;
    [SerializeField] private Texture[] luts;
    
    [SerializeField] private Material mRenderMaterial;

    private readonly int lutTex = Shader.PropertyToID("_LUT");
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, mRenderMaterial);
    }

    private void Awake()
    {
        RotateLut();
        DontDestroyOnLoad(gameObject);
    }

    public void RotateLut()
    {
        if (++lutIdx >= luts.Length)
        {
            lutIdx = 0;
        }
        print("swapping");
        mRenderMaterial.SetTexture(lutTex, luts[lutIdx]);
    }
}
