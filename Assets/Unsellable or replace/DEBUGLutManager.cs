using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
[DefaultExecutionOrder(300)]
public class DEBUGLutManager : MonoBehaviour
{
    private static int lutIdx;
    [SerializeField] private Texture[] luts;
    
    [SerializeField] private Material mRenderMaterial;

    private readonly int lutTex = Shader.PropertyToID("_LUT");
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, mRenderMaterial);
    }

    public static DEBUGLutManager Instance;
    
    private void Awake()
    {
        Instance = this;
        mRenderMaterial.SetTexture(lutTex, luts[lutIdx]);
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
