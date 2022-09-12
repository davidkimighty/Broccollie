using System;
using System.Collections;
using System.Collections.Generic;
using CollieMollie.Editor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CollieMollie.Interactable
{
    public class InteractableObject : BaseInteractable
    {
        #region Variable Field
        public event Action OnDefault = null;
        public event Action OnHovered = null;
        public event Action OnPressed = null;
        public event Action OnBeginDrag = null;
        public event Action OnEndDrag = null;

        #endregion

        #region Publishers
        protected override void InvokeEnterAction(PointerEventData eventData = null)
        {
            base.InvokeEnterAction(eventData);
        }

        protected override void InvokeExitAction(PointerEventData eventData = null)
        {
            base.InvokeExitAction(eventData);
        }

        protected override void InvokeDownAction(PointerEventData eventData = null)
        {
            base.InvokeDownAction(eventData);
        }

        protected override void InvokeUpAction(PointerEventData eventData = null)
        {
            base.InvokeUpAction(eventData);
        }

        protected override void InvokeClickAction(PointerEventData eventData = null)
        {
            base.InvokeClickAction(eventData);
        }

        protected override void BeginDragAction(PointerEventData eventData = null)
        {
            base.BeginDragAction(eventData);
        }

        protected override void DragAction(PointerEventData eventData = null)
        {
            base.DragAction(eventData);
        }

        protected override void EndDragAction(PointerEventData eventData = null)
        {
            base.EndDragAction(eventData);
        }
        #endregion
    }
}
