using System;
using System.Collections;
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
        public void ChangeState(InteractionState state, bool invokeEvent = true, bool playAudio = true, bool instantChange = false)
        {
            _interactable = true;
            switch (state)
            {
                case InteractionState.Default:
                    _selected = _pressed = _hovering = false;
                    DefaultButton(instantChange, playAudio, invokeEvent);
                    break;

                case InteractionState.Hovered:
                    _hovering = true; _pressed = false;
                    HoveredButton(instantChange, playAudio, invokeEvent);
                    break;

                case InteractionState.Pressed:
                    _pressed = true;
                    PressedButton(instantChange, playAudio, invokeEvent);
                    break;

                case InteractionState.Selected:
                    _hovering = false; _pressed = false; _selected = true;
                    SelectedButton(instantChange, playAudio, invokeEvent);
                    break;

                case InteractionState.Interactive:
                    _selected = _pressed = _hovering = false;
                    _interactable = true;
                    InteractiveButton(instantChange, playAudio, invokeEvent);
                    break;

                case InteractionState.NonInteractive:
                    _selected = _pressed = _hovering = false;
                    _interactable = false;
                    NonInteractiveButton(instantChange, playAudio, invokeEvent);
                    break;
            }
        }

        public void Show(float duration, bool invokeEvent = true, bool playAudio = true, bool instantChange = false)
        {
            _interactable = true;
            gameObject.SetActive(true);
            StartCoroutine(ShowButton(duration, instantChange, playAudio, invokeEvent, () =>
            {
                ChangeState(InteractionState.Default, invokeEvent, playAudio, instantChange);
            }));
        }

        public void Hide(float duration, bool invokeEvent = true, bool playAudio = true, bool instantChange = false)
        {
            _interactable = false;
            StartCoroutine(HideButton(duration, instantChange, playAudio, invokeEvent, () =>
            {
                gameObject.SetActive(false);
            }));
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
            ChangeColors(InteractionState.Default, instantChange);
            ChangeSprites(InteractionState.Default);
            ChangeAnimation(InteractionState.Default);
            if (playAudio)
                PlayAudio(InteractionState.Default);

            if (invokeEvent)
            {
                OnDefault?.Invoke(new InteractableEventArgs(this));
                //Debug.Log("[UIButton] Invoke Default");
            }
        }

        private void HoveredButton(bool instantChange = false, bool playAudio = true, bool invokeEvent = true)
        {
            ChangeColors(InteractionState.Hovered, instantChange);
            ChangeSprites(InteractionState.Hovered);
            ChangeAnimation(InteractionState.Hovered);
            if (playAudio)
                PlayAudio(InteractionState.Hovered);

            if (invokeEvent)
            {
                OnHovered?.Invoke(new InteractableEventArgs(this));
                //Debug.Log("[UIButton] Invoke Hovered");
            }
        }

        private void PressedButton(bool instantChange = false, bool playAudio = true, bool invokeEvent = true)
        {
            ChangeColors(InteractionState.Pressed, instantChange);
            ChangeSprites(InteractionState.Pressed);
            ChangeAnimation(InteractionState.Pressed);
            if (playAudio)
                PlayAudio(InteractionState.Pressed);

            if (invokeEvent)
            {
                OnPressed?.Invoke(new InteractableEventArgs(this));
                //Debug.Log("[UIButton] Invoke Pressed");
            }
        }

        private void SelectedButton(bool instantChange = false, bool playAudio = true, bool invokeEvent = true)
        {
            ChangeColors(InteractionState.Selected, instantChange);
            ChangeSprites(InteractionState.Selected);
            ChangeAnimation(InteractionState.Selected);
            if (playAudio)
                PlayAudio(InteractionState.Selected);

            if (invokeEvent)
            {
                OnSelected?.Invoke(new InteractableEventArgs(this));
                //Debug.Log("[UIButton] Invoke Selected");
            }
        }

        private void InteractiveButton(bool instantChange = false, bool playAudio = true, bool invokeEvent = true)
        {
            ChangeColors(InteractionState.Interactive, instantChange);
            ChangeSprites(InteractionState.Interactive);
            ChangeAnimation(InteractionState.Interactive);
            if (playAudio)
                PlayAudio(InteractionState.Interactive);

            if (invokeEvent)
            {
                OnInteractive?.Invoke(new InteractableEventArgs(this));
            }
        }

        private void NonInteractiveButton(bool instantChange = false, bool playAudio = true, bool invokeEvent = true)
        {
            ChangeColors(InteractionState.NonInteractive, instantChange);
            ChangeSprites(InteractionState.NonInteractive);
            ChangeAnimation(InteractionState.NonInteractive);
            if (playAudio)
                PlayAudio(InteractionState.NonInteractive);

            if (invokeEvent)
            {
                OnNonInteractive?.Invoke(new InteractableEventArgs(this));
            }
        }

        private IEnumerator ShowButton(float duration, bool instantChange = false, bool playAudio = true, bool invokeEvent = true, Action done = null)
        {
            ChangeColors(InteractionState.Show, instantChange);
            ChangeSprites(InteractionState.Show);
            ChangeAnimation(InteractionState.Show);
            if (playAudio)
                PlayAudio(InteractionState.Show);

            yield return new WaitForSeconds(duration);

            if (invokeEvent)
                OnShow?.Invoke(new InteractableEventArgs(this));
            done?.Invoke();
        }

        private IEnumerator HideButton(float duration, bool instantChange = false, bool playAudio = true, bool invokeEvent = true, Action done = null)
        {
            ChangeColors(InteractionState.Hide, instantChange);
            ChangeSprites(InteractionState.Hide);
            ChangeAnimation(InteractionState.Hide);
            if (playAudio)
                PlayAudio(InteractionState.Hide);

            yield return new WaitForSeconds(duration);

            if (invokeEvent)
                OnHide?.Invoke(new InteractableEventArgs(this));
            done?.Invoke();
        }
        #endregion

        #region Button Features
        private void ChangeColors(InteractionState state, bool instantChange = false)
        {
            if (_colorFeature == null) return;

            if (instantChange)
                _colorFeature.ChangeInstantly(state);
            else
                _colorFeature.ChangeGradually(state);
        }

        private void ChangeSprites(InteractionState state)
        {
            if (_spriteFeature == null) return;

            _spriteFeature.Change(state);
        }

        private void PlayAudio(InteractionState state)
        {
            if (_audioFeature == null) return;

            _audioFeature.Play(state);
        }

        private void ChangeAnimation(InteractionState state)
        {
            if (_animationFeature == null) return;

            _animationFeature.Change(state);
        }
        #endregion
    }

    public enum ButtonType { Button, Radio, Checkbox }
    public enum InteractionState { None, Default, Hovered, Pressed, Selected, Interactive, NonInteractive, Show, Hide }
}
