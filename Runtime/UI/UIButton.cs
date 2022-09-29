using System;
using CollieMollie.Editor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CollieMollie.UI
{
    public class UIButton : BaseUI
    {
        #region Variable Field
        public event Action<UIEventArgs> OnDefault = null;
        public event Action<UIEventArgs> OnHovered = null;
        public event Action<UIEventArgs> OnPressed = null;
        public event Action<UIEventArgs> OnSelected = null;
        public event Action<UIEventArgs> OnDisabled = null;

        [Header("Button")]
        [SerializeField] private ButtonType _type = ButtonType.Button;
        [SerializeField] private UIColorFeature _colorFeature = null;
        [SerializeField] private UIAudioFeature _audioFeature = null;

        [SerializeField] private bool _interactable = true;
        public bool IsInteractable
        {
            get => _interactable;
        }

        [ReadOnly]
        [SerializeField] private bool _hovering = false;
        public bool IsHovering
        {
            get => _hovering;
        }

        [ReadOnly]
        [SerializeField] private bool _pressed = false;
        public bool IsPressed
        {
            get => _pressed;
        }

        [ReadOnly]
        [SerializeField] private bool _selected = false;
        public bool IsSelected
        {
            get => _selected;
        }

        [ReadOnly]
        [SerializeField] private bool _dragging = false;
        public bool IsDragging
        {
            get => _dragging;
        }
        #endregion

        private void Start()
        {
            _hovering = _pressed = _selected = false;
            if (_interactable)
                DefaultButton(true);
            else
                DisabledButton(true);
        }

        #region Public Functions
        /// <summary>
        /// Force
        /// </summary>
        public void ChangeState(ButtonState state, bool invokeEvent = true, bool playAudio = true, bool instantChange = false)
        {
            _interactable = true;
            switch (state)
            {
                case ButtonState.Default:
                    DefaultButton(instantChange, playAudio, invokeEvent);
                    break;

                case ButtonState.Hovered:
                    HoveredButton(instantChange, playAudio, invokeEvent);
                    break;

                case ButtonState.Pressed:
                    PressedButton(instantChange, playAudio, invokeEvent);
                    break;

                case ButtonState.Selected:
                    SelectedButton(instantChange, playAudio, invokeEvent);
                    break;

                case ButtonState.Disabled:
                    _interactable = false;
                    DisabledButton(instantChange, playAudio, invokeEvent);
                    break;
            }
        }
        #endregion

        #region Button Interaction Publishers
        protected override sealed void InvokeEnterAction(PointerEventData eventData = null)
        {
            if (!_interactable) return;

            _hovering = true;
            if (_selected) return;

            HoveredButton();
        }

        protected override sealed void InvokeExitAction(PointerEventData eventData = null)
        {
            if (!_interactable) return;

            _hovering = false;
            if (_selected) return;

            DefaultButton();
        }

        protected override sealed void InvokeDownAction(PointerEventData eventData = null)
        {
            if (!_interactable) return;

            PressedButton();
        }

        protected override sealed void InvokeUpAction(PointerEventData eventData = null)
        {
            if (!_interactable) return;

            _pressed = false;
            CancelInteraction();

            void CancelInteraction()
            {
                if (!_selected && !_hovering)
                {
                    ChangeColors(ButtonState.Default);
                }
                else if (_selected && !_hovering)
                {
                    ChangeColors(ButtonState.Selected);
                }
            }
        }

        protected override sealed void InvokeClickAction(PointerEventData eventData = null)
        {
            if (!_interactable) return;

            SelectedButton();
        }

        private void InvokeDisableAction()
        {
            DisabledButton();

            OnDisabled?.Invoke(new UIEventArgs(this));
            //Debug.Log("[UIButton] Invoke Disabled");
        }
        #endregion

        #region Button Behaviors
        private void DefaultButton(bool instantChange = false, bool playAudio = true, bool invokeEvent = true)
        {
            _selected = _pressed = _hovering = false;
            ChangeColors(ButtonState.Default, instantChange);
            if (playAudio)
                PlayAudio(ButtonState.Default);

            if (invokeEvent)
            {
                OnDefault?.Invoke(new UIEventArgs(this));
                //Debug.Log("[UIButton] Invoke Default");
            }
        }

        private void HoveredButton(bool instantChange = false, bool playAudio = true, bool invokeEvent = true)
        {
            _hovering = true;
            ChangeColors(ButtonState.Hovered, instantChange);
            if (playAudio)
                PlayAudio(ButtonState.Hovered);

            if (invokeEvent)
            {
                OnHovered?.Invoke(new UIEventArgs(this));
                //Debug.Log("[UIButton] Invoke Hovered");
            }
        }

        private void PressedButton(bool instantChange = false, bool playAudio = true, bool invokeEvent = true)
        {
            _pressed = true;
            ChangeColors(ButtonState.Pressed, instantChange);
            if (playAudio)
                PlayAudio(ButtonState.Pressed);

            if (invokeEvent)
            {
                OnPressed?.Invoke(new UIEventArgs(this));
                //Debug.Log("[UIButton] Invoke Pressed");
            }
        }

        private void SelectedButton(bool instantChange = false, bool playAudio = true, bool invokeEvent = true)
        {
            _selected = _type switch
            {
                ButtonType.Radio => true,
                ButtonType.Checkbox => !_selected,
                _ => false
            };

            if (_selected)
            {
                ChangeColors(ButtonState.Selected, instantChange);
                if (playAudio)
                    PlayAudio(ButtonState.Selected);
            }
            else if (_hovering)
            {
                ChangeColors(ButtonState.Hovered, instantChange);
                if (playAudio)
                    PlayAudio(ButtonState.Hovered);
            }
            else
            {
                ChangeColors(ButtonState.Default, instantChange);
                if (playAudio)
                    PlayAudio(ButtonState.Default);
            }

            if (invokeEvent)
            {
                OnSelected?.Invoke(new UIEventArgs(this));
                //Debug.Log("[UIButton] Invoke Selected");
            }
        }

        private void DisabledButton(bool instantChange = false, bool playAudio = true, bool invokeEvent = true)
        {
            _interactable = false;
            ChangeColors(ButtonState.Disabled, instantChange);
            if (playAudio)
                PlayAudio(ButtonState.Disabled);
        }
        #endregion

        #region Button Features
        private void ChangeColors(ButtonState state, bool instantChange = false)
        {
            if (_colorFeature == null) return;

            if (instantChange)
                _colorFeature.ChangeInstantly(state);
            else
                _colorFeature.ChangeGradually(state);
        }

        private void ChangeSprites(ButtonState state)
        {

        }

        private void PlayAudio(ButtonState state)
        {
            if (_audioFeature == null) return;

            _audioFeature.Play(state);
        }
        #endregion
    }

    public enum ButtonType { Button, Radio, Checkbox }
    public enum ButtonState { Default, Hovered, Pressed, Selected, Disabled }
}
