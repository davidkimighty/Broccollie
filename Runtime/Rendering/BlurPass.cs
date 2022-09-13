using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace CollieMollie.Rendering
{
    public class BlurPass : ScriptableRenderPass
    {
        #region Variable Field
        private const string PROFILER_TAG = "Blur Pass";
        private static readonly int s_blurStrengthProperty = Shader.PropertyToID("_BlurStrength");

        private BlurFeature.PassSetting _passSettings = null;
        private RenderTargetIdentifier _colorBuffer;
        private RenderTargetIdentifier _temporaryBuffer;
        private int _temporaryBufferID = Shader.PropertyToID("_TemporaryBuffer");
        private Material _blurMaterial = null;
        #endregion

        public BlurPass(BlurFeature.PassSetting passSettings)
        {
            this._passSettings = passSettings;

            renderPassEvent = passSettings.RenderPassEvent;
            _blurMaterial = passSettings.BlurMat;
            _blurMaterial.SetFloat(s_blurStrengthProperty, passSettings.BlurStrength);
        }

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            RenderTextureDescriptor descriptor = renderingData.cameraData.cameraTargetDescriptor;

            descriptor.width /= _passSettings.Downsample;
            descriptor.height /= _passSettings.Downsample;
            descriptor.depthBufferBits = 0;

            _colorBuffer = renderingData.cameraData.renderer.cameraColorTarget;

            cmd.GetTemporaryRT(_temporaryBufferID, descriptor, FilterMode.Bilinear);
            _temporaryBuffer = new RenderTargetIdentifier(_temporaryBufferID);
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer cmd = CommandBufferPool.Get();
            using (new ProfilingScope(cmd, new ProfilingSampler(PROFILER_TAG)))
            {
                Blit(cmd, _colorBuffer, _temporaryBuffer, _blurMaterial, 0); // shader pass 0
                Blit(cmd, _temporaryBuffer, _colorBuffer, _blurMaterial, 1); // shader pass 1
            }
            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void OnCameraCleanup(CommandBuffer cmd)
        {
            if (cmd == null) throw new ArgumentNullException("cmd");

            cmd.ReleaseTemporaryRT(_temporaryBufferID);
        }

        #region Blur Controls
        public bool IsValid()
        {
            return _blurMaterial != null;
        }

        public void SetBlurStrength(float strength)
        {
            if (_blurMaterial == null) return;
            _blurMaterial.SetFloat(s_blurStrengthProperty, strength);
        }

        public float GetBlurStrength()
        {
            if (_blurMaterial == null) return 0f;
            return _blurMaterial.GetFloat(s_blurStrengthProperty);
        }
        #endregion
    }
}
