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
        public event Action<InteractableEventArgs> OnInteractive = null;
        public event Action<InteractableEventArgs> OnNonInteractive = null;
        public event Action<InteractableEventArgs> OnShow = null;
        public event Action<InteractableEventArgs> OnHide = null;

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
                NonInteractiveButton(true);
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

                case ButtonState.Interactive:
                    _selected = _pressed = _hovering = false;
                    _interactable = true;
                    InteractiveButton(instantChange, playAudio, invokeEvent);
                    break;

                case ButtonState.NonInteractive:
                    _selected = _pressed = _hovering = false;
                    _interactable = false;
                    NonInteractiveButton(instantChange, playAudio, invokeEvent);
                    break;

                case ButtonState.Show:
                    _interactable = true;
                    ShowButton(instantChange, playAudio, invokeEvent);
                    break;

                case ButtonState.Hide:
                    _interactable = false;
                    HideButton(instantChange, playAudio, invokeEvent);
                    break;
            }
        }
        #endregion

        #region Button Interaction Publishers
        protected override sealed void InvokeEnterAction(PointerEventData eventData = null)
        {
            if (!_interactable) return;

            _hovering = true;
            HoveredButton(false, true, false);
            OnHovered?.Invoke(new InteractableEventArgs(this));
        }

        protected override sealed void InvokeExitAction(PointerEventData eventData = null)
        {
            if (!_interactable) return;

            _hovering = false;
            if (_selected)
            {
                SelectedButton(false, true, false);
            }
            else
            {
                _selected = _pressed = false;
                DefaultButton(false, true, false);
            }
        }

        protected override sealed void InvokeDownAction(PointerEventData eventData = null)
        {
            if (!_interactable) return;

            _pressed = true;
            PressedButton(false, true, false);

            OnPressed?.Invoke(new InteractableEventArgs(this));
        }

        protected override sealed void InvokeUpAction(PointerEventData eventData = null)
        {
            if (!_interactable) return;

            // Cancel Interaction
            _pressed = false;
            if (!_selected && !_hovering)
            {
                _selected = _pressed = _hovering = false;
                DefaultButton(false, true, false);
            }
            else if (_selected && !_hovering)
            {
                SelectedButton(false, true, false);
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
                SelectedButton(false, true, false);
            else
                DefaultButton(false, true, false);

            if (_hovering)
                HoveredButton(false, true, false);

            OnSelected?.Invoke(new InteractableEventArgs(this));
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

        private void InteractiveButton(bool instantChange = false, bool playAudio = true, bool invokeEvent = true)
        {
            ChangeColors(ButtonState.Interactive, instantChange);
            ChangeSprites(ButtonState.Interactive);
            ChangeAnimation(ButtonState.Interactive);
            if (playAudio)
                PlayAudio(ButtonState.Interactive);

            if (invokeEvent)
            {
                OnInteractive?.Invoke(new InteractableEventArgs(this));
            }
        }

        private void NonInteractiveButton(bool instantChange = false, bool playAudio = true, bool invokeEvent = true)
        {
            ChangeColors(ButtonState.NonInteractive, instantChange);
            ChangeSprites(ButtonState.NonInteractive);
            ChangeAnimation(ButtonState.NonInteractive);
            if (playAudio)
                PlayAudio(ButtonState.NonInteractive);

            if (invokeEvent)
            {
                OnNonInteractive?.Invoke(new InteractableEventArgs(this));
            }
        }

        private void ShowButton(bool instantChange = false, bool playAudio = true, bool invokeEvent = true)
        {
            ChangeColors(ButtonState.Show, instantChange);
            ChangeSprites(ButtonState.Show);
            ChangeAnimation(ButtonState.Show);
            if (playAudio)
                PlayAudio(ButtonState.Show);

            if (invokeEvent)
            {
                OnShow?.Invoke(new InteractableEventArgs(this));
            }
        }

        private void HideButton(bool instantChange = false, bool playAudio = true, bool invokeEvent = true)
        {
            ChangeColors(ButtonState.Hide, instantChange);
            ChangeSprites(ButtonState.Hide);
            ChangeAnimation(ButtonState.Hide);
            if (playAudio)
                PlayAudio(ButtonState.Hide);

            if (invokeEvent)
            {
                OnHide?.Invoke(new InteractableEventArgs(this));
            }
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
    public enum ButtonState { None, Default, Hovered, Pressed, Selected, Interactive, NonInteractive, Show, Hide }
}
