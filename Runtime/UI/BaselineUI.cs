using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Broccollie.UI
{
    public abstract class BaselineUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
    {
        #region Variable Field
        public event Action OnDefault = null;
        public event Func<Task> OnDefaultAsync = null;

        public event Action OnShow = null;
        public event Func<Task> OnShowAsync = null;

        public event Action OnHide = null;
        public event Func<Task> OnHideAsync = null;

        public event Action OnInteractive = null;
        public event Func<Task> OnInteractiveAsync = null;

        public event Action OnNonInteractive = null;
        public event Func<Task> OnNonInteractiveAsync = null;

        public event Action OnHover = null;
        public event Func<Task> OnHoverAsync = null;

        public event Action OnPress = null;
        public event Func<Task> OnPressAsync = null;

        public event Action OnSelect = null;
        public event Func<Task> OnSelectAsync = null;

        protected bool _isInteractive = true;
        protected bool _isHovered = false;
        protected bool _isPressed = false;
        protected bool _isSelected = false;

        #endregion

        #region Public Functions
        public virtual void SetActive(bool state) { }

        public virtual void SetInteractive(bool state) { }

        #endregion

        #region Publishers
        protected void RaiseOnDefault() => OnDefault?.Invoke();
        protected void RaiseOnDefaultAsync() => OnDefaultAsync?.Invoke();

        protected void RaiseOnShow() => OnShow?.Invoke();
        protected void RaiseOnShowAsync() => OnShowAsync?.Invoke();

        protected void RaiseOnHide() => OnHide?.Invoke();
        protected void RaiseOnHideAsync() => OnHideAsync?.Invoke();

        protected void RaiseOnInteractive() => OnInteractive?.Invoke();
        protected void RaiseOnInteractiveAsync() => OnInteractiveAsync?.Invoke();

        protected void RaiseOnNonInteractive() => OnNonInteractive?.Invoke();
        protected void RaiseOnNonInteractiveAsync() => OnNonInteractiveAsync?.Invoke();

        protected void RaiseOnHover() => OnHover?.Invoke();
        protected void RaiseOnHoverAsync() => OnHoverAsync?.Invoke();

        protected void RaiseOnPress() => OnPress?.Invoke();
        protected void RaiseOnPressAsync() => OnPressAsync?.Invoke();

        protected void RaiseOnSelect() => OnSelect?.Invoke();
        protected void RaiseOnSelectAsync() => OnSelectAsync?.Invoke();

        #endregion

        #region Pointer Callbacks
        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData) => InvokePointerEnter(eventData, null);

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData) => InvokePointerExit(eventData, null);

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData) => InvokePointerDown(eventData, null);

        void IPointerUpHandler.OnPointerUp(PointerEventData eventData) => InvokePointerUp(eventData, null);

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData) => InvokePointerClick(eventData, null);

        #endregion

        #region Pointer Callback Subscribers
        protected virtual void InvokePointerEnter(PointerEventData eventData, BaselineUI baselineUI)
        {
            if (baselineUI == null) return;
        }

        protected virtual void InvokePointerExit(PointerEventData eventData, BaselineUI baselineUI)
        {
            if (baselineUI == null) return;
        }

        protected virtual void InvokePointerDown(PointerEventData eventData, BaselineUI baselineUI)
        {
            if (baselineUI == null) return;
        }

        protected virtual void InvokePointerUp(PointerEventData eventData, BaselineUI baselineUI)
        {
            if (baselineUI == null) return;
        }

        protected virtual void InvokePointerClick(PointerEventData eventData, BaselineUI baselineUI)
        {
            if (baselineUI == null) return;
        }

        #endregion
    }

    public enum UIStates { Default, Show, Hide, Interactive, NonInteractive, Hover, Press, Select }
}
