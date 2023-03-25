using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Broccollie.UI
{
    public abstract class BaselineUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
    {
        #region Variable Field
        public event Action<BaselineUI> OnDefault = null;
        public event Action<BaselineUI> OnShow = null;
        public event Action<BaselineUI> OnHide = null;
        public event Action<BaselineUI> OnInteractive = null;
        public event Action<BaselineUI> OnNonInteractive = null;
        public event Action<BaselineUI> OnHover = null;
        public event Action<BaselineUI> OnPress = null;
        public event Action<BaselineUI> OnSelect = null;

        [Header("Baseline")]
        [SerializeField] protected UIStates _currentState = UIStates.Default;

        protected bool _isInteractive = true;
        public bool IsInteractive
        {
            get => _isInteractive;
        }
        protected bool _isHovered = false;
        public bool IsHovered
        {
            get => _isHovered;
        }
        protected bool _isPressed = false;
        public bool IsPressed
        {
            get => _isPressed;
        }
        protected bool _isSelected = false;
        public bool IsSelected
        {
            get => _isSelected;
        }

        #endregion

        #region Public Functions
        public virtual void SetActive(bool state, bool playAudio = false, bool invokeEvent = true) { }

        public virtual void SetInteractive(bool state, bool playAudio = false, bool invokeEvent = true) { }

        #endregion

        #region Publishers
        protected void RaiseOnDefault() => OnDefault?.Invoke(this);

        protected void RaiseOnShow() => OnShow?.Invoke(this);

        protected void RaiseOnHide() => OnHide?.Invoke(this);

        protected void RaiseOnInteractive() => OnInteractive?.Invoke(this);

        protected void RaiseOnNonInteractive() => OnNonInteractive?.Invoke(this);

        protected void RaiseOnHover() => OnHover?.Invoke(this);

        protected void RaiseOnPress() => OnPress?.Invoke(this);

        protected void RaiseOnSelect() => OnSelect?.Invoke(this);

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
