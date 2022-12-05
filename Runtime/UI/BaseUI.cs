using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CollieMollie.UI
{
    public abstract class BaseUI : MonoBehaviour
    {
        #region Variable Field
        public event Action<UIEventArgs> OnDefault = null;
        public event Action<UIEventArgs> OnInteractive = null;
        public event Action<UIEventArgs> OnNonInteractive = null;
        public event Action<UIEventArgs> OnShow = null;
        public event Action<UIEventArgs> OnHide = null;

        [Header("Base UI")]
        [SerializeField] protected bool _visible = true;
        public bool IsVisible
        {
            get => _visible;
        }

        [SerializeField] protected bool _interactable = true;
        public bool IsInteractable
        {
            get => _interactable;
        }

        protected UIState _currentState = UIState.None;
        #endregion

        #region Public Functions
        public virtual void ChangeState(UIState state, bool playAudio = true, bool invokeEvent = true)
        {
            if (state == UIState.None) return;
            _currentState = state; _interactable = false;

            switch (state)
            {
                case UIState.Default:
                    _interactable = true;
                    SetActive(true);
                    DefaultBehavior(playAudio, invokeEvent);
                    break;

                case UIState.Interactive:
                    SetActive(true);
                    InteractiveBehavior(playAudio, invokeEvent, () =>
                    {
                        _interactable = true;
                        _currentState = UIState.Default;
                        DefaultBehavior(playAudio, invokeEvent);
                    });
                    break;

                case UIState.NonInteractive:
                    SetActive(true);
                    NonInteractiveBehavior(playAudio, invokeEvent);
                    break;

                case UIState.Show:
                    SetActive(true);
                    ShowBehavior(playAudio, invokeEvent, () =>
                    {
                        _interactable = true;
                        _currentState = UIState.Default;
                        DefaultBehavior(playAudio, invokeEvent);
                    });
                    break;

                case UIState.Hide:
                    HideBehavior(playAudio, invokeEvent, () =>
                    {
                        SetActive(false);
                    });
                    break;
            }
        }

        #endregion

        #region Publishers
        protected void RaiseDefaultEvent(UIEventArgs args) => OnDefault?.Invoke(args);

        protected void RaiseInteractiveEvent(UIEventArgs args) => OnInteractive?.Invoke(args);

        protected void RaiseNonInteractiveEvent(UIEventArgs args) => OnNonInteractive?.Invoke(args);

        protected void RaiseShowEvent(UIEventArgs args) => OnShow?.Invoke(args);

        protected void RaiseHideEvent(UIEventArgs args) => OnHide?.Invoke(args);

        #endregion

        #region Behaviors
        protected virtual void DefaultBehavior(bool playAudio = true, bool invokeEvent = true, Action done = null) { }

        protected virtual void InteractiveBehavior(bool playAudio = true, bool invokeEvent = true, Action done = null) { }

        protected virtual void NonInteractiveBehavior(bool playAudio = true, bool invokeEvent = true, Action done = null) { }

        protected virtual void ShowBehavior(bool playAudio = true, bool invokeEvent = true, Action done = null) { }

        protected virtual void HideBehavior(bool playAudio = true, bool invokeEvent = true, Action done = null) { }

        #endregion

        #region Features
        protected virtual void SetActive(bool state) { }

        #endregion
    }

    public class UIEventArgs
    {
        public BaseUI Sender = null;

        public UIEventArgs() { }

        public UIEventArgs(BaseUI sender)
        {
            this.Sender = sender;
        }

        public bool IsValid()
        {
            return Sender != null;
        }
    }

    public enum UIState { None = -1, Default, Interactive, NonInteractive, Show, Hide }
}
