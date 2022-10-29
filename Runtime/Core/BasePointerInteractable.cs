using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CollieMollie.Core
{
    public abstract class BasePointerInteractable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler,
        IBeginDragHandler, IDragHandler, IEndDragHandler
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
        public event Action<InteractableEventArgs> OnBeginDrag = null;
        public event Action<InteractableEventArgs> OnDrag = null;
        public event Action<InteractableEventArgs> OnEndDrag = null;
        public event Action<InteractableEventArgs> OnDrop = null;

        [Header("State")]
        [SerializeField] protected bool _visible = true;
        public bool IsVisible
        {
            get => _visible;
        }

        [SerializeField] protected bool _interactable = true;
        public bool IsInteractable
        {
            get => _interactable;
        }

        [SerializeField] protected bool _hovering = false;
        public bool IsHovering
        {
            get => _hovering;
        }

        [SerializeField] protected bool _pressed = false;
        public bool IsPressed
        {
            get => _pressed;
        }

        [SerializeField] protected bool _selected = false;
        public bool IsSelected
        {
            get => _selected;
        }

        [SerializeField] protected bool _dragging = false;
        public bool IsDragging
        {
            get => _dragging;
        }

        private IEnumerator _visibleAction = null;
        #endregion

        #region Public Functions
        /// <summary>
        /// Force state change.
        /// </summary>
        public void ChangeState(InteractionState state, bool invokeEvent = true, bool playAudio = true,
            bool instantChange = false, bool forceChange = false)
        {
            switch (state)
            {
                case InteractionState.Default:
                    if (!_interactable && !forceChange) return;
                    _interactable = true;
                    _selected = _pressed = _hovering = false;
                    DefaultBehavior(instantChange, playAudio, invokeEvent);
                    break;

                case InteractionState.Hovered:
                    if (!_interactable && !forceChange) return;
                    _interactable = true;
                    _hovering = true; _pressed = false;
                    HoveredBehavior(instantChange, playAudio, invokeEvent);
                    break;

                case InteractionState.Pressed:
                    if (!_interactable && !forceChange) return;
                    _interactable = true;
                    _pressed = true;
                    PressedBehavior(instantChange, playAudio, invokeEvent);
                    break;

                case InteractionState.Selected:
                    if (!_interactable && !forceChange) return;
                    _interactable = true;
                    _hovering = false; _pressed = false; _selected = true;
                    SelectedBehavior(instantChange, playAudio, invokeEvent);
                    break;

                case InteractionState.Interactive:
                    _interactable = true;
                    _selected = _pressed = _hovering = false;
                    InteractiveBehavior(instantChange, playAudio, invokeEvent);
                    break;

                case InteractionState.NonInteractive:
                    _interactable = false;
                    _selected = _pressed = _hovering = false;
                    NonInteractiveBehavior(instantChange, playAudio, invokeEvent);
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void SetVisible(bool isVisible, float duration = 0f, bool invokeEvent = true,
            bool playAudio = true, bool instantChange = false)
        {
            if (_visibleAction != null)
                StopCoroutine(_visibleAction);
            _interactable = false;

            if (isVisible)
            {
                SetActive(isVisible);
                _visibleAction = ShowBehavior(duration, instantChange, playAudio, invokeEvent, () =>
                {
                    _interactable = true;
                    ChangeState(InteractionState.Default);
                });
                StartCoroutine(_visibleAction);
                _interactable = true;
            }
            else
            {
                _visibleAction = HideBehavior(duration, instantChange, playAudio, invokeEvent, () =>
                {
                    SetActive(isVisible);
                });
                StartCoroutine(_visibleAction);
            }
            _visible = isVisible;
        }
        #endregion

        #region Publishers
        protected void RaiseDefaultEvent(InteractableEventArgs args) => OnDefault?.Invoke(args);

        protected void RaiseHoveredEvent(InteractableEventArgs args) => OnHovered?.Invoke(args);

        protected void RaisePressedEvent(InteractableEventArgs args) => OnPressed?.Invoke(args);

        protected void RaiseSelectedEvent(InteractableEventArgs args) => OnSelected?.Invoke(args);

        protected void RaiseInteractiveEvent(InteractableEventArgs args) => OnInteractive?.Invoke(args);

        protected void RaiseNonInteractiveEvent(InteractableEventArgs args) => OnNonInteractive?.Invoke(args);

        protected void RaiseShowEvent(InteractableEventArgs args) => OnShow?.Invoke(args);

        protected void RaiseHideEvent(InteractableEventArgs args) => OnHide?.Invoke(args);

        protected void RaiseBeginDragEvent(InteractableEventArgs args) => OnBeginDrag?.Invoke(args);

        protected void RaiseDragEvent(InteractableEventArgs args) => OnDrag?.Invoke(args);

        protected void RaiseEndDragEvent(InteractableEventArgs args) => OnEndDrag?.Invoke(args);

        #endregion

        #region Pointer Callbacks
        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData) => InvokeEnterAction(eventData);

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData) => InvokeExitAction(eventData);

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData) => InvokeDownAction(eventData);

        void IPointerUpHandler.OnPointerUp(PointerEventData eventData) => InvokeUpAction(eventData);

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData) => InvokeClickAction(eventData);

        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData) => BeginDragAction(eventData);

        void IDragHandler.OnDrag(PointerEventData eventData) => DragAction(eventData);

        void IEndDragHandler.OnEndDrag(PointerEventData eventData) => EndDragAction(eventData);
        #endregion

        #region Pointer Subscribers
        protected virtual void InvokeEnterAction(PointerEventData eventData = null)
        {
            if (this.GetType() == typeof(BasePointerInteractable)) return;

            if (!_interactable) return;

            _hovering = true;
            HoveredBehavior();
        }

        protected virtual void InvokeExitAction(PointerEventData eventData = null)
        {
            if (this.GetType() == typeof(BasePointerInteractable)) return;

            if (!_interactable) return;

            _hovering = false;
            if (_selected)
            {
                SelectedBehavior(false, true, false);
            }
            else
            {
                _selected = _pressed = false;
                DefaultBehavior(false, true, false);
            }
        }

        protected virtual void InvokeDownAction(PointerEventData eventData = null)
        {
            if (this.GetType() == typeof(BasePointerInteractable)) return;

            if (!_interactable) return;

            _pressed = true;
            PressedBehavior();
        }

        protected virtual void InvokeUpAction(PointerEventData eventData = null)
        {
            if (this.GetType() == typeof(BasePointerInteractable)) return;

            if (!_interactable) return;

            // Cancel Interaction
            _pressed = false;
            if (!_selected && !_hovering)
            {
                _selected = _pressed = _hovering = false;
                DefaultBehavior(false, true, false);
            }
            else if (_selected && !_hovering)
            {
                SelectedBehavior(false, true, false);
            }
        }

        protected virtual void InvokeClickAction(PointerEventData eventData = null)
        {
            if (this.GetType() == typeof(BasePointerInteractable)) return;

            if (!_interactable) return;

            _selected = !_selected;

            if (_selected)
                SelectedBehavior();
            else
                DefaultBehavior();
        }

        protected virtual void BeginDragAction(PointerEventData eventData = null)
        {
            if (this.GetType() == typeof(BasePointerInteractable)) return;

            if (!_interactable) return;

            _dragging = true;
        }

        protected virtual void DragAction(PointerEventData eventData = null)
        {
            if (this.GetType() == typeof(BasePointerInteractable)) return;

            DragBehavior(eventData);
        }

        protected virtual void EndDragAction(PointerEventData eventData = null)
        {
            if (this.GetType() == typeof(BasePointerInteractable)) return;

            if (!_interactable) return;

            _dragging = false;
        }

        #endregion

        #region Behaviors
        protected virtual void DefaultBehavior(bool instantChange = false, bool playAudio = true,
            bool invokeEvent = true)
        {
            ChangeColorFeature(InteractionState.Default, instantChange);
            ChangeSpriteFeature(InteractionState.Default);
            PlayAnimationFeature(InteractionState.Default);
            if (playAudio)
                PlayAudioFeature(InteractionState.Default);

            if (invokeEvent)
            {
                RaiseDefaultEvent(new InteractableEventArgs(this));
                //Debug.Log("[UIButton] Invoke Default");
            }
        }

        protected virtual void HoveredBehavior(bool instantChange = false, bool playAudio = true,
            bool invokeEvent = true)
        {
            if (invokeEvent)
            {
                RaiseHoveredEvent(new InteractableEventArgs(this));
                //Debug.Log("[UIButton] Invoke Hovered");
            }

            ChangeColorFeature(InteractionState.Hovered, instantChange);
            ChangeSpriteFeature(InteractionState.Hovered);
            PlayAnimationFeature(InteractionState.Hovered);
            if (playAudio)
                PlayAudioFeature(InteractionState.Hovered);
        }

        protected virtual void PressedBehavior(bool instantChange = false, bool playAudio = true,
            bool invokeEvent = true)
        {
            if (invokeEvent)
            {
                RaisePressedEvent(new InteractableEventArgs(this));
                //Debug.Log("[UIButton] Invoke Pressed");
            }

            ChangeColorFeature(InteractionState.Pressed, instantChange);
            ChangeSpriteFeature(InteractionState.Pressed);
            PlayAnimationFeature(InteractionState.Pressed);
            if (playAudio)
                PlayAudioFeature(InteractionState.Pressed);
        }

        protected virtual void SelectedBehavior(bool instantChange = false, bool playAudio = true,
            bool invokeEvent = true)
        {
            if (invokeEvent)
            {
                RaiseSelectedEvent(new InteractableEventArgs(this));
                //Debug.Log("[UIButton] Invoke Selected");
            }

            ChangeColorFeature(InteractionState.Selected, instantChange);
            ChangeSpriteFeature(InteractionState.Selected);
            PlayAnimationFeature(InteractionState.Selected);
            if (playAudio)
                PlayAudioFeature(InteractionState.Selected);
        }

        protected virtual void InteractiveBehavior(bool instantChange = false, bool playAudio = true,
            bool invokeEvent = true)
        {
            if (invokeEvent)
            {
                RaiseInteractiveEvent(new InteractableEventArgs(this));
            }

            ChangeColorFeature(InteractionState.Interactive, instantChange);
            ChangeSpriteFeature(InteractionState.Interactive);
            PlayAnimationFeature(InteractionState.Interactive);
            if (playAudio)
                PlayAudioFeature(InteractionState.Interactive);
        }

        protected virtual void NonInteractiveBehavior(bool instantChange = false, bool playAudio = true,
            bool invokeEvent = true)
        {
            if (invokeEvent)
            {
                RaiseNonInteractiveEvent(new InteractableEventArgs(this));
            }

            ChangeColorFeature(InteractionState.NonInteractive, instantChange);
            ChangeSpriteFeature(InteractionState.NonInteractive);
            PlayAnimationFeature(InteractionState.NonInteractive);
            if (playAudio)
                PlayAudioFeature(InteractionState.NonInteractive);
        }

        protected virtual IEnumerator ShowBehavior(float duration, bool instantChange = false, bool playAudio = true,
            bool invokeEvent = true, Action done = null)
        {
            if (invokeEvent)
                RaiseShowEvent(new InteractableEventArgs(this));

            ChangeColorFeature(InteractionState.Show, instantChange);
            ChangeSpriteFeature(InteractionState.Show);
            PlayAnimationFeature(InteractionState.Show);
            if (playAudio)
                PlayAudioFeature(InteractionState.Show);

            yield return new WaitForSeconds(duration);

            done?.Invoke();
        }

        protected virtual IEnumerator HideBehavior(float duration, bool instantChange = false, bool playAudio = true,
            bool invokeEvent = true, Action done = null)
        {
            if (invokeEvent)
                RaiseHideEvent(new InteractableEventArgs(this));

            ChangeColorFeature(InteractionState.Hide, instantChange);
            ChangeSpriteFeature(InteractionState.Hide);
            PlayAnimationFeature(InteractionState.Hide);
            if (playAudio)
                PlayAudioFeature(InteractionState.Hide);

            yield return new WaitForSeconds(duration);

            done?.Invoke();
        }

        protected virtual void BeginDragBehavior(PointerEventData eventData, bool invokeEvent = true)
        {
            if (invokeEvent)
            {
                RaiseBeginDragEvent(new InteractableEventArgs(this));
            }
        }

        protected virtual void DragBehavior(PointerEventData eventData, bool invokeEvent = true)
        {
            if (invokeEvent)
            {
                RaiseDragEvent(new InteractableEventArgs(this));
            }

            DragFeature(eventData);
        }

        protected virtual void EndDragBehavior(PointerEventData eventData, bool invokeEvent = true)
        {
            if (invokeEvent)
            {
                RaiseEndDragEvent(new InteractableEventArgs(this));
            }
        }

        #endregion

        #region Features
        protected abstract void SetActive(bool state);

        protected virtual void ChangeColorFeature(InteractionState state, bool instantChange = false) { }

        protected virtual void ChangeSpriteFeature(InteractionState state) { }

        protected virtual void PlayAudioFeature(InteractionState state) { }

        protected virtual void PlayAnimationFeature(InteractionState state) { }

        protected virtual void DragFeature(PointerEventData eventData) { }

        #endregion
    }

    public class InteractableEventArgs
    {
        public BasePointerInteractable Sender = null;

        public InteractableEventArgs() { }

        public InteractableEventArgs(BasePointerInteractable sender)
        {
            this.Sender = sender;
        }

        public bool IsValid()
        {
            return Sender != null;
        }
    }

    public enum InteractionState { None, Default, Hovered, Pressed, Selected, Interactive, NonInteractive, Show, Hide }
}
