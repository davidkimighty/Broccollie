using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CollieMollie.UI
{
    [DefaultExecutionOrder(-100)]
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

        #endregion

        protected override void Awake()
        {
            if (_selected)
                ChangeState(State.Selected);
            base.Awake();
        }

        #region Public Functions
        public override void ChangeState(State state, bool playAudio = true, bool invokeEvent = true)
        {
            base.ChangeState(state, playAudio, invokeEvent);

            if (state == State.None || state == _currentState) return;
            
            switch (state)
            {
                case State.Hovered:
                    _interactable = true; _hovering = true; _pressed = false;
                    HoveredBehavior(playAudio, invokeEvent);
                    break;

                case State.Pressed:
                    _interactable = true; _pressed = true;
                    PressedBehavior(playAudio, invokeEvent);
                    break;

                case State.Selected:
                    _interactable = true; _hovering = false; _pressed = false; _selected = true;
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
        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData) => InvokeEnterAction(eventData);

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData) => InvokeExitAction(eventData);

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData) => InvokeDownAction(eventData);

        void IPointerUpHandler.OnPointerUp(PointerEventData eventData) => InvokeUpAction(eventData);

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData) => InvokeClickAction(eventData);

        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData) => InvokeBeginDragAction(eventData);

        void IDragHandler.OnDrag(PointerEventData eventData) => InvokeDragAction(eventData);

        void IEndDragHandler.OnEndDrag(PointerEventData eventData) => InvokeEndDragAction(eventData);

        #endregion

        #region Pointer Subscribers
        protected virtual void InvokeEnterAction(PointerEventData eventData = null) { }

        protected virtual void InvokeExitAction(PointerEventData eventData = null) { }

        protected virtual void InvokeDownAction(PointerEventData eventData = null) { }

        protected virtual void InvokeUpAction(PointerEventData eventData = null) { }

        protected virtual void InvokeClickAction(PointerEventData eventData = null) { }

        protected virtual void InvokeBeginDragAction(PointerEventData eventData = null) { }

        protected virtual void InvokeDragAction(PointerEventData eventData = null) { }

        protected virtual void InvokeEndDragAction(PointerEventData eventData = null) { }

        #endregion

        #region Behaviors
        protected virtual void HoveredBehavior(bool playAudio = true, bool invokeEvent = true, Action done = null) { }

        protected virtual void PressedBehavior(bool playAudio = true, bool invokeEvent = true, Action done = null) { }

        protected virtual void SelectedBehavior(bool playAudio = true, bool invokeEvent = true, Action done = null) { }

        protected virtual void BeginDragBehavior(PointerEventData eventData, bool invokeEvent = true) { }

        protected virtual void DragBehavior(PointerEventData eventData, bool playAudio = true, bool invokeEvent = true) { }

        protected virtual void EndDragBehavior(PointerEventData eventData, bool invokeEvent = true) { }

        #endregion
    }
}
