using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Broccollie.UI
{
    [DefaultExecutionOrder(-120)]
    public class ButtonUI : BaseUI, IDefaultUI, IHoverUI, IPressUI, ISelectUI
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

        protected override void InvokePointerClick(PointerEventData eventData, BaseUI invoker) => Select();

        protected override void InvokeMove(AxisEventData eventData, BaseUI invoker, List<BaseUI> activeList)
        {
            base.InvokeMove(eventData, this, s_activeButtons);
        }

        protected override void InvokeSelect(BaseEventData eventData, BaseUI invoker) => Hover();

        protected override void InvokeDeselect(BaseEventData eventData, BaseUI invoker) => Exit();

        #endregion

        #region Public Functions
        public override void SetVisible(bool state, bool playAudio = true, bool invokeEvent = true)
        {
            CancelFeatureTask();

            if (state)
            {
                _currentState = UIStates.Show;
                _isActive = true;

                if (!gameObject.activeSelf)
                    gameObject.SetActive(true);

                if (invokeEvent)
                    RaiseOnShow(this, new ButtonUIEventArgs());

                _featureTasks = ExecuteFeaturesAsync(UIStates.Show, _cts.Token, playAudio, () =>
                {
                    Default(playAudio, invokeEvent);
                });
            }
            else
            {
                _currentState = UIStates.Hide;
                _isActive = false;

                if (invokeEvent)
                    RaiseOnHide(this, new ButtonUIEventArgs());

                _featureTasks = ExecuteFeaturesAsync(UIStates.Hide, _cts.Token, playAudio, () =>
                {
                    if (gameObject.activeSelf)
                        gameObject.SetActive(false);
                });
            }
        }

        public override void SetVisibleInstant(bool state, bool playAudio = true, bool invokeEvent = true)
        {
            CancelFeatureTask();

            if (state)
            {
                _currentState = UIStates.Show;
                _isActive = true;

                if (!gameObject.activeSelf)
                    gameObject.SetActive(true);

                if (invokeEvent)
                    RaiseOnShow(this, new ButtonUIEventArgs());

                ExecuteFeatureInstant(UIStates.Show, playAudio);
            }
            else
            {
                _currentState = UIStates.Hide;
                _isActive = false;

                if (gameObject.activeSelf)
                    gameObject.SetActive(false);

                if (invokeEvent)
                    RaiseOnHide(this, new ButtonUIEventArgs());

                ExecuteFeatureInstant(UIStates.Hide, playAudio);
            }
        }

        public override void SetInteractive(bool state, bool playAudio = true, bool invokeEvent = true)
        {
            CancelFeatureTask();

            if (state)
            {
                _currentState = UIStates.Interactive;

                if (!gameObject.activeSelf)
                    gameObject.SetActive(true);

                if (invokeEvent)
                    RaiseOnInteractive(this, new ButtonUIEventArgs());

                _featureTasks = ExecuteFeaturesAsync(UIStates.Interactive, _cts.Token, playAudio, () =>
                {
                    _isInteractive = true;
                    Default(playAudio, invokeEvent);
                });
            }
            else
            {
                _currentState = UIStates.NonInteractive;
                _isInteractive = false;

                if (!gameObject.activeSelf)
                    gameObject.SetActive(true);

                if (invokeEvent)
                    RaiseOnInteractive(this, new ButtonUIEventArgs());

                _featureTasks = ExecuteFeaturesAsync(UIStates.NonInteractive, _cts.Token, playAudio);
            }
        }

        public void Default(bool playAudio = true, bool invokeEvent = true)
        {
            if (!_isInteractive) return;

            CancelFeatureTask();

            _currentState = UIStates.Default;
            _isHovered = _isPressed = _isClicked = false;

            if (invokeEvent)
                RaiseOnDefault(this, new ButtonUIEventArgs());

            _featureTasks = ExecuteFeaturesAsync(UIStates.Default, _cts.Token, playAudio);
        }

        public void Hover(bool playAudio = true, bool invokeEvent = true)
        {
            if (!_isInteractive) return;

            CancelFeatureTask();

            _currentState = UIStates.Hover;
            _isHovered = true;

            if (invokeEvent)
                RaiseOnHover(this, new ButtonUIEventArgs());

            _featureTasks = ExecuteFeaturesAsync(UIStates.Hover, _cts.Token, playAudio);
        }

        public void Press(PointerEventData eventData, bool playAudio = true, bool invokeEvent = true)
        {
            if (!_isInteractive) return;

            CancelFeatureTask();

            EventSystem.current.SetSelectedGameObject(gameObject, eventData);
            _currentState = UIStates.Press;
            _isPressed = true;

            if (invokeEvent)
                RaiseOnPress(this, new ButtonUIEventArgs());

            _featureTasks = ExecuteFeaturesAsync(UIStates.Press, _cts.Token, playAudio);
        }

        public void Select(bool playAudio = true, bool invokeEvent = true)
        {
            if (!_isInteractive) return;

            switch (_buttonType)
            {
                case ButtonTypes.Button:
                    if (invokeEvent)
                        RaiseOnClick(this, new ButtonUIEventArgs());
                    break;

                case ButtonTypes.Checkbox:
                    _isClicked = !_isClicked;
                    if (_isClicked)
                    {
                        CancelFeatureTask();

                        _currentState = UIStates.Click;

                        if (invokeEvent)
                            RaiseOnClick(this, new ButtonUIEventArgs());

                        _featureTasks = ExecuteFeaturesAsync(UIStates.Click, _cts.Token, playAudio);
                    }
                    else
                    {
                        CancelFeatureTask();

                        _currentState = UIStates.Default;

                        if (invokeEvent)
                            RaiseOnDefault(this, new ButtonUIEventArgs());

                        _featureTasks = ExecuteFeaturesAsync(UIStates.Default, _cts.Token, playAudio);
                    }
                    break;

                case ButtonTypes.Radio:
                    CancelFeatureTask();

                    _currentState = UIStates.Click;
                    _isClicked = true;

                    if (invokeEvent)
                        RaiseOnClick(this, new ButtonUIEventArgs());

                    _featureTasks = ExecuteFeaturesAsync(UIStates.Click, _cts.Token, playAudio);
                    break;
            }
        }

        public void Exit()
        {
            if (!_isInteractive) return;

            CancelFeatureTask();

            _isHovered = false;
            if (_isPressed) return;

            if (_isClicked)
            {
                _currentState = UIStates.Click;
                _featureTasks = ExecuteFeaturesAsync(UIStates.Click, _cts.Token, false);
            }
            else
            {
                _currentState = UIStates.Default;
                _featureTasks = ExecuteFeaturesAsync(UIStates.Default, _cts.Token, false);
            }
        }

        public void Release()
        {
            if (!_isInteractive) return;

            CancelFeatureTask();

            _isPressed = false;
            if (_isHovered)
            {
                _currentState = UIStates.Hover;
                _featureTasks = ExecuteFeaturesAsync(UIStates.Hover, _cts.Token, false);
                return;
            }

            if (_isClicked)
            {
                _currentState = UIStates.Click;
                _featureTasks = ExecuteFeaturesAsync(UIStates.Click, _cts.Token, false);
            }
            else
            {
                _currentState = UIStates.Default;
                _featureTasks = ExecuteFeaturesAsync(UIStates.Default, _cts.Token, false);
            }
        }

        #endregion
    }

    public class ButtonUIEventArgs : EventArgs
    {

    }

    public enum ButtonTypes { Button, Checkbox, Radio }
}
