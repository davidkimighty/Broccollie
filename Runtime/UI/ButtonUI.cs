using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Broccollie.Core;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Broccollie.UI
{
    [DefaultExecutionOrder(-100)]
    public class ButtonUI : BaselineUI, IDefaultUI, IHoverUI, IPressUI, ISelectUI
    {
        #region Variable Field
        [Header("Button")]
        [SerializeField] private ButtonTypes _buttonType = ButtonTypes.Button;

        [Header("Features")]
        [SerializeField] private UIColorFeature _colorFeature = null;
        [SerializeField] private UISpriteFeature _spriteFeature = null;
        [SerializeField] private UITransformFeature _transformFeature = null;
        [SerializeField] private UIAudioFeature _audioFeature = null;

        private Task _featureTasks = null;

        #endregion

        #region Public Functions
        public override void SetActive(bool state, bool playAudio = false, bool invokeEvent = true)
        {
            if (state)
            {
                if (_currentState == UIStates.Show) return;
                _currentState = UIStates.Show;

                gameObject.SetActive(true);
                if (invokeEvent)
                {
                    RaiseOnShow();
                    Task.Run(() => RaiseOnShowAsync());
                }

                _featureTasks = ExecuteFeaturesAsync(UIStates.Show, playAudio, () =>
                {
                    _isInteractive = true;
                    _featureTasks = ExecuteFeaturesAsync(UIStates.Default);
                });
            }
            else
            {
                if (_currentState == UIStates.Hide) return;
                _currentState = UIStates.Hide;

                _isInteractive = false;
                if (invokeEvent)
                {
                    RaiseOnHide();
                    Task.Run(() => RaiseOnHideAsync());
                }

                _featureTasks = ExecuteFeaturesAsync(UIStates.Hide, playAudio, () =>
                {
                    gameObject.SetActive(false);
                });
            }
        }

        public override void SetInteractive(bool state, bool playAudio = false, bool invokeEvent = true)
        {
            if (state)
            {
                if (_currentState == UIStates.Interactive) return;
                _currentState = UIStates.Interactive;

                if (!gameObject.activeSelf)
                    gameObject.SetActive(true);

                if (invokeEvent)
                {
                    RaiseOnInteractive();
                    Task.Run(() => RaiseOnInteractiveAsync());
                }

                _featureTasks = ExecuteFeaturesAsync(UIStates.Interactive, playAudio, () =>
                {
                    _isInteractive = true;
                });
            }
            else
            {
                if (_currentState == UIStates.NonInteractive) return;
                _currentState = UIStates.NonInteractive;

                if (!gameObject.activeSelf)
                    gameObject.SetActive(true);

                _isInteractive = false;
                if (invokeEvent)
                {
                    RaiseOnInteractive();
                    Task.Run(() => RaiseOnInteractiveAsync());
                }
                _featureTasks = ExecuteFeaturesAsync(UIStates.NonInteractive, playAudio);
            }
        }

        public void Default(bool playAudio = false, bool invokeEvent = true)
        {
            if (!_isInteractive || _currentState == UIStates.Default) return;
            _currentState = UIStates.Default;

            _isHovered = _isPressed = _isSelected = false;
            if (invokeEvent)
            {
                RaiseOnDefault();
                Task.Run(() => RaiseOnDefaultAsync());
            }
            _featureTasks = ExecuteFeaturesAsync(UIStates.Default, playAudio);
        }

        public void Hover(bool playAudio = false, bool invokeEvent = true)
        {
            if (!_isInteractive || _currentState == UIStates.Hover) return;
            _currentState = UIStates.Hover;

            _isHovered = true;
            if (invokeEvent)
            {
                RaiseOnHover();
                Task.Run(() => RaiseOnHoverAsync());
            }
            _featureTasks = ExecuteFeaturesAsync(UIStates.Hover, playAudio);
        }

        public void Press(bool playAudio = false, bool invokeEvent = true)
        {
            if (!_isInteractive || _currentState == UIStates.Press) return;
            _currentState = UIStates.Press;

            _isPressed = true;
            if (invokeEvent)
            {
                RaiseOnPress();
                Task.Run(() => RaiseOnPressAsync());
            }
            _featureTasks = ExecuteFeaturesAsync(UIStates.Press, playAudio);
        }

        public void Select(bool playAudio = false, bool invokeEvent = true)
        {
            if (!_isInteractive) return;
           
            switch (_buttonType)
            {
                case ButtonTypes.Button:
                    if (invokeEvent)
                    {
                        RaiseOnSelect();
                        Task.Run(() => RaiseOnSelectAsync());
                    }
                    break;

                case ButtonTypes.Checkbox:
                    _isSelected = !_isSelected;
                    if (_isSelected)
                    {
                        _currentState = UIStates.Select;
                        if (invokeEvent)
                        {
                            RaiseOnSelect();
                            Task.Run(() => RaiseOnSelectAsync());
                        }
                        _featureTasks = ExecuteFeaturesAsync(UIStates.Select, playAudio);
                    }
                    else
                    {
                        _currentState = UIStates.Default;
                        if (invokeEvent)
                        {
                            RaiseOnDefault();
                            Task.Run(() => RaiseOnDefaultAsync());
                        }
                        _featureTasks = ExecuteFeaturesAsync(UIStates.Default, playAudio);
                    }
                    break;

                case ButtonTypes.Radio:
                    if (_currentState == UIStates.Select) break;
                    _currentState = UIStates.Select;

                    _isSelected = true;
                    if (invokeEvent)
                    {
                        RaiseOnSelect();
                        Task.Run(() => RaiseOnSelectAsync());
                    }
                    _featureTasks = ExecuteFeaturesAsync(UIStates.Select, playAudio);
                    break;
            }
        }

        public void Cancel()
        {
            if (_isSelected)
            {
                Select(false);
            }
            else
            {
                if (_isHovered)
                    Hover(false);
                else
                    Default(false);
            }
        }

        #endregion

        private void Awake()
        {
            switch (_currentState)
            {
                case UIStates.Show:
                    SetActive(true, false, false);
                    break;

                case UIStates.Hide:
                    SetActive(false, false, false);
                    break;

                case UIStates.Interactive:
                    SetInteractive(true, false, false);
                    break;

                case UIStates.NonInteractive:
                    SetInteractive(false, false, false);
                    break;

                case UIStates.Default:
                    Default(false, false);
                    break;

                case UIStates.Hover:
                    Hover(false, false);
                    break;

                case UIStates.Press:
                    Press(false, false);
                    break;

                case UIStates.Select:
                    Select(false, false);
                    break;
            }
        }

        #region Pointer Callback Subscribers
        protected override void InvokePointerEnter(PointerEventData eventData, BaselineUI baselineUI) => Hover();

        protected override void InvokePointerExit(PointerEventData eventData, BaselineUI baselineUI)
        {
            _isHovered = false;
            Cancel();
        }

        protected override void InvokePointerDown(PointerEventData eventData, BaselineUI baselineUI) => Press();

        protected override void InvokePointerUp(PointerEventData eventData, BaselineUI baselineUI)
        {
            _isPressed = false;
            if (_isHovered)
                Cancel();
        }

        protected override void InvokePointerClick(PointerEventData eventData, BaselineUI baselineUI) => Select();

        #endregion

        #region Private Functions
        private async Task ExecuteFeaturesAsync(UIStates state, bool playAudio = true, Action done = null)
        {
            List<Task> featureTasks = new List<Task>();
            if (_colorFeature != null)
                featureTasks.Add(_colorFeature.ExecuteFeaturesAsync(state));

            if (_spriteFeature != null)
                featureTasks.Add(_spriteFeature.ExecuteFeaturesAsync(state));

            if (_transformFeature != null)
                featureTasks.Add(_transformFeature.ExecuteFeaturesAsync(state));

            if (_audioFeature != null && playAudio)
                featureTasks.Add(_audioFeature.ExecuteFeaturesAsync(state));

            await Task.WhenAll(featureTasks);
            done?.Invoke();
        }

        #endregion
    }

    public enum ButtonTypes { Button, Checkbox, Radio }
}
