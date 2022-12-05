using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CollieMollie.UI
{
    public abstract class InteractableUI : BaseUI, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler,
        IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        #region Variable Field
        public event Action<UIEventArgs> OnHovered = null;
        public event Action<UIEventArgs> OnPressed = null;
        public event Action<UIEventArgs> OnSelected = null;
        public event Action<UIEventArgs> OnBeginDrag = null;
        public event Action<UIEventArgs> OnDrag = null;
        public event Action<UIEventArgs> OnEndDrag = null;

        [Header("Interactable UI")]
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

        protected UIInteractionState _currentInteractionState = UIInteractionState.None;

        #endregion

        #region Public Functions
        public virtual void ChangeInteractionState(UIInteractionState state, bool playAudio = true, bool invokeEvent = true)
        {
            if (state == UIInteractionState.None || state == _currentInteractionState) return;
            _currentInteractionState = state;
            _interactable = true;

            switch (state)
            {
                case UIInteractionState.Hovered:
                    _hovering = true; _pressed = false;
                    HoveredBehavior(playAudio, invokeEvent);
                    break;

                case UIInteractionState.Pressed:
                    _pressed = true;
                    PressedBehavior(playAudio, invokeEvent);
                    break;

                case UIInteractionState.Selected:
                    _hovering = false; _pressed = false; _selected = true;
                    SelectedBehavior(playAudio, invokeEvent);
                    break;
            }
        }

        #endregion

        #region Publishers
        protected void RaiseHoveredEvent(UIEventArgs args) => OnHovered?.Invoke(args);

        protected void RaisePressedEvent(UIEventArgs args) => OnPressed?.Invoke(args);

        protected void RaiseSelectedEvent(UIEventArgs args) => OnSelected?.Invoke(args);

        protected void RaiseBeginDragEvent(UIEventArgs args) => OnBeginDrag?.Invoke(args);

        protected void RaiseDragEvent(UIEventArgs args) => OnDrag?.Invoke(args);

        protected void RaiseEndDragEvent(UIEventArgs args) => OnEndDrag?.Invoke(args);

        #endregion

        #region Pointer Callbacks
        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData) => InvokeEnterAction(eventData, new UIEventArgs());

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData) => InvokeExitAction(eventData, new UIEventArgs());

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData) => InvokeDownAction(eventData, new UIEventArgs());

        void IPointerUpHandler.OnPointerUp(PointerEventData eventData) => InvokeUpAction(eventData, new UIEventArgs());

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData) => InvokeClickAction(eventData, new UIEventArgs());

        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData) => InvokeBeginDragAction(eventData, new UIEventArgs());

        void IDragHandler.OnDrag(PointerEventData eventData) => InvokeDragAction(eventData, new UIEventArgs());

        void IEndDragHandler.OnEndDrag(PointerEventData eventData) => InvokeEndDragAction(eventData, new UIEventArgs());

        #endregion

        #region Pointer Subscribers
        protected virtual void InvokeEnterAction(PointerEventData eventData = null, UIEventArgs args = null) { }

        protected virtual void InvokeExitAction(PointerEventData eventData = null, UIEventArgs args = null) { }

        protected virtual void InvokeDownAction(PointerEventData eventData = null, UIEventArgs args = null) { }

        protected virtual void InvokeUpAction(PointerEventData eventData = null, UIEventArgs args = null) { }

        protected virtual void InvokeClickAction(PointerEventData eventData = null, UIEventArgs args = null) { }

        protected virtual void InvokeBeginDragAction(PointerEventData eventData = null, UIEventArgs args = null) { }

        protected virtual void InvokeDragAction(PointerEventData eventData = null, UIEventArgs args = null) { }

        protected virtual void InvokeEndDragAction(PointerEventData eventData = null, UIEventArgs args = null) { }

        #endregion

        #region Behaviors
        protected virtual void HoveredBehavior(bool playAudio = true, bool invokeEvent = true) { }

        protected virtual void PressedBehavior(bool playAudio = true, bool invokeEvent = true) { }

        protected virtual void SelectedBehavior(bool playAudio = true, bool invokeEvent = true) { }

        protected virtual void BeginDragBehavior(PointerEventData eventData, bool invokeEvent = true) { }

        protected virtual void DragBehavior(PointerEventData eventData, bool playAudio = true, bool invokeEvent = true) { }

        protected virtual void EndDragBehavior(PointerEventData eventData, bool invokeEvent = true) { }

        #endregion
    }

    public enum UIInteractionState { None = -1, Hovered, Pressed, Selected, Drag }
}
