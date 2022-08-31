using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CollieMollie.UI
{
    public abstract class BaseUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        #region Variable Field
        public event Action<UIEventArgs> OnDefault = null;
        public event Action<UIEventArgs> OnHovered = null;
        public event Action<UIEventArgs> OnPressed = null;
        public event Action<UIEventArgs> OnSelected = null;
        public event Action<UIEventArgs> OnDisabled = null;

        public event Action<UIEventArgs> OnUIBeginDrag = null;
        public event Action<UIEventArgs> OnUIDrag = null;
        public event Action<UIEventArgs> OnUIEndDrag = null;

        [Header("BaseUI")]
        [SerializeField] protected bool isInteractable = true;
        public bool IsInteractable
        {
            get => isInteractable;
        }

        [SerializeField] protected bool isHovering = false;
        public bool IsHovering
        {
            get => IsHovering;
        }

        [SerializeField] protected bool isPressed = false;
        public bool IsPressed
        {
            get => isPressed;
        }

        [SerializeField] protected bool isSelected = false;
        public bool IsSelected
        {
            get => IsSelected;
        }

        [SerializeField] protected bool isDragging = false;
        public bool IsDragging
        {
            get => isDragging;
        }
        #endregion

        #region Pointer Callbacks
        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData) => InvokeHoverInAction(eventData);

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData) => InvokeDefaultAction(eventData);

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData) => InvokePressAction(eventData);

        void IPointerUpHandler.OnPointerUp(PointerEventData eventData) => InvokeDefaultAction(eventData);

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData) => InvokeSelectAction(eventData);

        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData) => BeginDragAction(eventData);

        void IDragHandler.OnDrag(PointerEventData eventData) => DragAction(eventData);

        void IEndDragHandler.OnEndDrag(PointerEventData eventData) => EndDragAction(eventData);
        #endregion

        #region Interaction Publishers
        protected virtual void InvokeDefaultAction(PointerEventData eventData = null, UIEventArgs args = null)
        {
            if (!IsValid(args)) return;

            OnDefault?.Invoke(args);
        }

        protected virtual void InvokeHoverInAction(PointerEventData eventData = null, UIEventArgs args = null)
        {
            if (!IsValid(args)) return;

            OnHovered?.Invoke(args);
        }

        protected virtual void InvokePressAction(PointerEventData eventData = null, UIEventArgs args = null)
        {
            if (!IsValid(args)) return;

            OnPressed?.Invoke(args);
        }

        protected virtual void InvokeSelectAction(PointerEventData eventData = null, UIEventArgs args = null)
        {
            if (!IsValid(args)) return;

            OnSelected?.Invoke(args);
        }

        protected virtual void InvokeDisableAction(UIEventArgs args = null)
        {
            if (!IsValid(args)) return;

            OnDisabled?.Invoke(args);
        }

        protected virtual void BeginDragAction(PointerEventData eventData = null, UIEventArgs args = null)
        {
            if (!IsValid(args)) return;

            OnUIBeginDrag?.Invoke(args);
        }

        protected virtual void DragAction(PointerEventData eventData = null, UIEventArgs args = null)
        {
            if (!IsValid(args)) return;

            OnUIDrag?.Invoke(args);
        }

        protected virtual void EndDragAction(PointerEventData eventData = null, UIEventArgs args = null)
        {
            if (!IsValid(args)) return;

            OnUIEndDrag?.Invoke(args);
        }
        #endregion

        #region BaseUI Features
        /// <summary>
        /// Check if UI event argument is valid.
        /// To publish an event, the argument itself and the sender can not be null.
        /// </summary>
        private bool IsValid(UIEventArgs args)
        {
            return args != null && args.sender != null;
        }
        #endregion
    }

    public class UIEventArgs
    {
        public BaseUI sender = null;

        public UIEventArgs(BaseUI sender)
        {
            this.sender = sender;
        }
    }
}
