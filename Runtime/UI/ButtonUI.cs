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
            if (!_isInteractive) return;

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
            if (!_isInteractive) return;

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
            if (!_isInteractive) return;

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

        #endregion

        private void Awake()
        {
            switch (_currentState)
            {
                case UIStates.Show:
                    gameObject.SetActive(true);
                    break;

                case UIStates.Hide:
                    gameObject.SetActive(false);
                    break;

                case UIStates.Interactive:
                    _isInteractive = true;
                    ExecuteFeatureInstant(UIStates.Interactive, false);
                    break;

                case UIStates.NonInteractive:
                    _isInteractive = false;
                    ExecuteFeatureInstant(UIStates.NonInteractive, false);
                    break;

                case UIStates.Default:
                    ExecuteFeatureInstant(UIStates.Default, false);
                    break;

                case UIStates.Hover:
                    _isHovered = true;
                    ExecuteFeatureInstant(UIStates.Hover, false);
                    break;

                case UIStates.Press:
                    _isPressed = true;
                    ExecuteFeatureInstant(UIStates.Press, false);
                    break;

                case UIStates.Select:
                    _isSelected = true;
                    ExecuteFeatureInstant(UIStates.Select, false);
                    break;
            }
        }

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

        private void ExecuteFeatureInstant(UIStates state, bool playAudio = true, Action done = null)
        {
            if (_colorFeature != null)
                _colorFeature.ExecuteFeatureInstant(state);

            if (_spriteFeature != null)
                _spriteFeature.ExecuteFeatureInstant(state);

            if (_transformFeature != null)
                _transformFeature.ExecuteFeatureInstant(state);

            if (_audioFeature != null && playAudio)
                _audioFeature.ExecuteFeatureInstant(state);
        }

        #endregion
    }

    public enum ButtonTypes { Button, Checkbox, Radio }
}
