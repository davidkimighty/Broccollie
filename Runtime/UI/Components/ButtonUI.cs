using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Broccollie.UI
{
    [DefaultExecutionOrder(-120)]
    public class ButtonUI : BaseUI, IDefaultUI, IHoverUI, IPressUI, IClickUI
    {
        #region Variable Field
        private static List<BaseUI> s_activeButtons = new List<BaseUI>();

        [Header("Button")]
        [SerializeField] private ButtonTypes _buttonType = ButtonTypes.Button;

        #endregion

        private void OnEnable()
        {
            s_activeButtons.Add(this);
        }

        private void OnDisable()
        {

            s_activeButtons.Remove(this);
        }

        #region Pointer Callback Subscribers
        protected override void InvokePointerEnter(PointerEventData eventData, BaseUI invoker) => Hover();

        protected override void InvokePointerExit(PointerEventData eventData, BaseUI invoker) => Exit();

        protected override void InvokePointerDown(PointerEventData eventData, BaseUI invoker) => Press(eventData);

        protected override void InvokePointerUp(PointerEventData eventData, BaseUI invoker) => Release();

        protected override void InvokePointerClick(PointerEventData eventData, BaseUI invoker) => Click();

        protected override void InvokeMove(AxisEventData eventData, BaseUI invoker, List<BaseUI> activeList)
        {
            base.InvokeMove(eventData, this, s_activeButtons);
        }

        protected override void InvokeSelect(BaseEventData eventData, BaseUI invoker) => Hover();

        protected override void InvokeDeselect(BaseEventData eventData, BaseUI invoker) => Exit();

        protected override void InvokeSubmit(BaseEventData eventData, BaseUI invoker)
        {
            Click();
            ButtonSubmitVisuals();

            void ButtonSubmitVisuals()
            {
                if (!_isInteractive) return;
                if (_isInteractive && _buttonType == ButtonTypes.Button)
                {
                    _featureTasks = ExecuteFeaturesAsync(UIStates.Press, true, () =>
                    {
                        Default(false, true);
                    });
                }
            }
        }

        #endregion

        #region Public Functions
        public override void SetVisible(bool state, bool playAudio = true, bool invokeEvent = true, bool instant = false)
        {
            if (state)
            {
                _currentState = UIStates.Show;
                _isActive = true;

                if (!gameObject.activeSelf)
                    gameObject.SetActive(true);

                if (invokeEvent)
                    RaiseOnShow(this, null);

                if (instant)
                {
                    ExecuteFeatureInstant(UIStates.Show, playAudio);
                }
                else
                {
                    _featureTasks = ExecuteFeaturesAsync(UIStates.Show, playAudio, () =>
                    {
                        Default(playAudio, invokeEvent);
                    });
                }
            }
            else
            {
                _currentState = UIStates.Hide;
                _isActive = false;

                if (invokeEvent)
                    RaiseOnHide(this, null);

                if (instant)
                {
                    if (gameObject.activeSelf)
                        gameObject.SetActive(false);

                    ExecuteFeatureInstant(UIStates.Hide, playAudio);
                }
                else
                {
                    _featureTasks = ExecuteFeaturesAsync(UIStates.Hide, playAudio, () =>
                    {
                        if (gameObject.activeSelf)
                            gameObject.SetActive(false);
                    });
                }
            }
        }

        public override void SetInteractive(bool state, bool playAudio = true, bool invokeEvent = true, bool instant = false)
        {
            if (state)
            {
                _currentState = UIStates.Interactive;

                if (!gameObject.activeSelf)
                    gameObject.SetActive(true);

                if (invokeEvent)
                    RaiseOnInteractive(this, null);

                if (instant)
                {

                }
                else
                {
                    _featureTasks = ExecuteFeaturesAsync(UIStates.Interactive, playAudio, () =>
                    {
                        _isInteractive = true;
                        Default(playAudio, invokeEvent);
                    });
                }
            }
            else
            {
                _currentState = UIStates.NonInteractive;
                _isInteractive = false;

                if (!gameObject.activeSelf)
                    gameObject.SetActive(true);

                if (invokeEvent)
                    RaiseOnInteractive(this, null);

                if (instant)
                {

                }
                else
                {
                    _featureTasks = ExecuteFeaturesAsync(UIStates.NonInteractive, playAudio);
                }
            }
        }

        public void Default(bool playAudio = true, bool invokeEvent = true)
        {
            if (!_isInteractive) return;

            _currentState = UIStates.Default;
            _isHovered = _isPressed = _isClicked = false;

            if (invokeEvent)
                RaiseOnDefault(this, null);

            _featureTasks = ExecuteFeaturesAsync(UIStates.Default, playAudio);
        }

        public void Hover(bool playAudio = true, bool invokeEvent = true)
        {
            if (!_isInteractive) return;

            _currentState = UIStates.Hover;
            _isHovered = true;

            if (invokeEvent)
                RaiseOnHover(this, null);

            _featureTasks = ExecuteFeaturesAsync(UIStates.Hover, playAudio);
        }

        public void Press(PointerEventData eventData, bool playAudio = true, bool invokeEvent = true)
        {
            if (!_isInteractive) return;

            if (eventData != null)
                EventSystem.current.SetSelectedGameObject(gameObject, eventData);

            _currentState = UIStates.Press;
            _isPressed = true;

            if (invokeEvent)
                RaiseOnPress(this, null);

            _featureTasks = ExecuteFeaturesAsync(UIStates.Press, playAudio);
        }

        public void Click(bool playAudio = true, bool invokeEvent = true)
        {
            if (!_isInteractive) return;

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
                        _currentState = UIStates.Click;

                        if (invokeEvent)
                            RaiseOnClick(this, null);

                        _featureTasks = ExecuteFeaturesAsync(UIStates.Click, playAudio);
                    }
                    else
                    {
                        _currentState = UIStates.Default;

                        if (invokeEvent)
                            RaiseOnDefault(this, null);

                        _featureTasks = ExecuteFeaturesAsync(UIStates.Default, playAudio);
                    }
                    break;

                case ButtonTypes.Radio:
                    _currentState = UIStates.Click;
                    _isClicked = true;

                    if (invokeEvent)
                        RaiseOnClick(this, null);

                    _featureTasks = ExecuteFeaturesAsync(UIStates.Click, playAudio);
                    break;
            }
        }

        public void Exit()
        {
            if (!_isInteractive) return;

            _isHovered = false;
            if (_isPressed) return;

            if (_isClicked)
            {
                _currentState = UIStates.Click;
                _featureTasks = ExecuteFeaturesAsync(UIStates.Click, false);
            }
            else
            {
                _currentState = UIStates.Default;
                _featureTasks = ExecuteFeaturesAsync(UIStates.Default, false);
            }
        }

        public void Release()
        {
            if (!_isInteractive) return;

            _isPressed = false;
            if (_isHovered)
            {
                _currentState = UIStates.Hover;
                _featureTasks = ExecuteFeaturesAsync(UIStates.Hover, false);
                return;
            }

            if (_isClicked)
            {
                _currentState = UIStates.Click;
                _featureTasks = ExecuteFeaturesAsync(UIStates.Click, false);
            }
            else
            {
                _currentState = UIStates.Default;
                _featureTasks = ExecuteFeaturesAsync(UIStates.Default, false);
            }
        }

        #endregion
    }

    public class ButtonUIEventArgs : EventArgs
    {

    }

    public enum ButtonTypes { Button, Checkbox, Radio }
}
