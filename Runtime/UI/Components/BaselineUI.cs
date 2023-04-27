using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Broccollie.UI
{
    public abstract class BaselineUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,
        IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
    {
        #region Variable Field
        public event Action<BaselineUI, EventArgs> OnDefault = null;
        public event Action<BaselineUI, EventArgs> OnShow = null;
        public event Action<BaselineUI, EventArgs> OnHide = null;
        public event Action<BaselineUI, EventArgs> OnInteractive = null;
        public event Action<BaselineUI, EventArgs> OnNonInteractive = null;
        public event Action<BaselineUI, EventArgs> OnHover = null;
        public event Action<BaselineUI, EventArgs> OnPress = null;
        public event Action<BaselineUI, EventArgs> OnSelect = null;

        [Header("Base")]
#if UNITY_EDITOR
        [SerializeField] protected bool _autoUpdate = true;
#endif
        [SerializeField] protected UIStates _currentState = UIStates.Default;
        public UIStates CurrentState
        {
            get => _currentState;
        }

        [SerializeField] protected List<UIBaseFeature> _features = null;

        protected bool _isActive = true;
        public bool IsActive
        {
            get => _isActive;
        }

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

        #region Publishers
        protected virtual void RaiseOnDefault(BaselineUI sender, EventArgs args) => OnDefault?.Invoke(this, args);

        protected virtual void RaiseOnShow(BaselineUI sender, EventArgs args) => OnShow?.Invoke(this, args);

        protected virtual void RaiseOnHide(BaselineUI sender, EventArgs args) => OnHide?.Invoke(this, args);

        protected virtual void RaiseOnInteractive(BaselineUI sender, EventArgs args) => OnInteractive?.Invoke(this, args);

        protected virtual void RaiseOnNonInteractive(BaselineUI sender, EventArgs args) => OnNonInteractive?.Invoke(this, args);

        protected virtual void RaiseOnHover(BaselineUI sender, EventArgs args) => OnHover?.Invoke(this, args);

        protected virtual void RaiseOnPress(BaselineUI sender, EventArgs args) => OnPress?.Invoke(this, args);

        protected virtual void RaiseOnSelect(BaselineUI sender, EventArgs args) => OnSelect?.Invoke(this, args);

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

        #region Public Functions
        public virtual void SetVisible(bool state, bool playAudio = true, bool invokeEvent = true) { }

        public virtual void SetVisibleInstant(bool state, bool playAudio = true, bool invokeEvent = true) { }

        public virtual void SetInteractive(bool state, bool playAudio = true, bool invokeEvent = true) { }

        public virtual void SetInteractiveInstant(bool state, bool playAudio = true, bool invokeEvent = true) { }

        #endregion

        #region Features
        protected virtual async Task ExecuteFeaturesAsync(UIStates state, CancellationToken ct, bool playAudio = true, Action done = null)
        {
            if (_features == null) return;

            List<Task> featureTasks = new List<Task>();
            foreach (UIBaseFeature feature in _features)
            {
                if (feature.FeatureType == FeatureTypes.Audio && !playAudio) continue;
                featureTasks.Add(feature.ExecuteFeaturesAsync(state, ct));
            }
            
            await Task.WhenAll(featureTasks);
            done?.Invoke();
        }

        protected virtual void ExecuteFeatureInstant(UIStates state, bool playAudio = true, Action done = null)
        {
            if (_features == null) return;

            foreach (UIBaseFeature feature in _features)
            {
                if (feature == null || feature.FeatureType == FeatureTypes.Audio && !playAudio) continue;
                feature.ExecuteFeatureInstant(state);
            }
            done?.Invoke();
        }

        #endregion

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (!_autoUpdate) return;

            switch (_currentState)
            {
                case UIStates.Show:
                    _isActive = _isInteractive = true;
                    _isHovered = _isPressed = _isSelected = false;
                    SetVisibleInstant(true);
                    break;

                case UIStates.Hide:
                    _isActive = _isInteractive = _isHovered = _isPressed = _isSelected = false;
                    SetVisibleInstant(false);
                    break;

                case UIStates.Interactive:
                    _isActive = _isInteractive = true;
                    _isHovered = _isPressed = _isSelected = false;
                    ExecuteFeatureInstant(UIStates.Interactive, false);
                    break;

                case UIStates.NonInteractive:
                    _isActive = true;
                    _isInteractive = _isHovered = _isPressed = _isSelected = false;
                    ExecuteFeatureInstant(UIStates.NonInteractive, false);
                    break;

                case UIStates.Default:
                    _isActive = _isInteractive = true;
                    _isHovered = _isPressed = _isSelected = false;
                    ExecuteFeatureInstant(UIStates.Default, false);
                    break;

                case UIStates.Hover:
                    _isActive = _isInteractive = _isHovered = true;
                    _isPressed = _isSelected = false;
                    ExecuteFeatureInstant(UIStates.Hover, false);
                    break;

                case UIStates.Press:
                    _isActive = _isInteractive = _isPressed = true;
                    _isHovered = _isSelected = false;
                    ExecuteFeatureInstant(UIStates.Press, false);
                    break;

                case UIStates.Select:
                    _isActive = _isInteractive = _isSelected = true;
                    _isHovered = _isPressed = false;
                    ExecuteFeatureInstant(UIStates.Select, false);
                    break;
            }
            UnityEditor.EditorUtility.SetDirty(this);
        }

        public void HideFeatureComponentsEditor()
        {
            Component[] featureComponents = GetComponents(typeof(UIBaseFeature));
            for (int i = 0; i < featureComponents.Length; i++)
            {
                if (featureComponents[i].hideFlags != HideFlags.HideInInspector)
                    featureComponents[i].hideFlags = HideFlags.HideInInspector;
            }
        }

        public void AddFeatureComponentEditor<T>() where T : UIBaseFeature
        {
            if (TryGetComponent<T>(out T component)) return;
            gameObject.AddComponent<T>();
        }

        public void RemoveFeatureComponentEditor<T>() where T : UIBaseFeature
        {
            if (!TryGetComponent<T>(out T component)) return;
            Destroy(component);
        }

        public bool CheckComponentEditor<T>() where T : UIBaseFeature
        {
            return TryGetComponent<T>(out T component);
        }
#endif
    }

    public enum UIStates { Default, Show, Hide, Interactive, NonInteractive, Hover, Press, Select }
}
