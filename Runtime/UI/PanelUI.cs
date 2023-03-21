using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Broccollie.UI
{
    [DefaultExecutionOrder(-100)]
    public class PanelUI : BaselineUI
    {
        #region Variable Field
        [Header("Button")]
        [SerializeField] private GameObject _panel = null;

        [Header("Features")]
        [SerializeField] private UIColorFeature _colorFeature = null;
        [SerializeField] private UISpriteFeature _spriteFeature = null;
        [SerializeField] private UITransformFeature _transformFeature = null;
        [SerializeField] private UIAudioFeature _audioFeature = null;

        private Task _featureTasks = null;

        #endregion

        #region Public Functions
        public override void SetActive(bool state, bool playAudio = false, bool invokeEvent = true)
        {
            if (state)
            {
                if (_currentState == UIStates.Show) return;
                _currentState = UIStates.Show;

                _panel.SetActive(true);
                if (invokeEvent)
                {
                    RaiseOnShow();
                    Task.Run(() => RaiseOnShowAsync());
                }

                _featureTasks = ExecuteFeaturesAsync(UIStates.Show, playAudio, () =>
                {
                    _isInteractive = true;
                    _featureTasks = ExecuteFeaturesAsync(UIStates.Default);
                });
            }
            else
            {
                if (_currentState == UIStates.Hide) return;
                _currentState = UIStates.Hide;

                _isInteractive = false;
                if (invokeEvent)
                {
                    RaiseOnHide();
                    Task.Run(() => RaiseOnHideAsync());
                }

                _featureTasks = ExecuteFeaturesAsync(UIStates.Hide, playAudio, () =>
                {
                    _panel.SetActive(false);
                });
            }
        }

        public override void SetInteractive(bool state, bool playAudio = false, bool invokeEvent = true)
        {
            if (state)
            {
                if (_currentState == UIStates.Interactive) return;
                _currentState = UIStates.Interactive;

                if (!_panel.activeSelf)
                    _panel.SetActive(true);

                if (invokeEvent)
                {
                    RaiseOnInteractive();
                    Task.Run(() => RaiseOnInteractiveAsync());
                }

                _featureTasks = ExecuteFeaturesAsync(UIStates.Interactive, playAudio, () =>
                {
                    _isInteractive = true;
                });
            }
            else
            {
                if (_currentState == UIStates.NonInteractive) return;
                _currentState = UIStates.NonInteractive;

                if (!_panel.activeSelf)
                    _panel.SetActive(true);

                _isInteractive = false;
                if (invokeEvent)
                {
                    RaiseOnInteractive();
                    Task.Run(() => RaiseOnInteractiveAsync());
                }
                _featureTasks = ExecuteFeaturesAsync(UIStates.NonInteractive, playAudio);
            }
        }

        #endregion

        private void Awake()
        {
            switch (_currentState)
            {
                case UIStates.Show:
                    SetActive(true, false, false);
                    break;

                case UIStates.Hide:
                    SetActive(false, false, false);
                    break;

                case UIStates.Interactive:
                    SetInteractive(true, false, false);
                    break;

                case UIStates.NonInteractive:
                    SetInteractive(false, false, false);
                    break;
            }
        }

        #region Private Functions
        private async Task ExecuteFeaturesAsync(UIStates state, bool playAudio = true, Action done = null)
        {
            List<Task> featureTasks = new List<Task>();
            if (_colorFeature != null)
                featureTasks.Add(_colorFeature.ExecuteFeaturesAsync(state));

            if (_spriteFeature != null)
                featureTasks.Add(_spriteFeature.ExecuteFeaturesAsync(state));

            if (_transformFeature != null)
                featureTasks.Add(_transformFeature.ExecuteFeaturesAsync(state));

            if (_audioFeature != null && playAudio)
                featureTasks.Add(_audioFeature.ExecuteFeaturesAsync(state));

            await Task.WhenAll(featureTasks);
            done?.Invoke();
        }

        #endregion
    }
}
