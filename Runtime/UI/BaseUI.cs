using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CollieMollie.UI
{
    public abstract class BaseUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        #region Variable Field
        [Header("BaseUI")]
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
        #endregion

        #region Pointer Callbacks
        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData) => InvokeEnterAction(eventData, new UIEventArgs());

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData) => InvokeExitAction(eventData, new UIEventArgs());

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData) => InvokeDownAction(eventData, new UIEventArgs());

        void IPointerUpHandler.OnPointerUp(PointerEventData eventData) => InvokeUpAction(eventData, new UIEventArgs());

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData) => InvokeClickAction(eventData, new UIEventArgs());

        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData) => BeginDragAction(eventData, new UIEventArgs());

        void IDragHandler.OnDrag(PointerEventData eventData) => DragAction(eventData, new UIEventArgs());

        void IEndDragHandler.OnEndDrag(PointerEventData eventData) => EndDragAction(eventData, new UIEventArgs());
        #endregion

        #region Pointer Subscribers
        protected virtual void InvokeEnterAction(PointerEventData eventData = null, UIEventArgs args = null)
        {

        }

        protected virtual void InvokeExitAction(PointerEventData eventData = null, UIEventArgs args = null)
        {

        }

        protected virtual void InvokeDownAction(PointerEventData eventData = null, UIEventArgs args = null)
        {

        }

        protected virtual void InvokeUpAction(PointerEventData eventData = null, UIEventArgs args = null)
        {

        }

        protected virtual void InvokeClickAction(PointerEventData eventData = null, UIEventArgs args = null)
        {

        }

        protected virtual void BeginDragAction(PointerEventData eventData = null, UIEventArgs args = null)
        {

        }

        protected virtual void DragAction(PointerEventData eventData = null, UIEventArgs args = null)
        {

        }

        protected virtual void EndDragAction(PointerEventData eventData = null, UIEventArgs args = null)
        {

        }
        #endregion
    }

    public class UIEventArgs
    {
        public BaseUI Sender = null;

        public UIEventArgs() { }

        public UIEventArgs(BaseUI sender)
        {
            this.Sender = sender;
        }

        public bool IsValid()
        {
            return Sender != null;
        }
    }
}
