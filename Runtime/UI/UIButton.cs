using System;
using CollieMollie.Core;
using CollieMollie.Editor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CollieMollie.UI
{
    public class UIButton : BasePointerInteractable
    {
        #region Variable Field
        public event Action<InteractableEventArgs> OnDefault = null;
        public event Action<InteractableEventArgs> OnHovered = null;
        public event Action<InteractableEventArgs> OnPressed = null;
        public event Action<InteractableEventArgs> OnSelected = null;
        public event Action<InteractableEventArgs> OnDisabled = null;

        [Header("Button")]
        [SerializeField] private ButtonType _type = ButtonType.Button;
        [SerializeField] private UIColorFeature _colorFeature = null;
        [SerializeField] private UIAudioFeature _audioFeature = null;
        [SerializeField] private UISpriteFeature _spriteFeature = null;
        [SerializeField] private UIAnimationFeature _animationFeature = null;

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
        /// Force state change.
        /// </summary>
        public void ChangeState(ButtonState state, bool invokeEvent = true, bool playAudio = true, bool instantChange = false)
        {
            _interactable = true;
            switch (state)
            {
                case ButtonState.Default:
                    _selected = _pressed = _hovering = false;
                    DefaultButton(instantChange, playAudio, invokeEvent);
                    break;

                case ButtonState.Hovered:
                    _hovering = true; _pressed = false;
                    HoveredButton(instantChange, playAudio, invokeEvent);
                    break;

                case ButtonState.Pressed:
                    _pressed = true;
                    PressedButton(instantChange, playAudio, invokeEvent);
                    break;

                case ButtonState.Selected:
                    _hovering = false; _pressed = false; _selected = true;
                    SelectedButton(instantChange, playAudio, invokeEvent);
                    break;

                case ButtonState.Disabled:
                    _selected = _pressed = _hovering = false;
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
            HoveredButton();
        }

        protected override sealed void InvokeExitAction(PointerEventData eventData = null)
        {
            if (!_interactable) return;

            _hovering = false;
            if (_selected)
            {
                SelectedButton();
            }
            else
            {
                _selected = _pressed = false;
                DefaultButton();
            }
        }

        protected override sealed void InvokeDownAction(PointerEventData eventData = null)
        {
            if (!_interactable) return;

            _pressed = true;
            PressedButton();
        }

        protected override sealed void InvokeUpAction(PointerEventData eventData = null)
        {
            if (!_interactable) return;

            // Cancel Interaction
            _pressed = false;
            if (!_selected && !_hovering)
            {
                _selected = _pressed = _hovering = false;
                DefaultButton();
            }
            else if (_selected && !_hovering)
            {
                SelectedButton();
            }
        }

        protected override sealed void InvokeClickAction(PointerEventData eventData = null)
        {
            if (!_interactable) return;

            _selected = _type switch
            {
                ButtonType.Radio => true,
                ButtonType.Checkbox => !_selected,
                _ => false
            };

            if (_selected)
            {
                SelectedButton();
            }
            else
            {
                DefaultButton();
            }

            if (_hovering)
            {
                HoveredButton();
            }
        }

        private void InvokeDisableAction()
        {
            _interactable = false;
            DisabledButton();

            OnDisabled?.Invoke(new InteractableEventArgs(this));
            //Debug.Log("[UIButton] Invoke Disabled");
        }
        #endregion

        #region Button Behaviors
        private void DefaultButton(bool instantChange = false, bool playAudio = true, bool invokeEvent = true)
        {
            ChangeColors(ButtonState.Default, instantChange);
            ChangeSprites(ButtonState.Default);
            ChangeAnimation(ButtonState.Default);
            if (playAudio)
                PlayAudio(ButtonState.Default);

            if (invokeEvent)
            {
                OnDefault?.Invoke(new InteractableEventArgs(this));
                //Debug.Log("[UIButton] Invoke Default");
            }
        }

        private void HoveredButton(bool instantChange = false, bool playAudio = true, bool invokeEvent = true)
        {
            ChangeColors(ButtonState.Hovered, instantChange);
            ChangeSprites(ButtonState.Hovered);
            ChangeAnimation(ButtonState.Hovered);
            if (playAudio)
                PlayAudio(ButtonState.Hovered);

            if (invokeEvent)
            {
                OnHovered?.Invoke(new InteractableEventArgs(this));
                //Debug.Log("[UIButton] Invoke Hovered");
            }
        }

        private void PressedButton(bool instantChange = false, bool playAudio = true, bool invokeEvent = true)
        {
            ChangeColors(ButtonState.Pressed, instantChange);
            ChangeSprites(ButtonState.Pressed);
            ChangeAnimation(ButtonState.Pressed);
            if (playAudio)
                PlayAudio(ButtonState.Pressed);

            if (invokeEvent)
            {
                OnPressed?.Invoke(new InteractableEventArgs(this));
                //Debug.Log("[UIButton] Invoke Pressed");
            }
        }

        private void SelectedButton(bool instantChange = false, bool playAudio = true, bool invokeEvent = true)
        {
            ChangeColors(ButtonState.Selected, instantChange);
            ChangeSprites(ButtonState.Selected);
            ChangeAnimation(ButtonState.Selected);
            if (playAudio)
                PlayAudio(ButtonState.Selected);

            if (invokeEvent)
            {
                OnSelected?.Invoke(new InteractableEventArgs(this));
                //Debug.Log("[UIButton] Invoke Selected");
            }
        }

        private void DisabledButton(bool instantChange = false, bool playAudio = true, bool invokeEvent = true)
        {
            ChangeColors(ButtonState.Disabled, instantChange);
            ChangeSprites(ButtonState.Disabled);
            ChangeAnimation(ButtonState.Disabled);
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
            if (_spriteFeature == null) return;

            _spriteFeature.Change(state);
        }

        private void PlayAudio(ButtonState state)
        {
            if (_audioFeature == null) return;

            _audioFeature.Play(state);
        }

        private void ChangeAnimation(ButtonState state)
        {
            if (_animationFeature == null) return;

            _animationFeature.Change(state);
        }
        #endregion
    }

    public enum ButtonType { Button, Radio, Checkbox }
    public enum ButtonState { None, Default, Hovered, Pressed, Selected, Enabled, Disabled }
}
