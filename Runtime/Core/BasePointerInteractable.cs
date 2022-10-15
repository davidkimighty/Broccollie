using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CollieMollie.Core
{
    public abstract class BasePointerInteractable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
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
        }

        protected virtual void InvokeExitAction(PointerEventData eventData = null)
        {
            if (this.GetType() == typeof(BasePointerInteractable)) return;
        }

        protected virtual void InvokeDownAction(PointerEventData eventData = null)
        {
            if (this.GetType() == typeof(BasePointerInteractable)) return;
        }

        protected virtual void InvokeUpAction(PointerEventData eventData = null)
        {
            if (this.GetType() == typeof(BasePointerInteractable)) return;
        }

        protected virtual void InvokeClickAction(PointerEventData eventData = null)
        {
            if (this.GetType() == typeof(BasePointerInteractable)) return;
        }

        protected virtual void BeginDragAction(PointerEventData eventData = null)
        {
            if (this.GetType() == typeof(BasePointerInteractable)) return;
        }

        protected virtual void DragAction(PointerEventData eventData = null)
        {
            if (this.GetType() == typeof(BasePointerInteractable)) return;
        }

        protected virtual void EndDragAction(PointerEventData eventData = null)
        {
            if (this.GetType() == typeof(BasePointerInteractable)) return;
        }
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
