using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineTexture : MonoBehaviour
{

    [SerializeField] private ComputeShader shader;
    private RenderTexture renderTexture;

    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (renderTexture == null)
        {
            renderTexture = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.Default);
            renderTexture.enableRandomWrite = true;
            renderTexture.Create();

            int kernel = shader.FindKernel("CSMain");
            shader.SetTexture(kernel, "Result", renderTexture);

            shader.SetTexture(0, "Result", renderTexture);

            int workGroupsX = Mathf.CeilToInt(Screen.width/8f);
            int workGroupsY = Mathf.CeilToInt(Screen.height/8f);
            
            shader.Dispatch(kernel, workGroupsX, workGroupsY, 1);
            Graphics.Blit(renderTexture, dest);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
