using System;
using System.Collections;
using System.Collections.Generic;
using CollieMollie.Core;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CollieMollie.Interactable
{
    public class InteractableObject : BasePointerInteractable
    {
        #region Variable Field
        public event Action<InteractableEventArgs> OnDefault = null;
        public event Action<InteractableEventArgs> OnHovered = null;
        public event Action<InteractableEventArgs> OnPressed = null;
        public event Action<InteractableEventArgs> OnBeginDrag = null;
        public event Action<InteractableEventArgs> OnEndDrag = null;

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

        #region Publishers
        protected override void InvokeEnterAction(PointerEventData eventData = null)
        {
            if (!_interactable) return;

            _hovering = true;
            if (_selected) return;

            

            OnHovered?.Invoke(new InteractableEventArgs(this));
            //Debug.Log("[InteractableObject] Invoke Hovered");
        }

        protected override void InvokeExitAction(PointerEventData eventData = null)
        {
            if (!_interactable) return;

            _hovering = false;
            if (_selected) return;

        }

        protected override void InvokeDownAction(PointerEventData eventData = null)
        {
            if (!_interactable) return;

            _pressed = true;
        }

        protected override void InvokeUpAction(PointerEventData eventData = null)
        {
            _pressed = false;

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
