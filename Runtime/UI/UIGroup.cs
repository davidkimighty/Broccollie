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
        public event Action<UIEventArgs> OnInteractive = null;
        public event Action<UIEventArgs> OnNonInteractive = null;
        public event Action<UIEventArgs> OnShow = null;
        public event Action<UIEventArgs> OnHide = null;

        [SerializeField] private GameObject _groupObject = null;
        [SerializeField] private CanvasGroup _canvasGroup = null;
        [SerializeField] private bool _useFade = true;
        [SerializeField] private float _fadeDuration = 0.6f;
        [SerializeField] private AnimationCurve _fadeCurve = null;

        [SerializeField] private UIPositionFeature _positionFeature = null;
        [SerializeField] private UIScaleFeature _scaleFeature = null;

        private UIState _currentState = UIState.None;
        private Operation _stateChangeOperation = new Operation();
        #endregion

        #region Public Functions
        public void ChangeState(UIState state)
        {
            if (state == UIState.None) return;
            _currentState = state;

            if (state == UIState.Show)
            {
                OnShow?.Invoke(new UIEventArgs(this));

                SetActive(true);
                UIBehaviors(state, () =>
                {
                    _currentState = UIState.Default;
                    UIBehaviors(UIState.Default);
                });
            }
            else if (state == UIState.Hide)
            {
                OnHide?.Invoke(new UIEventArgs(this));

                UIBehaviors(state, () =>
                {
                    SetActive(false);
                });
            }
            else
            {
                UIBehaviors(state);
            }
        }
        #endregion

        #region Features
        protected override void SetActive(bool state)
        {
            _groupObject.SetActive(state);
        }

        #endregion

        #region
        private void UIBehaviors(UIState state, Action done = null)
        {
            _stateChangeOperation.Stop(this);

            _positionFeature.SetFeature(state, out float positionFeatureDuration);
            _stateChangeOperation.Add(_positionFeature.Execute(state));

            if (_useFade && (state == UIState.Show || state == UIState.Hide))
            {
                float fadeValue = state == UIState.Show ? 1f : 0f;
                _stateChangeOperation.Add(Fade(_canvasGroup, fadeValue, _fadeDuration, _fadeCurve));
            }

            List<float> featureDurations = new List<float>();
            featureDurations.Add(positionFeatureDuration);

            _stateChangeOperation.Start(this, featureDurations.Max(), done);
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
