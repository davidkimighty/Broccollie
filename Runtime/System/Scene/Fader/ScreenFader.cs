using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace CollieMollie.System
{
    public class ScreenFader : MonoBehaviour
    {
        #region Variable Field
        [SerializeField] private CanvasGroup _canvasGroup = null;
        [SerializeField] private Image _fadeImage = null;
        [SerializeField] private AnimationCurve _fadeCurve = null;

        #endregion

        #region Public Functions
        public void SetColor(Color targetColor)
        {
            _fadeImage.color = targetColor;
        }

        public IEnumerator Fade(float targetValue, float duration, Action done = null)
        {
            float startValue = _canvasGroup.alpha;
            float elapsedTime = 0;
            while (elapsedTime < duration)
            {
                _canvasGroup.alpha = Mathf.Lerp(startValue, targetValue, _fadeCurve.Evaluate(elapsedTime / duration));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            _canvasGroup.alpha = targetValue;
            done?.Invoke();
        }
        #endregion
    }
}
