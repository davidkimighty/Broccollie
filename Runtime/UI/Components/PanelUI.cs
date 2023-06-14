using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Broccollie.UI
{
    [DefaultExecutionOrder(-120)]
    public class PanelUI : BaseUI, IDefaultUI
    {
        #region Variable Field
        private static List<BaseUI> s_activePanels = new List<BaseUI>();

        [Header("Panel")]
        [SerializeField] private GameObject _panel = null;

        #endregion

        private void OnEnable()
        {
            s_activePanels.Add(this);
        }

        private void OnDisable()
        {
            s_activePanels.Remove(this);
        }

        #region Public Functions
        public override void SetVisible(bool state, bool playAudio = true, bool invokeEvent = true)
        {
            if (state)
            {
                _currentState = UIStates.Show;
                _isActive = true;

                if (!_panel.activeSelf)
                    _panel.SetActive(true);

                if (invokeEvent)
                    RaiseOnShow(this, new PanelUIEventArgs());

                _featureTasks = ExecuteFeaturesAsync(UIStates.Show, false, () =>
                {
                    Default(playAudio, invokeEvent);
                });
            }
            else
            {
                _currentState = UIStates.Hide;
                _isActive = false;

                if (invokeEvent)
                    RaiseOnHide(this, new PanelUIEventArgs());

                _featureTasks = ExecuteFeaturesAsync(UIStates.Hide, false, () =>
                {
                    _panel.SetActive(false);
                });
            }
        }

        public override void SetVisibleInstant(bool state, bool playAudio = true, bool invokeEvent = true)
        {
            if (state)
            {
                _currentState = UIStates.Default;
                _isActive = true;

                if (!_panel.activeSelf)
                    _panel.SetActive(true);

                if (invokeEvent)
                    RaiseOnShow(this, new PanelUIEventArgs());

                ExecuteFeatureInstant(UIStates.Default, playAudio);
            }
            else
            {
                _currentState = UIStates.Hide;
                _isActive = false;

                if (_panel.activeSelf)
                    _panel.SetActive(false);

                if (invokeEvent)
                    RaiseOnHide(this, new PanelUIEventArgs());

                ExecuteFeatureInstant(UIStates.Hide, playAudio);
            }
        }

        public override void SetInteractive(bool state, bool playAudio = true, bool invokeEvent = true)
        {
            if (state)
            {
                _currentState = UIStates.Interactive;

                if (!_panel.activeSelf)
                    _panel.SetActive(true);

                if (invokeEvent)
                    RaiseOnInteractive(this, new PanelUIEventArgs());

                _featureTasks = ExecuteFeaturesAsync(UIStates.Interactive, false, () =>
                {
                    Default(playAudio, invokeEvent);
                    _isInteractive = true;
                });
            }
            else
            {
                _currentState = UIStates.NonInteractive;
                _isInteractive = false;

                if (!_panel.activeSelf)
                    _panel.SetActive(true);

                if (invokeEvent)
                    RaiseOnInteractive(this, new PanelUIEventArgs());

                _featureTasks = ExecuteFeaturesAsync(UIStates.NonInteractive, false);
            }
        }

        public void Default(bool playAudio = true, bool invokeEvent = true)
        {
            if (!_isInteractive) return;

            _currentState = UIStates.Default;
            _isHovered = _isPressed = _isClicked = false;

            if (invokeEvent)
                RaiseOnDefault(this, new PanelUIEventArgs());

            _featureTasks = ExecuteFeaturesAsync(UIStates.Default, false);
        }

        #endregion
    }

    public class PanelUIEventArgs : EventArgs
    {

    }
}
