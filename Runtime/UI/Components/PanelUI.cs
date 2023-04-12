using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Broccollie.UI
{
    [DefaultExecutionOrder(-120)]
    public class PanelUI : BaselineUI, IDefaultUI
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

                case UIStates.Default:
                    ExecuteFeatureInstant(UIStates.Default, false);
                    break;
            }
        }

        #region Public Functions
        public override void SetActive(bool state, bool playAudio = false, bool invokeEvent = true)
        {
            if (state)
            {
                _currentState = UIStates.Show;
                _isActive = true;
                _panel.SetActive(true);

                if (invokeEvent)
                    RaiseOnShow();

                _featureTasks = ExecuteFeaturesAsync(UIStates.Show, playAudio, () =>
                {
                    _isInteractive = true;
                    Default(playAudio, invokeEvent);
                });
            }
            else
            {
                _currentState = UIStates.Hide;
                _isActive = _isInteractive = false;

                if (invokeEvent)
                    RaiseOnHide();

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
                _currentState = UIStates.Interactive;
                if (!_panel.activeSelf)
                    _panel.SetActive(true);

                if (invokeEvent)
                    RaiseOnInteractive();

                _featureTasks = ExecuteFeaturesAsync(UIStates.Interactive, playAudio, () =>
                {
                    _isInteractive = true;
                    Default(playAudio, invokeEvent);
                });
            }
            else
            {
                _currentState = UIStates.NonInteractive;
                if (!_panel.activeSelf)
                    _panel.SetActive(true);

                _isInteractive = false;
                if (invokeEvent)
                    RaiseOnInteractive();

                _featureTasks = ExecuteFeaturesAsync(UIStates.NonInteractive, playAudio);
            }
        }

        public void Default(bool playAudio = false, bool invokeEvent = true)
        {
            if (!_isInteractive) return;

            _currentState = UIStates.Default;
            _isHovered = _isPressed = _isSelected = false;
            if (invokeEvent)
                RaiseOnDefault();

            _featureTasks = ExecuteFeaturesAsync(UIStates.Default, playAudio);
        }

        #endregion

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

        private void ExecuteFeatureInstant(UIStates state, bool playAudio = true, Action done = null)
        {
            if (_colorFeature != null)
                _colorFeature.ExecuteFeatureInstant(state);

            if (_spriteFeature != null)
                _spriteFeature.ExecuteFeatureInstant(state);

            if (_transformFeature != null)
                _transformFeature.ExecuteFeatureInstant(state);

            if (_audioFeature != null && playAudio)
                _audioFeature.ExecuteFeatureInstant(state);
        }

        #endregion
    }
}
