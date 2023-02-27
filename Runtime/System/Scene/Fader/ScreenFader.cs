using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace CollieMollie.System
{
    public class ScreenFader : MonoBehaviour
    {
        #region Variable Field
        [SerializeField] private Image _fadeImage = null;
        [SerializeField] private float _fadeDuration = 1f;
        [SerializeField] private AnimationCurve _fadeCurve = null;

        #endregion

        #region Public Functions
        public void SetColor(Color targetColor)
        {
            _fadeImage.color = targetColor;
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
