using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Broccollie.UI
{
    public abstract class BaseUIElement : MonoBehaviour
    {
        [SerializeField] protected UIStates _currentState = UIStates.Default;
        public UIStates CurrentState
        {
            get => _currentState;
        }
        [SerializeField] protected List<BaseUIFeature> _features = new();
        public List<BaseUIFeature> Features
        {
            get => _features;
        }

        protected bool _isRaycastInteractive = true;
        public bool IsRaycastInteractive
        {
            get => _isRaycastInteractive;
        }

        protected CancellationTokenSource _cts = new();
        public CancellationTokenSource Cts
        {
            get => _cts;
        }

        #region Public Functions
        public virtual void SetRaycastInteractive(bool state) => _isRaycastInteractive = state;

        public virtual void CancelCancellationToken() => _cts.Cancel();

        public virtual void RenewCancelToken() => _cts = new();

        #endregion

#if UNITY_EDITOR
        public virtual void ChangeUIStateEditor() { }

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

    public struct UIEventArgs
    {
        public BaseUIElement Sender;
    }

    public enum UIStates { Default, Active, InActive, Interactive, NonInteractive, Hover, Press, Select }
}
