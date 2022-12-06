using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using CollieMollie.Core;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CollieMollie.UI
{
    public class UIPanel : InteractableUI
    {
        #region Variable Field
        [Header("Panel")]
        // [SerializeField] private Canvas _canvas = null;
        [SerializeField] private GameObject _popupObject = null;

        [SerializeField] private UIColorFeature _colorFeature = null;
        [SerializeField] private UISpriteFeature _spriteFeature = null;
        [SerializeField] private UITransformFeature _transformFeature = null;
        [SerializeField] private UIAnimationFeature _animationFeature = null;
        [SerializeField] private UIAudioFeature _audioFeature = null;

        private IEnumerator _behaviorAction = null;

        #endregion

        #region Behaviors
        protected override void DefaultBehavior(bool playAudio = true, bool invokeEvent = true, Action done = null)
        {
            _currentState = UIState.Default;
            _currentInteractionState = UIInteractionState.None;

            if (invokeEvent)
                RaiseDefaultEvent(new UIEventArgs(this));

            RunAction(ExecuteFeatures(UIState.Default.ToString(), playAudio, done));
        }

        protected override void InteractiveBehavior(bool playAudio = true, bool invokeEvent = true, Action done = null)
        {
            _currentState = UIState.Interactive;

            if (invokeEvent)
                RaiseInteractiveEvent(new UIEventArgs(this));

            RunAction(ExecuteFeatures(UIState.Interactive.ToString(), playAudio, done));
        }

        protected override void NonInteractiveBehavior(bool playAudio = true, bool invokeEvent = true, Action done = null)
        {
            _currentState = UIState.NonInteractive;

            if (invokeEvent)
                RaiseNonInteractiveEvent(new UIEventArgs(this));

            RunAction(ExecuteFeatures(UIState.NonInteractive.ToString(), playAudio, done));
        }

        protected override void ShowBehavior(bool playAudio = true, bool invokeEvent = true, Action done = null)
        {
            _currentState = UIState.Show;

            if (invokeEvent)
                RaiseShowEvent(new UIEventArgs(this));

            RunAction(ExecuteFeatures(UIState.Show.ToString(), playAudio, done));
        }

        protected override void HideBehavior(bool playAudio = true, bool invokeEvent = true, Action done = null)
        {
            _currentState = UIState.Hide;

            if (invokeEvent)
                RaiseHideEvent(new UIEventArgs(this));

            RunAction(ExecuteFeatures(UIState.Hide.ToString(), playAudio, done));
        }

        protected override void SelectedBehavior(bool playAudio = true, bool invokeEvent = true)
        {
            _currentState = UIState.None;
            _currentInteractionState = UIInteractionState.Selected;

            if (invokeEvent)
                RaiseSelectedEvent(new UIEventArgs(this));

            RunAction(ExecuteFeatures(UIInteractionState.Selected.ToString(), playAudio));
        }

        #endregion

        #region Features
        protected override void SetActive(bool state)
        {
            _popupObject.SetActive(state);
        }

        private void RunAction(IEnumerator action)
        {
            if (_behaviorAction != null)
                StopCoroutine(_behaviorAction);
            _behaviorAction = action;
            StartCoroutine(_behaviorAction);
        }

        private IEnumerator ExecuteFeatures(string state, bool playAudio, Action done = null)
        {
            List<float> durations = new List<float>();

            if (_colorFeature != null)
            {
                _colorFeature.Execute(state, out float duration);
                durations.Add(duration);
            }

            if (_spriteFeature != null)
            {
                _spriteFeature.Execute(state, out float duration);
                durations.Add(duration);
            }

            if (_transformFeature != null)
            {
                _transformFeature.Execute(state, out float duration);
                durations.Add(duration);
            }

            if (_animationFeature != null)
            {
                _animationFeature.Execute(state, out float duration);
                durations.Add(duration);
            }

            if (_audioFeature != null && playAudio)
            {
                _audioFeature.Execute(state, out float duration);
                durations.Add(duration);
            }

            if (done != null)
            {
                yield return new WaitForSeconds(durations.Count > 0 ? durations.Max() : 0);
                done?.Invoke();
            }
        }

        #endregion
    }
}
