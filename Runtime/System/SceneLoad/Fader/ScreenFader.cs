using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Broccollie.System
{
    public class ScreenFader : MonoBehaviour
    {
        [SerializeField] private FaderEventChannel _eventChannel;
        [SerializeField] private Image _fadeImage;
        [SerializeField] private float _fadeDuration = 1f;
        [SerializeField] private AnimationCurve _fadeCurve;

        private IEnumerator _fadeCoroutine;

        private void OnEnable()
        {
            if (_eventChannel != null)
            {
                _eventChannel.OnFade += ExecuteFade;
                _eventChannel.OnFadeAsync += ExecuteFadeAsync;
            }
        }

        private void OnDisable()
        {
            if (_eventChannel != null)
            {
                _eventChannel.OnFade -= ExecuteFade;
                _eventChannel.OnFadeAsync -= ExecuteFadeAsync;
            }
        }

        #region Subscribers
        private void ExecuteFade(float alpha)
        {
            if (_fadeCoroutine != null)
                StopCoroutine(_fadeCoroutine);

            _fadeCoroutine = Fade(alpha);
            StartCoroutine(_fadeCoroutine);
        }

        private async Task ExecuteFadeAsync(float alpha) => await FadeAsync(alpha);

        #endregion

        #region Public Functions
        public void SetColor(Color targetColor)
        {
            _fadeImage.color = targetColor;
        }

        public void SetAlpha(float alpha)
        {
            Color startColor = _fadeImage.color;
            startColor.a = alpha;
            _fadeImage.color = startColor;
        }

        public IEnumerator Fade(float alpha, Action done = null)
        {
            Color startColor = _fadeImage.color;
            Color targetColor = startColor;
            targetColor.a = alpha;

            float elapsedTime = 0;
            while (elapsedTime < _fadeDuration)
            {
                _fadeImage.color = Color.Lerp(startColor, targetColor, _fadeCurve.Evaluate(elapsedTime / _fadeDuration));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            _fadeImage.color = targetColor;
            done?.Invoke();
        }

        public async Task FadeAsync(float alpha, Action done = null)
        {
            Color startColor = _fadeImage.color;
            Color targetColor = startColor;
            targetColor.a = alpha;

            float elapsedTime = 0;
            while (elapsedTime < _fadeDuration)
            {
                _fadeImage.color = Color.Lerp(startColor, targetColor, _fadeCurve.Evaluate(elapsedTime / _fadeDuration));
                elapsedTime += Time.deltaTime;
                await Task.Yield();
            }
            _fadeImage.color = targetColor;
            done?.Invoke();
        }

        #endregion
    }
}
