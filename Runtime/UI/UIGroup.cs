using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CollieMollie.Core;
using UnityEngine;

namespace CollieMollie.UI
{
    [DefaultExecutionOrder(-100)]
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

        private Task _behaviorTask = null;
        private readonly CancellationTokenSource _cancelSource = new CancellationTokenSource();
        #endregion

        #region Behaviors
        protected override void ShowBehavior(bool playAudio = true, bool invokeEvent = true, Action done = null)
        {
            _currentState = BaseUI.State.Show;

            if (invokeEvent)
                RaiseShowEvent(new UIEventArgs(this));

            _behaviorTask = ExecuteFeaturesAsync(State.Show.ToString(), playAudio, done);
        }

        protected override void HideBehavior(bool playAudio = true, bool invokeEvent = true, Action done = null)
        {
            _currentState = BaseUI.State.Hide;

            if (invokeEvent)
                RaiseHideEvent(new UIEventArgs(this));

            _behaviorTask = ExecuteFeaturesAsync(State.Hide.ToString(), playAudio, done);
        }

        #endregion

        #region Features
        protected override void SetActive(bool state)
        {
            _groupObject.SetActive(state);
        }

        private async Task ExecuteFeaturesAsync(string state, bool playAudio, Action done = null)
        {
            List<Task> featureTasks = new List<Task>();
            if (_transformFeature != null)
                featureTasks.Add(_transformFeature.ExecuteAsync(state));

            if (_useFade)
            {
                float targetValue = state == State.Show.ToString() ? 1 : 0;
                featureTasks.Add(Fade(_canvasGroup, targetValue, _fadeDuration, _fadeCurve));
            }

            await Task.WhenAll(featureTasks);
            done?.Invoke();
        }

        private async Task Fade(CanvasGroup group, float targetValue, float duration, AnimationCurve curve)
        {
            float elapsedTime = 0f;
            float startValue = group.alpha;

            while (elapsedTime < duration)
            {
                group.alpha = Mathf.Lerp(startValue, targetValue, curve.Evaluate(elapsedTime / duration));
                elapsedTime += Time.deltaTime;
                await Task.Yield();
            }
            group.alpha = targetValue;
        }

        #endregion
    }
}
