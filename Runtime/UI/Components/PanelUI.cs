using System;
using System.Collections.Generic;
using UnityEngine;

namespace Broccollie.UI
{
    [DefaultExecutionOrder(-120)]
    public class PanelUI : BaseUI
    {
        private static List<BaseUI> s_activePanels = new List<BaseUI>();

        [Header("Panel")]
        [SerializeField] private GameObject _panel = null;

        private void OnEnable()
        {
            s_activePanels.Add(this);
        }

        private void OnDisable()
        {
            s_activePanels.Remove(this);
        }

        #region Public Functions
        public override void ChangeState(string state, bool instant = false, bool playAudio = true, bool invokeEvent = true)
        {
            if (Enum.TryParse(state, out UIStates uiState))
            {
                switch (uiState)
                {
                    case UIStates.Default:
                        Default(playAudio, invokeEvent);
                        break;

                    case UIStates.Interactive:
                        Interactive(instant, playAudio, invokeEvent);
                        break;

                    case UIStates.NonInteractive:
                        NonInteractive(instant, playAudio, invokeEvent);
                        break;

                    case UIStates.Show:
                        Show(instant, playAudio, invokeEvent);
                        break;

                    case UIStates.Hide:
                        Hide(instant, playAudio, invokeEvent);
                        break;
                }
            }
            else
            {

            }
        }

        public override void SetActive(bool state)
        {
            _panel.SetActive(state);
        }

        #endregion

        private void Default(bool playAudio, bool invokeEvent)
        {
            if (!_isInteractive) return;

            SetCurrentState(UIStates.Default, out string state);
            _isHovered = _isPressed = _isClicked = false;

            if (invokeEvent)
                RaiseOnDefault(this, null);

            _featureTasks = ExecuteFeaturesAsync(state, playAudio);
        }

        private void Interactive(bool instant, bool playAudio, bool invokeEvent)
        {
            SetCurrentState(UIStates.Interactive, out string state);
            SetActive(true);

            if (invokeEvent)
                RaiseOnInteractive(this, null);

            if (instant) { }
            else
            {
                _featureTasks = ExecuteFeaturesAsync(state, playAudio, () =>
                {
                    _isInteractive = true;
                    Default(playAudio, invokeEvent);
                });
            }
        }

        private void NonInteractive(bool instant, bool playAudio, bool invokeEvent)
        {
            SetCurrentState(UIStates.NonInteractive, out string state);
            _isInteractive = false;
            SetActive(true);

            if (invokeEvent)
                RaiseOnInteractive(this, null);

            if (instant) { }
            else
                _featureTasks = ExecuteFeaturesAsync(state, playAudio);
        }

        private void Show(bool instant, bool playAudio, bool invokeEvent)
        {
            SetCurrentState(UIStates.Show, out string state);
            _isActive = true;
            SetActive(true);

            if (invokeEvent)
                RaiseOnShow(this, null);

            if (instant)
                ExecuteFeatureInstant(state, playAudio);
            else
            {
                _featureTasks = ExecuteFeaturesAsync(state, playAudio, () =>
                {
                    Default(playAudio, invokeEvent);
                });
            }
        }

        private void Hide(bool instant, bool playAudio, bool invokeEvent)
        {
            SetCurrentState(UIStates.Hide, out string state);
            _isActive = false;

            if (invokeEvent)
                RaiseOnHide(this, null);

            if (instant)
            {
                SetActive(false);
                ExecuteFeatureInstant(state, playAudio);
            }
            else
            {
                _featureTasks = ExecuteFeaturesAsync(state, playAudio, () =>
                {
                    SetActive(false);
                });
            }
        }
    }

    public class PanelUIEventArgs : EventArgs
    {

    }
}
