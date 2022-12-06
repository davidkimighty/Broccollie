using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CollieMollie.Core;
using UnityEngine;

namespace CollieMollie.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UIGroup : BaseUI
    {
        #region Variable Field
        [Header("Group")]
        [SerializeField] private GameObject _groupObject = null;

        [Header("Group Features")]
        [SerializeField] private bool _useFade = true;
        [SerializeField] private CanvasGroup _canvasGroup = null;
        [SerializeField] private float _fadeDuration = 0.6f;
        [SerializeField] private AnimationCurve _fadeCurve = null;

        [SerializeField] private UITransformFeature _transformFeature = null;

        private Operation _featureOperation = new Operation();

        #endregion

        #region Behaviors
        protected override void ShowBehavior(bool playAudio = true, bool invokeEvent = true, Action done = null)
        {
            _currentState = UIState.Show;
            if (_transformFeature != null)
                _transformFeature.Execute(UIState.Show.ToString(), out float duration, done);

            if (_useFade)
            {
                _featureOperation.Stop(this);
                _featureOperation.Add(Fade(_canvasGroup, 1f, _fadeDuration, _fadeCurve));
                _featureOperation.Start(this, _fadeDuration);
            }
        }

        protected override void HideBehavior(bool playAudio = true, bool invokeEvent = true, Action done = null)
        {
            _currentState = UIState.Hide;
            if (_transformFeature != null)
                _transformFeature.Execute(UIState.Hide.ToString(), out float duration, done);

            if (_useFade)
            {
                _featureOperation.Stop(this);
                _featureOperation.Add(Fade(_canvasGroup, 0, _fadeDuration, _fadeCurve));
                _featureOperation.Start(this, _fadeDuration);
            }
        }

        #endregion

        #region Features
        protected override void SetActive(bool state)
        {
            _groupObject.SetActive(state);
        }

        private IEnumerator Fade(CanvasGroup group, float targetValue, float duration, AnimationCurve curve)
        {
            float elapsedTime = 0f;
            float startValue = group.alpha;

            while (elapsedTime < duration)
            {
                group.alpha = Mathf.Lerp(startValue, targetValue, curve.Evaluate(elapsedTime / duration));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            group.alpha = targetValue;
        }

        #endregion
    }
}
