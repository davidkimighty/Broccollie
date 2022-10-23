using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CollieMollie.Core
{
    public abstract class BasePointerInteractable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
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

        [Header("Base Pointer Interactable")]
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
            ChangeColors(InteractionState.Default, instantChange);
            ChangeSprites(InteractionState.Default);
            PlayAnimation(InteractionState.Default);
            if (playAudio)
                PlayAudio(InteractionState.Default);

            if (invokeEvent)
            {
                RaiseDefaultEvent(new InteractableEventArgs(this));
                //Debug.Log("[UIButton] Invoke Default");
            }
        }

        protected virtual void HoveredBehavior(bool instantChange = false, bool playAudio = true,
            bool invokeEvent = true)
        {
            ChangeColors(InteractionState.Hovered, instantChange);
            ChangeSprites(InteractionState.Hovered);
            PlayAnimation(InteractionState.Hovered);
            if (playAudio)
                PlayAudio(InteractionState.Hovered);

            if (invokeEvent)
            {
                RaiseHoveredEvent(new InteractableEventArgs(this));
                //Debug.Log("[UIButton] Invoke Hovered");
            }
        }

        protected virtual void PressedBehavior(bool instantChange = false, bool playAudio = true,
            bool invokeEvent = true)
        {
            ChangeColors(InteractionState.Pressed, instantChange);
            ChangeSprites(InteractionState.Pressed);
            PlayAnimation(InteractionState.Pressed);
            if (playAudio)
                PlayAudio(InteractionState.Pressed);

            if (invokeEvent)
            {
                RaisePressedEvent(new InteractableEventArgs(this));
                //Debug.Log("[UIButton] Invoke Pressed");
            }
        }

        protected virtual void SelectedBehavior(bool instantChange = false, bool playAudio = true,
            bool invokeEvent = true)
        {
            ChangeColors(InteractionState.Selected, instantChange);
            ChangeSprites(InteractionState.Selected);
            PlayAnimation(InteractionState.Selected);
            if (playAudio)
                PlayAudio(InteractionState.Selected);

            if (invokeEvent)
            {
                RaiseSelectedEvent(new InteractableEventArgs(this));
                //Debug.Log("[UIButton] Invoke Selected");
            }
        }

        protected virtual void InteractiveBehavior(bool instantChange = false, bool playAudio = true,
            bool invokeEvent = true)
        {
            ChangeColors(InteractionState.Interactive, instantChange);
            ChangeSprites(InteractionState.Interactive);
            PlayAnimation(InteractionState.Interactive);
            if (playAudio)
                PlayAudio(InteractionState.Interactive);

            if (invokeEvent)
            {
                RaiseInteractiveEvent(new InteractableEventArgs(this));
            }
        }

        protected virtual void NonInteractiveBehavior(bool instantChange = false, bool playAudio = true,
            bool invokeEvent = true)
        {
            ChangeColors(InteractionState.NonInteractive, instantChange);
            ChangeSprites(InteractionState.NonInteractive);
            PlayAnimation(InteractionState.NonInteractive);
            if (playAudio)
                PlayAudio(InteractionState.NonInteractive);

            if (invokeEvent)
            {
                RaiseNonInteractiveEvent(new InteractableEventArgs(this));
            }
        }

        protected virtual IEnumerator ShowBehavior(float duration, bool instantChange = false, bool playAudio = true,
            bool invokeEvent = true, Action done = null)
        {
            if (invokeEvent)
                RaiseShowEvent(new InteractableEventArgs(this));

            ChangeColors(InteractionState.Show, instantChange);
            ChangeSprites(InteractionState.Show);
            PlayAnimation(InteractionState.Show);
            if (playAudio)
                PlayAudio(InteractionState.Show);

            yield return new WaitForSeconds(duration);

            done?.Invoke();
        }

        protected virtual IEnumerator HideBehavior(float duration, bool instantChange = false, bool playAudio = true,
            bool invokeEvent = true, Action done = null)
        {
            if (invokeEvent)
                RaiseHideEvent(new InteractableEventArgs(this));

            ChangeColors(InteractionState.Hide, instantChange);
            ChangeSprites(InteractionState.Hide);
            PlayAnimation(InteractionState.Hide);
            if (playAudio)
                PlayAudio(InteractionState.Hide);

            yield return new WaitForSeconds(duration);

            done?.Invoke();
        }
        #endregion

        #region Features
        protected abstract void SetActive(bool state);

        protected virtual void ChangeColors(InteractionState state, bool instantChange = false) { }

        protected virtual void ChangeSprites(InteractionState state) { }

        protected virtual void PlayAudio(InteractionState state) { }

        protected virtual void PlayAnimation(InteractionState state) { }

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
