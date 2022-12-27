using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using CollieMollie.Core;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CollieMollie.UI
{
    [DefaultExecutionOrder(-100)]
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

        private Task _behaviorTask = null;
        private CancellationTokenSource _cancelSource = new CancellationTokenSource();
        #endregion

        #region Behaviors
        protected override void DefaultBehavior(bool playAudio = true, bool invokeEvent = true, Action done = null)
        {
            _currentState = State.Default;

            if (invokeEvent)
                RaiseDefaultEvent(new UIEventArgs(this));

            _cancelSource.Cancel();
            _cancelSource = new CancellationTokenSource();
            _behaviorTask = ExecuteFeaturesAsync(State.Default.ToString(), playAudio, done);
        }

        protected override void InteractiveBehavior(bool playAudio = true, bool invokeEvent = true, Action done = null)
        {
            _currentState = State.Interactive;

            if (invokeEvent)
                RaiseInteractiveEvent(new UIEventArgs(this));

            _cancelSource.Cancel();
            _cancelSource = new CancellationTokenSource();
            _behaviorTask = ExecuteFeaturesAsync(State.Interactive.ToString(), playAudio, done);
        }

        protected override void NonInteractiveBehavior(bool playAudio = true, bool invokeEvent = true, Action done = null)
        {
            _currentState = State.NonInteractive;

            if (invokeEvent)
                RaiseNonInteractiveEvent(new UIEventArgs(this));

            _cancelSource.Cancel();
            _cancelSource = new CancellationTokenSource();
            _behaviorTask = ExecuteFeaturesAsync(State.NonInteractive.ToString(), playAudio, done);
        }

        protected override void ShowBehavior(bool playAudio = true, bool invokeEvent = true, Action done = null)
        {
            _currentState = State.Show;

            if (invokeEvent)
                RaiseShowEvent(new UIEventArgs(this));

            _cancelSource.Cancel();
            _cancelSource = new CancellationTokenSource();
            _behaviorTask = ExecuteFeaturesAsync(State.Show.ToString(), playAudio, done);
        }

        protected override void HideBehavior(bool playAudio = true, bool invokeEvent = true, Action done = null)
        {
            _currentState = State.Hide;

            if (invokeEvent)
                RaiseHideEvent(new UIEventArgs(this));

            _cancelSource.Cancel();
            _cancelSource = new CancellationTokenSource();
            _behaviorTask = ExecuteFeaturesAsync(State.Hide.ToString(), playAudio, done);
        }

        protected override void SelectedBehavior(bool playAudio = true, bool invokeEvent = true, Action done = null)
        {
            _currentState = State.Selected;

            if (invokeEvent)
                RaiseSelectedEvent(new UIEventArgs(this));

            _cancelSource.Cancel();
            _cancelSource = new CancellationTokenSource();
            _behaviorTask = ExecuteFeaturesAsync(State.Selected.ToString(), playAudio);
        }

        #endregion

        #region Features
        protected override void SetActive(bool state)
        {
            _popupObject.SetActive(state);
        }

        private async Task ExecuteFeaturesAsync(string state, bool playAudio, Action done = null)
        {
            List<Task> featureTasks = new List<Task>();
            if (_colorFeature != null)
                featureTasks.Add(_colorFeature.ExecuteAsync(state, _cancelSource));

            if (_spriteFeature != null)
                featureTasks.Add(_spriteFeature.ExecuteAsync(state, _cancelSource));

            if (_transformFeature != null)
                featureTasks.Add(_transformFeature.ExecuteAsync(state, _cancelSource));

            if (_animationFeature != null)
                featureTasks.Add(_animationFeature.ExecuteAsync(state, _cancelSource));

            if (_audioFeature != null && playAudio)
                featureTasks.Add(_audioFeature.ExecuteAsync(state, _cancelSource));

            await Task.WhenAll(featureTasks);
            done?.Invoke();
        }

        #endregion
    }
}
