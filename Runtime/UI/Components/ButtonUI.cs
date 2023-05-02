using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Broccollie.Core;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Broccollie.UI
{
    [DefaultExecutionOrder(-120)]
    public class ButtonUI : BaselineUI, IDefaultUI, IHoverUI, IPressUI, ISelectUI
    {
        #region Variable Field
        [Header("Button")]
        [SerializeField] private ButtonTypes _buttonType = ButtonTypes.Button;

        #endregion

        #region Pointer Callback Subscribers
        protected override void InvokePointerEnter(PointerEventData eventData, BaselineUI baselineUI) => Hover();

        protected override void InvokePointerExit(PointerEventData eventData, BaselineUI baselineUI)
        {
            if (!_isInteractive) return;

            CancelFeatureTask();

            _isHovered = false;
            if (_isPressed) return;

            if (_isSelected)
            {
                _currentState = UIStates.Select;
                _featureTasks = ExecuteFeaturesAsync(UIStates.Select, _cts.Token, false);
            }
            else
            {
                _currentState = UIStates.Default;
                _featureTasks = ExecuteFeaturesAsync(UIStates.Default, _cts.Token, false);
            }
        }

        protected override void InvokePointerDown(PointerEventData eventData, BaselineUI baselineUI) => Press();

        protected override void InvokePointerUp(PointerEventData eventData, BaselineUI baselineUI)
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

            if (_isSelected)
            {
                _currentState = UIStates.Select;
                _featureTasks = ExecuteFeaturesAsync(UIStates.Select, _cts.Token, false);
            }
            else
            {
                _currentState = UIStates.Default;
                _featureTasks = ExecuteFeaturesAsync(UIStates.Default, _cts.Token, false);
            }
        }

        protected override void InvokePointerClick(PointerEventData eventData, BaselineUI baselineUI) => Select();

        protected override void InvokeMove(AxisEventData eventData, BaselineUI baselineUI)
        {
            Debug.Log($"Move {eventData.moveDir}");
        }

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
            _isHovered = _isPressed = _isSelected = false;

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

        public void Press(bool playAudio = true, bool invokeEvent = true)
        {
            if (!_isInteractive) return;

            CancelFeatureTask();

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
                        RaiseOnSelect(this, new ButtonUIEventArgs());
                    break;

                case ButtonTypes.Checkbox:
                    _isSelected = !_isSelected;
                    if (_isSelected)
                    {
                        CancelFeatureTask();

                        _currentState = UIStates.Select;

                        if (invokeEvent)
                            RaiseOnSelect(this, new ButtonUIEventArgs());

                        _featureTasks = ExecuteFeaturesAsync(UIStates.Select, _cts.Token, playAudio);
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

                    _currentState = UIStates.Select;
                    _isSelected = true;

                    if (invokeEvent)
                        RaiseOnSelect(this, new ButtonUIEventArgs());

                    _featureTasks = ExecuteFeaturesAsync(UIStates.Select, _cts.Token, playAudio);
                    break;
            }
        }

        #endregion
    }

    public class ButtonUIEventArgs : EventArgs
    {

    }

    public enum ButtonTypes { Button, Checkbox, Radio }
}
