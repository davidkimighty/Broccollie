using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Broccollie.Core;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Broccollie.UI
{
    public class ButtonUI : BaselineUI, IHoverUI, IPressUI, ISelectUI, ICancelUI
    {
        #region Variable Field
        [Header("Button")]
        [SerializeField] private GameObject _button = null;
        [SerializeField] private ButtonTypes _buttonType = ButtonTypes.Button;

        [Header("Features")]
        [SerializeField] private UIColorFeature _colorFeature = null;
        [SerializeField] private UISpriteFeature _spriteFeature = null;
        [SerializeField] private UITransformFeature _transformFeature = null;
        [SerializeField] private UIAudioFeature _audioFeature = null;

        private Task _featureTasks = null;

        #endregion

        #region Public Functions
        public override void SetActive(bool state)
        {
            if (state)
            {
                _button.SetActive(true);
                RaiseOnShow();
                Task.Run(() => RaiseOnShowAsync());

                _featureTasks = ExecuteFeaturesAsync(UIStates.Show, true, () =>
                {
                    _isInteractive = true;
                    _featureTasks = ExecuteFeaturesAsync(UIStates.Default);
                });
            }
            else
            {
                _isInteractive = false;
                RaiseOnHide();
                Task.Run(() => RaiseOnHideAsync());

                _featureTasks = ExecuteFeaturesAsync(UIStates.Hide, true, () =>
                {
                    _button.SetActive(false);
                });
            }
        }

        public override void SetInteractive(bool state)
        {
            if (state)
            {
                if (!_button.activeSelf)
                    _button.SetActive(true);

                RaiseOnInteractive();
                Task.Run(() => RaiseOnInteractiveAsync());

                _featureTasks = ExecuteFeaturesAsync(UIStates.Interactive, true, () =>
                {
                    _isInteractive = true;
                });
            }
            else
            {
                if (!_button.activeSelf)
                    _button.SetActive(true);

                _isInteractive = false;
                RaiseOnInteractive();
                Task.Run(() => RaiseOnInteractiveAsync());

                _featureTasks = ExecuteFeaturesAsync(UIStates.NonInteractive);
            }
        }

        public void Hover()
        {
            if (!_isInteractive) return;

            _isHovered = true;
            RaiseOnHover();
            Task.Run(() => RaiseOnHoverAsync());

            _featureTasks = ExecuteFeaturesAsync(UIStates.Hover);
        }

        public void Press()
        {
            if (!_isInteractive) return;

            _isPressed = true;
            RaiseOnPress();
            Task.Run(() => RaiseOnPressAsync());

            _featureTasks = ExecuteFeaturesAsync(UIStates.Press);
        }

        public void Select()
        {
            if (!_isInteractive) return;

            switch (_buttonType)
            {
                case ButtonTypes.Button:
                    RaiseOnSelect();
                    Task.Run(() => RaiseOnSelectAsync());
                    break;

                case ButtonTypes.Checkbox:
                    _isSelected = !_isSelected;
                    if (_isSelected)
                    {
                        RaiseOnSelect();
                        Task.Run(() => RaiseOnSelectAsync());

                        _featureTasks = ExecuteFeaturesAsync(UIStates.Select);
                    }
                    else
                    {
                        RaiseOnDefault();
                        Task.Run(() => RaiseOnDefaultAsync());

                        _featureTasks = ExecuteFeaturesAsync(UIStates.Default);
                    }
                    break;

                case ButtonTypes.Radio:
                    _isSelected = true;
                    RaiseOnSelect();
                    Task.Run(() => RaiseOnSelectAsync());

                    _featureTasks = ExecuteFeaturesAsync(UIStates.Select);
                    break;
            }
        }

        public void Cancel()
        {
            if (_isSelected)
            {
                _featureTasks = ExecuteFeaturesAsync(UIStates.Select, false);
            }
            else
            {
                if (_isHovered)
                    _featureTasks = ExecuteFeaturesAsync(UIStates.Hover, false);
                else
                    _featureTasks = ExecuteFeaturesAsync(UIStates.Default, false);
            }
        }

        #endregion

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

            if (_audioFeature != null)
                featureTasks.Add(_audioFeature.ExecuteFeaturesAsync(state));

            await Task.WhenAll(featureTasks);
            done?.Invoke();
        }

        #endregion
    }

    public enum ButtonTypes { Button, Checkbox, Radio }
}
