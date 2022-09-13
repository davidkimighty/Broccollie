using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace CollieMollie.Rendering
{
    public class BlurFeature : ScriptableRendererFeature
    {
        public BlurPass RenderPass = null;
        public PassSetting Settings = new PassSetting();

        public override void Create()
        {
            Settings.BlurMat = CoreUtils.CreateEngineMaterial("Hidden/Blur");
            RenderPass = new BlurPass(Settings);
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            if (RenderPass.IsValid())
                renderer.EnqueuePass(RenderPass);
        }

        [Serializable]
        public class PassSetting
        {
            public RenderPassEvent RenderPassEvent = RenderPassEvent.AfterRenderingTransparents;
            [HideInInspector] public Material BlurMat = null;
            [Range(1, 4)] public int Downsample = 1;
            [Range(0, 20)] public float BlurStrength = 5;
        }
    }
}
