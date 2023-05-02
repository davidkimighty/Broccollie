using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Broccollie.UI
{
    [DefaultExecutionOrder(-120)]
    public class PanelUI : BaselineUI, IDefaultUI
    {
        #region Variable Field
        [Header("Panel")]
        [SerializeField] private GameObject _panel = null;

        #endregion

        #region Public Functions
        public override void SetVisible(bool state, bool playAudio = true, bool invokeEvent = true)
        {
            CancelFeatureTask();

            if (state)
            {
                _currentState = UIStates.Show;
                _isActive = true;

                if (!_panel.activeSelf)
                    _panel.SetActive(true);

                if (invokeEvent)
                    RaiseOnShow(this, new PanelUIEventArgs());

                _featureTasks = ExecuteFeaturesAsync(UIStates.Show, _cts.Token, false, () =>
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

                _featureTasks = ExecuteFeaturesAsync(UIStates.Hide, _cts.Token, false, () =>
                {
                    _panel.SetActive(false);
                });
            }
        }

        public override void SetVisibleInstant(bool state, bool playAudio = true, bool invokeEvent = true)
        {
            CancelFeatureTask();

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
            CancelFeatureTask();

            if (state)
            {
                _currentState = UIStates.Interactive;

                if (!_panel.activeSelf)
                    _panel.SetActive(true);

                if (invokeEvent)
                    RaiseOnInteractive(this, new PanelUIEventArgs());

                _featureTasks = ExecuteFeaturesAsync(UIStates.Interactive, _cts.Token, false, () =>
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

                _featureTasks = ExecuteFeaturesAsync(UIStates.NonInteractive, _cts.Token, false);
            }
        }

        public void Default(bool playAudio = true, bool invokeEvent = true)
        {
            if (!_isInteractive) return;

            CancelFeatureTask();

            _currentState = UIStates.Default;
            _isHovered = _isPressed = _isSelected = false;

            if (invokeEvent)
                RaiseOnDefault(this, new PanelUIEventArgs());

            _featureTasks = ExecuteFeaturesAsync(UIStates.Default, _cts.Token, false);
        }

        #endregion
    }

    public class PanelUIEventArgs : EventArgs
    {

    }
}
