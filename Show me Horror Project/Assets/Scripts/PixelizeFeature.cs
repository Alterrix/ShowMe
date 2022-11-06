using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PixelizeFeature : ScriptableRendererFeature
{
    [Serializable]
    public class CustomPassSettings
    {
        public RenderPassEvent renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
        public int screenHeight = 144;
    }

    public class PixelizePass : ScriptableRenderPass
    {
        private CustomPassSettings settings;

        private RenderTargetIdentifier colorBuffer, pixelBuffer;
        private int pixelBufferID = Shader.PropertyToID("_PixelBuffer");

        //private RenderTargetIdentifier pointBuffer;
        //private int pointBufferID = Shader.PropertyToID("_PointBuffer");

        private Material material;
        private int pixelScreenHeight, pixelScreenWidth;

        public PixelizePass(CustomPassSettings settings)
        {
            this.settings = settings;
            this.renderPassEvent = settings.renderPassEvent;
            if (material == null) material = CoreUtils.CreateEngineMaterial("Hidden/Pixelize");
        }

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            colorBuffer = renderingData.cameraData.renderer.cameraColorTarget;
            RenderTextureDescriptor descriptor = renderingData.cameraData.cameraTargetDescriptor;

            //cmd.GetTemporaryRT(pointBufferID, descriptor.width, descriptor.height, 0, FilterMode.Point);
            //pointBuffer = new RenderTargetIdentifier(pointBufferID);

            pixelScreenHeight = settings.screenHeight;
            pixelScreenWidth = (int) (pixelScreenHeight * renderingData.cameraData.camera.aspect + 0.5f);

            material.SetVector("_BlockCount", new Vector2(pixelScreenWidth, pixelScreenHeight));
            material.SetVector("_BlockSize", new Vector2(1.0f / pixelScreenWidth, 1.0f / pixelScreenHeight));
            material.SetVector("_HalfBlockSize", new Vector2(0.5f / pixelScreenWidth, 0.5f / pixelScreenHeight));

            descriptor.height = pixelScreenHeight;
            descriptor.width = pixelScreenWidth;

            cmd.GetTemporaryRT(pixelBufferID, descriptor, FilterMode.Point);
            pixelBuffer = new RenderTargetIdentifier(pixelBufferID);
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer cmd = CommandBufferPool.Get();
            using (new ProfilingScope(cmd, new ProfilingSampler("Pixelize Pass")))
            {
                // No-shader variant
                //Blit(cmd, colorBuffer, pointBuffer);
                //Blit(cmd, pointBuffer, pixelBuffer);
                //Blit(cmd, pixelBuffer, colorBuffer);

                Blit(cmd, colorBuffer, pixelBuffer, material);
                Blit(cmd, pixelBuffer, colorBuffer);
            }

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void OnCameraCleanup(CommandBuffer cmd)
        {
            if (cmd == null) throw new System.ArgumentNullException("cmd");
            cmd.ReleaseTemporaryRT(pixelBufferID);
            //cmd.ReleaseTemporaryRT(pointBufferID);
        }
    }

    [SerializeField] private CustomPassSettings settings;
    private PixelizePass customPass;

    public override void Create()
    {
        customPass = new PixelizePass(settings);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
#if UNITY_EDITOR
        if (renderingData.cameraData.isSceneViewCamera) return;
#endif
        renderer.EnqueuePass(customPass);
    }
}