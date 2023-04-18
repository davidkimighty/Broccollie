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

        private Task _featureTasks = null;
        private CancellationTokenSource _cts = new CancellationTokenSource();

        #endregion

        #region Pointer Callback Subscribers
        protected override void InvokePointerEnter(PointerEventData eventData, BaselineUI baselineUI) => Hover();

        protected override void InvokePointerExit(PointerEventData eventData, BaselineUI baselineUI)
        {
            if (!_isInteractive) return;

            _isHovered = false;
            if (_isPressed) return;

            if (_isSelected)
            {
                _currentState = UIStates.Select;
                _featureTasks = ExecuteFeaturesAsync(UIStates.Select, false);
            }
            else
            {
                _currentState = UIStates.Default;
                _featureTasks = ExecuteFeaturesAsync(UIStates.Default, false);
            }
        }

        protected override void InvokePointerDown(PointerEventData eventData, BaselineUI baselineUI) => Press();

        protected override void InvokePointerUp(PointerEventData eventData, BaselineUI baselineUI)
        {
            if (!_isInteractive) return;

            _isPressed = false;
            if (_isHovered)
            {
                _currentState = UIStates.Hover;
                _featureTasks = ExecuteFeaturesAsync(UIStates.Hover, false);
                return;
            }

            if (_isSelected)
            {
                _currentState = UIStates.Select;
                _featureTasks = ExecuteFeaturesAsync(UIStates.Select, false);
            }
            else
            {
                _currentState = UIStates.Default;
                _featureTasks = ExecuteFeaturesAsync(UIStates.Default, false);
            }
        }

        protected override void InvokePointerClick(PointerEventData eventData, BaselineUI baselineUI) => Select();

        #endregion

        #region Public Functions
        public override void SetVisible(bool state, bool playAudio = true, bool invokeEvent = true)
        {
            _cts.Cancel();
            _cts = new CancellationTokenSource();

            if (state)
            {
                _currentState = UIStates.Show;
                _isActive = true;
                gameObject.SetActive(true);

                if (invokeEvent)
                    RaiseOnShow(this, new ButtonUIEventArgs());

                _featureTasks = ExecuteFeaturesAsync(UIStates.Show, playAudio, _cts, () =>
                {
                    _isInteractive = true;
                    Default(playAudio, invokeEvent);
                });
            }
            else
            {
                _currentState = UIStates.Hide;
                _isActive = _isInteractive = false;

                if (invokeEvent)
                    RaiseOnHide(this, new ButtonUIEventArgs());

                _featureTasks = ExecuteFeaturesAsync(UIStates.Hide, playAudio, _cts, () =>
                {
                    gameObject.SetActive(false);
                });
            }
        }

        public override void SetVisibleInstant(bool state, bool playAudio = true, bool invokeEvent = true)
        {
            _cts.Cancel();
            _cts = new CancellationTokenSource();

            if (state)
            {
                _currentState = UIStates.Default;
                _isActive = true;
                gameObject.SetActive(true);

                if (invokeEvent)
                    RaiseOnShow(this, new ButtonUIEventArgs());

                ExecuteFeatureInstant(UIStates.Default, playAudio);
            }
            else
            {
                _currentState = UIStates.Hide;
                _isActive = _isInteractive = false;
                gameObject.SetActive(false);

                if (invokeEvent)
                    RaiseOnHide(this, new ButtonUIEventArgs());

                ExecuteFeatureInstant(UIStates.Hide, playAudio);
            }
        }

        public override void SetInteractive(bool state, bool playAudio = true, bool invokeEvent = true)
        {
            _cts.Cancel();
            _cts = new CancellationTokenSource();

            if (state)
            {
                _currentState = UIStates.Interactive;
                if (!gameObject.activeSelf)
                    gameObject.SetActive(true);

                if (invokeEvent)
                    RaiseOnInteractive(this, new ButtonUIEventArgs());

                _featureTasks = ExecuteFeaturesAsync(UIStates.Interactive, playAudio, _cts, () =>
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

                _featureTasks = ExecuteFeaturesAsync(UIStates.NonInteractive, playAudio, _cts);
            }
        }

        public void Default(bool playAudio = true, bool invokeEvent = true)
        {
            if (!_isInteractive) return;

            _cts.Cancel();
            _cts = new CancellationTokenSource();

            _currentState = UIStates.Default;
            _isHovered = _isPressed = _isSelected = false;
            if (invokeEvent)
                RaiseOnDefault(this, new ButtonUIEventArgs());

            _featureTasks = ExecuteFeaturesAsync(UIStates.Default, playAudio, _cts);
        }

        public void Hover(bool playAudio = true, bool invokeEvent = true)
        {
            if (!_isInteractive) return;

            _cts.Cancel();
            _cts = new CancellationTokenSource();

            _currentState = UIStates.Hover;
            _isHovered = true;
            if (invokeEvent)
                RaiseOnHover(this, new ButtonUIEventArgs());

            _featureTasks = ExecuteFeaturesAsync(UIStates.Hover, playAudio, _cts);
        }

        public void Press(bool playAudio = true, bool invokeEvent = true)
        {
            if (!_isInteractive) return;

            _cts.Cancel();
            _cts = new CancellationTokenSource();

            _currentState = UIStates.Press;
            _isPressed = true;
            if (invokeEvent)
                RaiseOnPress(this, new ButtonUIEventArgs());

            _featureTasks = ExecuteFeaturesAsync(UIStates.Press, playAudio, _cts);
        }

        public void Select(bool playAudio = true, bool invokeEvent = true)
        {
            if (!_isInteractive) return;

            _cts.Cancel();
            _cts = new CancellationTokenSource();

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
                        _currentState = UIStates.Select;
                        if (invokeEvent)
                            RaiseOnSelect(this, new ButtonUIEventArgs());

                        _featureTasks = ExecuteFeaturesAsync(UIStates.Select, playAudio, _cts);
                    }
                    else
                    {
                        _currentState = UIStates.Default;
                        if (invokeEvent)
                            RaiseOnDefault(this, new ButtonUIEventArgs());

                        _featureTasks = ExecuteFeaturesAsync(UIStates.Default, playAudio, _cts);
                    }
                    break;

                case ButtonTypes.Radio:
                    _currentState = UIStates.Select;
                    _isSelected = true;
                    if (invokeEvent)
                        RaiseOnSelect(this, new ButtonUIEventArgs());

                    _featureTasks = ExecuteFeaturesAsync(UIStates.Select, playAudio, _cts);
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