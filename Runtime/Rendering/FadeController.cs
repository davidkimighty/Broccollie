using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace CollieMollie.Rendering
{
    public class FadeController : MonoBehaviour
    {
        #region Variable Field
        private const float MAX_VALUE = 1f;
        private const float MIN_VALUE = 0f;

        [SerializeField] private UniversalRendererData _rendererData = null;
        [SerializeField] private AnimationCurve _fadeCurve = null;

        private FadeFeature _fadeFeature = null;
        #endregion

        private void Awake()
        {
            if (_rendererData == null) return;
            _fadeFeature = (FadeFeature)_rendererData.rendererFeatures.Find(feature => feature is FadeFeature);
        }

        #region Public Functions
        public void SetFadeColor(Color color)
        {
            _fadeFeature.RenderPass.SetFadeColor(color);
        }

        public IEnumerator FadeIn(float duration = 1f, Action done = null)
        {
            yield return Fade(MAX_VALUE, duration);
            done?.Invoke();
        }

        public IEnumerator FadeOut(float duration = 1f, Action done = null)
        {
            yield return Fade(MIN_VALUE, duration);
            done?.Invoke();
        }

        public IEnumerator FadeAmount(float fadeAmount, float duration = 1f, Action done = null)
        {
            fadeAmount = Mathf.Clamp(fadeAmount, MIN_VALUE, MAX_VALUE);
            yield return Fade(fadeAmount, duration);
            done?.Invoke();
        }

        public void FadeAmout(float fadeAmount)
        {
            _fadeFeature.RenderPass.SetFadeAmount(fadeAmount);
        }
        #endregion

        #region Fade Features
        private IEnumerator Fade(float targetValue, float duration = 1f)
        {
            float elapsedTime = 0f;
            float startFadeValue = _fadeFeature.RenderPass.GetFadeAmount();
            while (elapsedTime < duration)
            {
                _fadeFeature.RenderPass.SetFadeAmount(Mathf.Lerp(startFadeValue, targetValue,
                    _fadeCurve.Evaluate(elapsedTime / duration)));

                elapsedTime += Time.deltaTime;
                yield return null;
            }
            _fadeFeature.RenderPass.SetFadeAmount(targetValue);
        }
        #endregion
    }
}
