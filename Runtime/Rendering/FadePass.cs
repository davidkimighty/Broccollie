using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace CollieMollie.Rendering
{
    public class FadePass : ScriptableRenderPass
    {
        #region Variable Field
        private static readonly int s_alphaProperty = Shader.PropertyToID("_Alpha");
        private static readonly int s_fadeColorProperty = Shader.PropertyToID("_FadeColor");

        private FadeFeature.PassSetting _settings = null;
        private Material _fadeMaterial = null;
        #endregion

        public FadePass (FadeFeature.PassSetting settings)
        {
            _settings = settings;
            _fadeMaterial = settings.FadeMaterial;
            SetFadeColor(settings.FadeColor);
            SetFadeAmount(settings.FadeValue);
            renderPassEvent = settings.RenderPassEvent;
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer command = CommandBufferPool.Get(_settings.ProfileTag);

            RenderTargetIdentifier source = BuiltinRenderTextureType.CameraTarget;
            RenderTargetIdentifier destination = BuiltinRenderTextureType.CurrentActive;

            command.Blit(source, destination, _fadeMaterial);
            context.ExecuteCommandBuffer(command);

            CommandBufferPool.Release(command);
        }

        #region Public Functions
        public void SetFadeColor(Color color)
        {
            if (_fadeMaterial == null) return;
            _fadeMaterial.SetColor(s_fadeColorProperty, color);
        }

        public void SetFadeAmount(float fadeValue)
        {
            if (_fadeMaterial == null) return;
            _fadeMaterial.SetFloat(s_alphaProperty, fadeValue);
        }

        public float GetFadeAmount()
        {
            if (_fadeMaterial == null) return 0f;
            return _fadeMaterial.GetFloat(s_alphaProperty);
        }
        #endregion
    }
}
