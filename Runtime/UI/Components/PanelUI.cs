using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Broccollie.UI
{
    public class PanelUI : BaseUIElement, IActive, IInteractive, IDefault
    {
        private static readonly List<BaseUIElement> s_activePanels = new();

        public event Action<UIEventArgs> OnActive;
        public event Action<UIEventArgs> OnInActive;
        public event Action<UIEventArgs> OnInteractive;
        public event Action<UIEventArgs> OnNonInteractive;
        public event Action<UIEventArgs> OnDefault;

        [SerializeField] private GameObject _panel;

        private bool _isInteractive = true;
        public bool IsInteractive
        {
            get => _isInteractive;
        }

        private void OnEnable()
        {
            s_activePanels.Add(this);
        }

        private void OnDisable()
        {
            s_activePanels.Remove(this);
        }

        #region Public Functions
        public async Task SetActiveAsync(bool state, bool instantChange = false, bool playAudio = false, bool invokeEvent = false)
        {
            if (state)
                _panel.SetActive(true);
            _currentState = state ? UIStates.Active : UIStates.InActive;

            if (_features != null && _features.Count > 0)
            {
                await this.ExecuteBehaviorAsync(_currentState, instantChange, playAudio);

                if (state)
                    await this.ExecuteBehaviorAsync(UIStates.Default, instantChange, playAudio);
            }

            if (state)
                OnActive?.Invoke(new UIEventArgs { Sender = this });
            else
            {
                OnInActive?.Invoke(new UIEventArgs { Sender = this });
                _panel.SetActive(false);
            }
        }

        public async Task SetInteractiveAsync(bool state, bool instantChange = false, bool playAudio = false, bool invokeEvent = false)
        {
            _isInteractive = state;
            _currentState = state ? UIStates.Interactive : UIStates.NonInteractive;

            if (_features != null && _features.Count > 0)
            {
                await this.ExecuteBehaviorAsync(_currentState, instantChange, playAudio);

                if (state)
                    await this.ExecuteBehaviorAsync(UIStates.Default, instantChange, playAudio);
            }

            if (state)
                OnInteractive?.Invoke(new UIEventArgs { Sender = this });
            else
                OnNonInteractive?.Invoke(new UIEventArgs { Sender = this });
        }

        public async Task DefaultAsync(bool instantChange = false, bool playAudio = false, bool invokeEvent = false)
        {
            _currentState = UIStates.Default;

            if (_features != null && _features.Count > 0)
                await this.ExecuteBehaviorAsync(UIStates.Default, instantChange, playAudio);

            if (invokeEvent)
                OnDefault?.Invoke(new UIEventArgs { Sender = this });
        }

        #endregion


#if UNITY_EDITOR
        private void OnValidate()
        {
            ChangeUIStateEditor();
        }

        public override void ChangeUIStateEditor()
        {
            switch (_currentState)
            {
                case UIStates.InActive:
                    _ = this.ExecuteBehaviorAsync(UIStates.InActive, true, false);
                    gameObject.SetActive(false);
                    break;

                case UIStates.NonInteractive:
                    _isInteractive = false;
                    _ = this.ExecuteBehaviorAsync(UIStates.NonInteractive, true, false);
                    gameObject.SetActive(true);
                    break;

                case UIStates.Default:
                    _isInteractive = true;
                    _ = this.ExecuteBehaviorAsync(UIStates.Default, true, false);
                    gameObject.SetActive(true);
                    break;

            }
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
    }
}
