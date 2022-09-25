using System;
using CollieMollie.Editor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CollieMollie.UI
{
    public abstract class BaseUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
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
            if (this.GetType() == typeof(BaseUI)) return;
        }

        protected virtual void InvokeExitAction(PointerEventData eventData = null)
        {
            if (this.GetType() == typeof(BaseUI)) return;
        }

        protected virtual void InvokeDownAction(PointerEventData eventData = null)
        {
            if (this.GetType() == typeof(BaseUI)) return;
        }

        protected virtual void InvokeUpAction(PointerEventData eventData = null)
        {
            if (this.GetType() == typeof(BaseUI)) return;
        }

        protected virtual void InvokeClickAction(PointerEventData eventData = null)
        {
            if (this.GetType() == typeof(BaseUI)) return;
        }

        protected virtual void BeginDragAction(PointerEventData eventData = null)
        {
            if (this.GetType() == typeof(BaseUI)) return;
        }

        protected virtual void DragAction(PointerEventData eventData = null)
        {
            if (this.GetType() == typeof(BaseUI)) return;
        }

        protected virtual void EndDragAction(PointerEventData eventData = null)
        {
            if (this.GetType() == typeof(BaseUI)) return;
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
