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

        private Task _featureTasks = null;

        #endregion

        #region Public Functions
        public override void SetActive(bool state, bool playAudio = true, bool invokeEvent = true)
        {
            if (state)
            {
                _currentState = UIStates.Show;
                _isActive = true;
                if (!_panel.activeSelf)
                    _panel.SetActive(true);

                if (invokeEvent)
                    RaiseOnShow(this, EventArgs.Empty);

                _featureTasks = ExecuteFeaturesAsync(UIStates.Show, true, () =>
                {
                    if (_isInteractive)
                        Default(playAudio, invokeEvent);
                });
            }
            else
            {
                _currentState = UIStates.Hide;
                _isActive = _isInteractive = false;

                if (invokeEvent)
                    RaiseOnHide(this, EventArgs.Empty);

                _featureTasks = ExecuteFeaturesAsync(UIStates.Hide, true, () =>
                {
                    _panel.SetActive(false);
                });
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
                    RaiseOnInteractive(this, EventArgs.Empty);

                _featureTasks = ExecuteFeaturesAsync(UIStates.Interactive, true, () =>
                {
                    _isInteractive = true;
                    Default(playAudio, invokeEvent);
                });
            }
            else
            {
                _currentState = UIStates.NonInteractive;
                _isInteractive = false;
                if (!_panel.activeSelf)
                    _panel.SetActive(true);

                if (invokeEvent)
                    RaiseOnInteractive(this, EventArgs.Empty);

                _featureTasks = ExecuteFeaturesAsync(UIStates.NonInteractive);
            }
        }

        public void Default(bool playAudio = true, bool invokeEvent = true)
        {
            if (!_isInteractive) return;

            _currentState = UIStates.Default;
            _isHovered = _isPressed = _isSelected = false;
            if (invokeEvent)
                RaiseOnDefault(this, EventArgs.Empty);

            _featureTasks = ExecuteFeaturesAsync(UIStates.Default);
        }

        #endregion
    }
}
