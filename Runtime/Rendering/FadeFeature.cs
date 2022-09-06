using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace CollieMollie.Rendering
{
    public class FadeFeature : ScriptableRendererFeature
    {
        #region Variable Field
        public FadePass RenderPass = null;
        public PassSetting Settings = new PassSetting();
        #endregion

        public override void Create()
        {
            Settings.FadeMaterial = CoreUtils.CreateEngineMaterial("Hidden/Fade");
            RenderPass = new FadePass(Settings);
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            renderer.EnqueuePass(RenderPass);
        }

        [Serializable]
        public class PassSetting
        {
            [HideInInspector] public string ProfileTag = "Screen Fade";
            [HideInInspector] public Material FadeMaterial = null;

            public Color FadeColor;
            [Range(0, 1)] public float FadeValue = 0f;
            public RenderPassEvent RenderPassEvent = RenderPassEvent.AfterRenderingPostProcessing;
        }
    }
}
