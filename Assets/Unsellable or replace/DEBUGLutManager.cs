using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DEBUGLutManager : ScriptableRenderPass
{
    private static int lutIdx;
    private Texture[] luts;
    
    private Material mRenderMaterial;

    private readonly int lutTex = Shader.PropertyToID("_LUT");
    
    public DEBUGLutManager(RenderPassEvent evt, Shader shader)
    {
        renderPassEvent = evt;
        if (shader == null)
        {
            Debug.Log("No Shader");
            return;
        }
 
        mRenderMaterial = CoreUtils.CreateEngineMaterial(shader);
    }

    private const string KRenderTag = "LUT"; // Add tag for Frame Debugger ???

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        if (mRenderMaterial == null)
        {
            Debug.LogError("Material not Created");
            return;
        }
 
        if (!renderingData.cameraData.postProcessEnabled) return;
   
        CommandBuffer cmd = CommandBufferPool.Get(KRenderTag); //???
        Render(cmd, ref renderingData);
        context.ExecuteCommandBuffer(cmd);
        CommandBufferPool.Release(cmd);
    }
    
    void Render(CommandBuffer cmd, ref RenderingData renderingData)
    {
        CameraData cameraData = renderingData.cameraData;
        //RenderTargetIdentifier source = currentTarget;
        //int destination = TempTargetId;
        int shaderPass = 0;
   
        int w = cameraData.camera.scaledPixelWidth >> 3;
        int h = cameraData.camera.scaledPixelHeight >> 3;
   
        //cmd.GetTemporaryRT(destination, w, h, 0, FilterMode.Point, RenderTextureFormat.Default);
        //cmd.Blit(source, destination, blitRenderMaterial, shaderPass);
        //cmd.Blit(source, source, mRenderMaterial, shaderPass);
   
        //cmd.ReleaseTemporaryRT(destination);
    }
}
