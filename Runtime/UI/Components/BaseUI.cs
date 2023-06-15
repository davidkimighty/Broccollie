using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Broccollie.UI
{
    public abstract class BaseUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,
        IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, IMoveHandler,
        ISelectHandler, IDeselectHandler, ISubmitHandler
    {
        #region Variable Field
        public event Action<BaseUI, EventArgs> OnDefault = null;
        public event Action<BaseUI, EventArgs> OnShow = null;
        public event Action<BaseUI, EventArgs> OnHide = null;
        public event Action<BaseUI, EventArgs> OnInteractive = null;
        public event Action<BaseUI, EventArgs> OnNonInteractive = null;
        public event Action<BaseUI, EventArgs> OnHover = null;
        public event Action<BaseUI, EventArgs> OnPress = null;
        public event Action<BaseUI, EventArgs> OnClick = null;

        [Header("Base")]
        [SerializeField] protected UIStates _currentState = UIStates.Default;
        public UIStates CurrentState
        {
            get => _currentState;
        }

        [SerializeField] protected List<BaseUIFeature> _features = null;
        public List<BaseUIFeature> Features
        {
            get => _features;
        }

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

        protected bool _isClicked = false;
        public bool IsSelected
        {
            get => _isClicked;
        }

        protected Task _featureTasks = null;
        protected CancellationTokenSource _cts = new CancellationTokenSource();

        #endregion

        #region Publishers
        protected virtual void RaiseOnDefault(BaseUI sender, EventArgs args) => OnDefault?.Invoke(this, args);

        protected virtual void RaiseOnShow(BaseUI sender, EventArgs args) => OnShow?.Invoke(this, args);

        protected virtual void RaiseOnHide(BaseUI sender, EventArgs args) => OnHide?.Invoke(this, args);

        protected virtual void RaiseOnInteractive(BaseUI sender, EventArgs args) => OnInteractive?.Invoke(this, args);

        protected virtual void RaiseOnNonInteractive(BaseUI sender, EventArgs args) => OnNonInteractive?.Invoke(this, args);

        protected virtual void RaiseOnHover(BaseUI sender, EventArgs args) => OnHover?.Invoke(this, args);

        protected virtual void RaiseOnPress(BaseUI sender, EventArgs args) => OnPress?.Invoke(this, args);

        protected virtual void RaiseOnClick(BaseUI sender, EventArgs args) => OnClick?.Invoke(this, args);

        #endregion

        #region Pointer Callbacks
        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData) => InvokePointerEnter(eventData, null);

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData) => InvokePointerExit(eventData, null);

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData) => InvokePointerDown(eventData, null);

        void IPointerUpHandler.OnPointerUp(PointerEventData eventData) => InvokePointerUp(eventData, null);

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData) => InvokePointerClick(eventData, null);

        void IMoveHandler.OnMove(AxisEventData eventData) => InvokeMove(eventData, null, null);

        void ISelectHandler.OnSelect(BaseEventData eventData) => InvokeSelect(eventData, null);

        void IDeselectHandler.OnDeselect(BaseEventData eventData) => InvokeDeselect(eventData, null);

        void ISubmitHandler.OnSubmit(BaseEventData eventData) => InvokeSubmit(eventData, null);

        #endregion

        #region Pointer Callback Subscribers
        protected virtual void InvokePointerEnter(PointerEventData eventData, BaseUI invoker)
        {
            if (invoker == null) return;
        }

        protected virtual void InvokePointerExit(PointerEventData eventData, BaseUI invoker)
        {
            if (invoker == null) return;
        }

        protected virtual void InvokePointerDown(PointerEventData eventData, BaseUI invoker)
        {
            if (invoker == null) return;
        }

        protected virtual void InvokePointerUp(PointerEventData eventData, BaseUI invoker)
        {
            if (invoker == null) return;
        }

        protected virtual void InvokePointerClick(PointerEventData eventData, BaseUI invoker)
        {
            if (invoker == null) return;
        }

        protected virtual void InvokeMove(AxisEventData eventData, BaseUI invoker, List<BaseUI> activeList)
        {
            if (invoker == null) return;

            BaseUI nextSelectable = null;
            switch (eventData.moveDir)
            {
                case MoveDirection.Left:
                    nextSelectable = GetNextSelectable(transform.rotation * Vector3.left);
                    ChangeSelectedObject(nextSelectable);
                    break;

                case MoveDirection.Right:
                    nextSelectable = GetNextSelectable(transform.rotation * Vector3.right);
                    ChangeSelectedObject(nextSelectable);
                    break;

                case MoveDirection.Up:
                    nextSelectable = GetNextSelectable(transform.rotation * Vector3.up);
                    ChangeSelectedObject(nextSelectable);
                    break;

                case MoveDirection.Down:
                    nextSelectable = GetNextSelectable(transform.rotation * Vector3.down);
                    ChangeSelectedObject(nextSelectable);
                    break;
            }

            BaseUI GetNextSelectable(Vector3 dir)
            {
                dir = dir.normalized;
                Vector3 localDir = Quaternion.Inverse(transform.rotation) * dir;
                Vector3 pos = transform.TransformPoint(GetPointOnRectEdge(transform as RectTransform, localDir));
                float maxScore = Mathf.NegativeInfinity;
                BaseUI bestPick = null;

                foreach (BaseUI selectable in activeList)
                {
                    if (selectable == this || selectable == null) continue;

                    if (!selectable._isInteractive) continue; // adding navigation mode?

                    RectTransform rect = selectable.transform as RectTransform;
                    Vector3 center = rect != null ? (Vector3)rect.rect.center : Vector3.zero;
                    Vector3 calcDir = selectable.transform.TransformPoint(center) - pos;

                    float dot = Vector3.Dot(dir, calcDir);
                    if (dot <= 0) continue;

                    float score = dot / calcDir.sqrMagnitude;
                    if (score > maxScore)
                    {
                        maxScore = score;
                        bestPick = selectable;
                    }
                }
                return bestPick;
            }

            Vector3 GetPointOnRectEdge(RectTransform rect, Vector2 dir)
            {
                if (rect == null)
                    return Vector3.zero;
                if (dir != Vector2.zero)
                    dir /= Mathf.Max(Mathf.Abs(dir.x), Mathf.Abs(dir.y));
                dir = rect.rect.center + Vector2.Scale(rect.rect.size, dir * 0.5f);
                return dir;
            }

            void ChangeSelectedObject(BaseUI selected)
            {
                if (selected != null && enabled && isActiveAndEnabled && gameObject.activeInHierarchy)
                    eventData.selectedObject = selected.gameObject;
            }
        }

        protected virtual void InvokeSelect(BaseEventData eventData, BaseUI invoker)
        {
            if (invoker == null) return;
        }

        protected virtual void InvokeDeselect(BaseEventData eventData, BaseUI invoker)
        {
            if (invoker == null) return;
        }

        protected virtual void InvokeSubmit(BaseEventData eventData, BaseUI invoker)
        {
            if (invoker == null) return;
        }

        #endregion

        #region Public Functions
        public virtual void SetVisible(bool state, bool playAudio = true, bool invokeEvent = true, bool instant = false) { }

        public virtual void SetInteractive(bool state, bool playAudio = true, bool invokeEvent = true, bool instant = false) { }

        #endregion

        #region Features
        protected virtual async Task ExecuteFeaturesAsync(UIStates state, bool playAudio = true, Action done = null)
        {
            if (_features == null) return;

            try
            {
                _cts.Cancel();
                _cts = new CancellationTokenSource();

                List<Task> featureTasks = new List<Task>();
                foreach (BaseUIFeature feature in _features)
                {
                    if (feature.FeatureType == FeatureTypes.Audio && !playAudio) continue;
                    featureTasks.Add(feature.ExecuteAsync(state, _cts.Token));
                }

                await Task.WhenAll(featureTasks);
            }
            catch (OperationCanceledException e)
            {
                
            }
            done?.Invoke();
        }

        protected virtual void ExecuteFeatureInstant(UIStates state, bool playAudio = true, Action done = null)
        {
            if (_features == null) return;

            _cts.Cancel();

            foreach (BaseUIFeature feature in _features)
            {
                if (feature == null || feature.FeatureType == FeatureTypes.Audio && !playAudio) continue;
                feature.ExecuteInstant(state);
            }
            done?.Invoke();
        }

        #endregion

