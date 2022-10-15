using System;
using System.Collections;
using System.Collections.Generic;
using CollieMollie.Core;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CollieMollie.UI
{
    public class UIPanel : BasePointerInteractable
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

        [Header("Panel")]
        // Nested Canvas method has lot of bugs in Unity 2021.3.
        // [SerializeField] private Canvas _canvas = null;
        [SerializeField] private GameObject _popupObject = null;

        [SerializeField] private UIColorFeature _colorFeature = null;
        [SerializeField] private UIAudioFeature _audioFeature = null;
        [SerializeField] private UISpriteFeature _spriteFeature = null;
        [SerializeField] private UIAnimationFeature _animationFeature = null;

        [SerializeField] protected bool _visible = true;
        public bool IsVisible
        {
            get => _visible;
        }

        [SerializeField] private bool _interactable = true;
        public bool IsInteractable
        {
            get => _interactable;
        }

        [SerializeField] private bool _hovering = false;
        public bool IsHovering
        {
            get => _hovering;
        }

        [SerializeField] private bool _pressed = false;
        public bool IsPressed
        {
            get => _pressed;
        }

        [SerializeField] private bool _selected = false;
        public bool IsSelected
        {
            get => _selected;
        }

        [SerializeField] private bool _dragging = false;
        public bool IsDragging
        {
            get => _dragging;
        }
        #endregion

        protected virtual void Awake()
        {
            _popupObject.gameObject.SetActive(_visible);
        }

        #region Public Functions
        public void ChangeState(InteractionState state, bool invokeEvent = true, bool playAudio = true, bool instantChange = false)
        {
            _interactable = true;
            switch (state)
            {
                case InteractionState.Default:
                    _selected = _pressed = _hovering = false;
                    Default(instantChange, playAudio, invokeEvent);
                    break;

                case InteractionState.Hovered:
                    _hovering = true; _pressed = false;
                    Hovered(instantChange, playAudio, invokeEvent);
                    break;

                case InteractionState.Pressed:
                    _pressed = true;
                    Pressed(instantChange, playAudio, invokeEvent);
                    break;

                case InteractionState.Selected:
                    _hovering = false; _pressed = false; _selected = true;
                    Selected(instantChange, playAudio, invokeEvent);
                    break;

                case InteractionState.Interactive:
                    _selected = _pressed = _hovering = false;
                    _interactable = true;
                    Interactive(instantChange, playAudio, invokeEvent);
                    break;

                case InteractionState.NonInteractive:
                    _selected = _pressed = _hovering = false;
                    _interactable = false;
                    NonInteractive(instantChange, playAudio, invokeEvent);
                    break;
            }
        }

        public virtual void SetVisible(bool isVisible, float duration, bool invokeEvent = true,
            bool playAudio = true, bool instantChange = false)
        {
            if (_popupObject.gameObject.activeSelf != isVisible)
            {
                if (isVisible)
                {
                    _popupObject.gameObject.SetActive(isVisible);
                    StartCoroutine(Show(duration, instantChange, playAudio, invokeEvent, () =>
                    {
                        ChangeState(InteractionState.Default);
                    }));
                }
                else
                {
                    StartCoroutine(Hide(duration, instantChange, playAudio, invokeEvent, () =>
                    {
                        _popupObject.gameObject.SetActive(isVisible);
                    }));
                }
                _visible = isVisible;
            }

            if (invokeEvent)
            {
                if (isVisible)
                    OnShow?.Invoke(new InteractableEventArgs(this));
                else
                    OnHide?.Invoke(new InteractableEventArgs(this));
            }
        }
        #endregion

        #region Panel Behaviors
        private void Default(bool instantChange = false, bool playAudio = true, bool invokeEvent = true)
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

        private void Hovered(bool instantChange = false, bool playAudio = true, bool invokeEvent = true)
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

        private void Pressed(bool instantChange = false, bool playAudio = true, bool invokeEvent = true)
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

        private void Selected(bool instantChange = false, bool playAudio = true, bool invokeEvent = true)
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

        private void Interactive(bool instantChange = false, bool playAudio = true, bool invokeEvent = true)
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

        private void NonInteractive(bool instantChange = false, bool playAudio = true, bool invokeEvent = true)
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

        private IEnumerator Show(float duration, bool instantChange = false, bool playAudio = true, bool invokeEvent = true, Action done = null)
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

        private IEnumerator Hide(float duration, bool instantChange = false, bool playAudio = true, bool invokeEvent = true, Action done = null)
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

        #region Features
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
}
