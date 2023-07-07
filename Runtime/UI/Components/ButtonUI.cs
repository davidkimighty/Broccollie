using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Broccollie.UI
{
    [DefaultExecutionOrder(-120)]
    public class ButtonUI : BaseUI
    {
        private static List<BaseUI> s_activeButtons = new List<BaseUI>();

        [Header("Button")]
        [SerializeField] private ButtonTypes _buttonType = ButtonTypes.Button;

        private void OnEnable()
        {
            s_activeButtons.Add(this);
        }

        private void OnDisable()
        {
            s_activeButtons.Remove(this);
        }

        #region Public Functions
        public override void ChangeState(string state, bool instant = false, bool playAudio = true, bool invokeEvent = true)
        {
            if (Enum.TryParse(state, out UIStates uiState))
            {
                switch (uiState)
                {
                    case UIStates.Default:
                        Default(playAudio, invokeEvent);
                        break;

                    case UIStates.Interactive:
                        Interactive(instant, playAudio, invokeEvent);
                        break;

                    case UIStates.NonInteractive:
                        NonInteractive(instant, playAudio, invokeEvent);
                        break;

                    case UIStates.Show:
                        Show(instant, playAudio, invokeEvent);
                        break;

                    case UIStates.Hide:
                        Hide(instant, playAudio, invokeEvent);
                        break;

                    case UIStates.Hover:
                        Hover(playAudio, invokeEvent);
                        break;

                    case UIStates.Press:
                        Press(null, playAudio, invokeEvent);
                        break;

                    case UIStates.Click:
                        Click(playAudio, invokeEvent);
                        break;
                }
            }
            else
                CustomState(state, playAudio, invokeEvent);
        }

        public override void SetActive(bool state)
        {
            gameObject.SetActive(state);
        }

        #endregion

        #region Pointer Callback Subscribers
        protected override void InvokePointerEnter(PointerEventData eventData, BaseUI invoker) => Hover(true, true);

        protected override void InvokePointerExit(PointerEventData eventData, BaseUI invoker) => Exit();

        protected override void InvokePointerDown(PointerEventData eventData, BaseUI invoker) => Press(eventData, true, true);

        protected override void InvokePointerUp(PointerEventData eventData, BaseUI invoker) => Release();

        protected override void InvokePointerClick(PointerEventData eventData, BaseUI invoker) => Click(true, true);

        protected override void InvokeMove(AxisEventData eventData, BaseUI invoker, List<BaseUI> activeList)
        {
            base.InvokeMove(eventData, this, s_activeButtons);
        }

        protected override void InvokeSelect(BaseEventData eventData, BaseUI invoker) => Hover(true, true);

        protected override void InvokeDeselect(BaseEventData eventData, BaseUI invoker) => Exit();

        protected override void InvokeSubmit(BaseEventData eventData, BaseUI invoker)
        {
            Click(true, true);

            if (!_isRaycastIgnored)
                ButtonSubmitVisuals();

            void ButtonSubmitVisuals()
            {
                if (!_isInteractive) return;
                if (_isInteractive && _buttonType == ButtonTypes.Button)
                {
                    _featureTasks = ExecuteFeaturesAsync(UIStates.Press.ToString(), true, () =>
                    {
                        Default(false, true);
                    });
                }
            }
        }

        #endregion

        private void Default(bool playAudio, bool invokeEvent)
        {
            if (!_isInteractive) return;

            SetCurrentState(UIStates.Default, out string state);
            _isHovered = _isPressed = _isClicked = false;

            if (invokeEvent)
                RaiseOnDefault(this, null);

            _featureTasks = ExecuteFeaturesAsync(state, playAudio);
        }

        private void Interactive(bool instant, bool playAudio, bool invokeEvent)
        {
            SetCurrentState(UIStates.Interactive, out string state);
            SetActive(true);

            if (invokeEvent)
                RaiseOnInteractive(this, null);

            if (instant) { }
            else
            {
                _featureTasks = ExecuteFeaturesAsync(state, playAudio, () =>
                {
                    _isInteractive = true;
                    Default(playAudio, invokeEvent);
                });
            }
        }

        private void NonInteractive(bool instant, bool playAudio, bool invokeEvent)
        {
            SetCurrentState(UIStates.NonInteractive, out string state);
            _isInteractive = false;
            SetActive(true);

            if (invokeEvent)
                RaiseOnInteractive(this, null);

            if (instant) { }
            else
                _featureTasks = ExecuteFeaturesAsync(state, playAudio);
        }

        private void Show(bool instant, bool playAudio, bool invokeEvent)
        {
            SetCurrentState(UIStates.Show, out string state);
            _isActive = true;
            SetActive(true);

            if (invokeEvent)
                RaiseOnShow(this, null);

            if (instant)
                ExecuteFeatureInstant(state, playAudio);
            else
            {
                _featureTasks = ExecuteFeaturesAsync(state, playAudio, () =>
                {
                    Default(playAudio, invokeEvent);
                });
            }
        }

        private void Hide(bool instant, bool playAudio, bool invokeEvent)
        {
            SetCurrentState(UIStates.Hide, out string state);
            _isActive = false;

            if (invokeEvent)
                RaiseOnHide(this, null);

            if (instant)
            {
                ExecuteFeatureInstant(state, playAudio);
                SetActive(false);
            }
            else
            {
                _featureTasks = ExecuteFeaturesAsync(state, playAudio, () =>
                {
                    SetActive(false);
                });
            }
        }

        private void Hover(bool playAudio, bool invokeEvent)
        {
            if (_isRaycastIgnored || !_isInteractive) return;

            SetCurrentState(UIStates.Hover, out string state);
            _isHovered = true;

            if (invokeEvent)
                RaiseOnHover(this, null);

            _featureTasks = ExecuteFeaturesAsync(state, playAudio);
        }

        private void Press(PointerEventData eventData, bool playAudio, bool invokeEvent)
        {
            if (_isRaycastIgnored || !_isInteractive) return;

            if (eventData != null)
                EventSystem.current.SetSelectedGameObject(gameObject, eventData);

            SetCurrentState(UIStates.Press, out string state);
            _isPressed = true;

            if (invokeEvent)
                RaiseOnPress(this, null);

            _featureTasks = ExecuteFeaturesAsync(state, playAudio);
        }

        private void Click(bool playAudio, bool invokeEvent)
        {
            if (_isRaycastIgnored || !_isInteractive) return;

            switch (_buttonType)
            {
                case ButtonTypes.Button:
                    if (invokeEvent)
                        RaiseOnClick(this, null);
                    break;

                case ButtonTypes.Checkbox:
                    _isClicked = !_isClicked;
                    if (_isClicked)
                    {
                        SetCurrentState(UIStates.Click, out string checkboxState);
                        if (invokeEvent)
                            RaiseOnClick(this, null);

                        _featureTasks = ExecuteFeaturesAsync(checkboxState, playAudio);
                    }
                    else
                    {
                        SetCurrentState(UIStates.Default, out string checkboxState);
                        if (invokeEvent)
                            RaiseOnDefault(this, null);

                        _featureTasks = ExecuteFeaturesAsync(checkboxState, playAudio);
                    }
                    break;

                case ButtonTypes.Radio:
                    _isClicked = true;
                    SetCurrentState(UIStates.Click, out string radioState);
                    if (invokeEvent)
                        RaiseOnClick(this, null);

                    _featureTasks = ExecuteFeaturesAsync(radioState, playAudio);
                    break;
            }
        }

        private void Exit()
        {
            if (_isRaycastIgnored || !_isInteractive) return;

            _isHovered = false;
            if (_isPressed) return;

            if (_isClicked)
            {
                SetCurrentState(UIStates.Click, out string state);
                _featureTasks = ExecuteFeaturesAsync(state, false);
            }
            else
            {
                SetCurrentState(UIStates.Default, out string state);
                _featureTasks = ExecuteFeaturesAsync(state, false);
            }
        }

        private void Release()
        {
            if (_isRaycastIgnored || !_isInteractive) return;

            _isPressed = false;
            if (_isHovered)
            {
                SetCurrentState(UIStates.Hover, out string state);
                _featureTasks = ExecuteFeaturesAsync(state, false);
                return;
            }

            if (_isClicked)
            {
                SetCurrentState(UIStates.Click, out string state);
                _featureTasks = ExecuteFeaturesAsync(state, false);
            }
            else
            {
                SetCurrentState(UIStates.Default, out string state);
                _featureTasks = ExecuteFeaturesAsync(state, false);
            }
        }

        private void CustomState(string state, bool playAudio, bool invokeEvent)
        {
            if (_isRaycastIgnored || !_isInteractive) return;

            _currentState = state;
            _isHovered = _isPressed = _isClicked = false;

            if (invokeEvent)
                RaiseOnCustomState(this, null);

            _featureTasks = ExecuteFeaturesAsync(state, playAudio);
        }
    }

    public class ButtonUIEventArgs : EventArgs
    {

    }

    public enum ButtonTypes { Button, Checkbox, Radio }
}