#if UNITY_EDITOR
        private void OnValidate()
        {
            switch (_currentState)
            {
                case UIStates.Show:
                    _isActive = true;
                    _isHovered = _isPressed = _isClicked = false;
                    SetVisible(true, true, true, true);
                    break;

                case UIStates.Hide:
                    _isActive = _isHovered = _isPressed = _isClicked = false;
                    SetVisible(false, true, true, true);
                    break;

                case UIStates.Interactive:
                    _isActive = _isInteractive = true;
                    _isHovered = _isPressed = _isClicked = false;
                    ExecuteFeatureInstant(UIStates.Interactive, false);
                    break;

                case UIStates.NonInteractive:
                    _isActive = true;
                    _isInteractive = _isHovered = _isPressed = _isClicked = false;
                    ExecuteFeatureInstant(UIStates.NonInteractive, false);
                    break;

                case UIStates.Default:
                    _isActive = true;
                    _isHovered = _isPressed = _isClicked = false;
                    ExecuteFeatureInstant(UIStates.Default, false);
                    break;

                case UIStates.Hover:
                    _isActive = _isHovered = true;
                    _isPressed = _isClicked = false;
                    ExecuteFeatureInstant(UIStates.Hover, false);
                    break;

                case UIStates.Press:
                    _isActive = _isPressed = true;
                    _isHovered = _isClicked = false;
                    ExecuteFeatureInstant(UIStates.Press, false);
                    break;

                case UIStates.Click:
                    _isActive = _isClicked = true;
                    _isHovered = _isPressed = false;
                    ExecuteFeatureInstant(UIStates.Click, false);
                    break;
            }
            UnityEditor.EditorUtility.SetDirty(this);
        }

        public void HideFeatureComponentsEditor()
        {
            Component[] featureComponents = GetComponents(typeof(BaseUIFeature));
            for (int i = 0; i < featureComponents.Length; i++)
            {
                if (featureComponents[i].hideFlags != HideFlags.HideInInspector)
                    featureComponents[i].hideFlags = HideFlags.HideInInspector;
            }
        }

        public void AddFeatureComponentEditor<T>() where T : BaseUIFeature
        {
            if (TryGetComponent<T>(out T component)) return;
            gameObject.AddComponent<T>();
        }

        public void RemoveFeatureComponentEditor<T>() where T : BaseUIFeature
        {
            if (!TryGetComponent<T>(out T component)) return;
            Destroy(component);
        }

        public bool CheckComponentEditor<T>() where T : BaseUIFeature
        {
            return TryGetComponent<T>(out T component);
        }
#endif
    }

    public enum UIStates { Default, Show, Hide, Interactive, NonInteractive, Hover, Press, Click }
}
