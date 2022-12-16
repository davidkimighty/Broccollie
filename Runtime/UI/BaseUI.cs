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

        protected State _currentState = State.None;
        #endregion

        #region Public Functions
        public virtual void ChangeState(State state, bool playAudio = true, bool invokeEvent = true)
        {
            if (state == State.None || state == _currentState) return;

            switch (state)
            {
                case State.Default:
                    _interactable = true;
                    DefaultBehavior(playAudio, invokeEvent);
                    break;

                case State.Interactive:
                    _interactable = false;
                    InteractiveBehavior(playAudio, invokeEvent, () =>
                    {
                        _interactable = true;
                        DefaultBehavior(playAudio, invokeEvent);
                    });
                    break;

                case State.NonInteractive:
                    _interactable = false;
                    NonInteractiveBehavior(playAudio, invokeEvent);
                    break;

                case State.Show:
                    _visible = true; _interactable = false;
                    SetActive(true);
                    ShowBehavior(playAudio, invokeEvent, () =>
                    {
                        _interactable = true;
                        DefaultBehavior(playAudio, invokeEvent);
                    });
                    break;

                case State.Hide:
                    HideBehavior(playAudio, invokeEvent, () =>
                    {
                        _visible = false;
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

        public enum State { None = -1, Default, Hovered, Pressed, Selected, Interactive, NonInteractive, Show, Hide }
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
}
