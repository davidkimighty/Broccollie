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
        [Header("Button")]
        [SerializeField] private GameObject _panel = null;

        private Task _featureTasks = null;
        private CancellationTokenSource _cts = new CancellationTokenSource();

        #endregion

        #region Public Functions
        public override void SetVisible(bool state, bool playAudio = true, bool invokeEvent = true)
        {
            _cts.Cancel();
            _cts = new CancellationTokenSource();

            if (state)
            {
                _currentState = UIStates.Show;
                _isActive = true;
                if (!_panel.activeSelf)
                    _panel.SetActive(true);

                if (invokeEvent)
                    RaiseOnShow(this, EventArgs.Empty);

                _featureTasks = ExecuteFeaturesAsync(UIStates.Show, playAudio, _cts, () =>
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
                    RaiseOnHide(this, EventArgs.Empty);

                _featureTasks = ExecuteFeaturesAsync(UIStates.Hide, playAudio, _cts, () =>
                {
                    _panel.SetActive(false);
                });
            }
        }

        public override void SetVisibleInstant(bool state, bool playAudio = true, bool invokeEvent = true)
        {
            _cts.Cancel();
            _cts = new CancellationTokenSource();

            if (state)
            {
                _currentState = UIStates.Default;
                _isActive = true;
                _panel.SetActive(true);

                if (invokeEvent)
                    RaiseOnShow(this, new ButtonUIEventArgs());

                ExecuteFeatureInstant(UIStates.Default, playAudio);
            }
            else
            {
                _currentState = UIStates.Hide;
                _isActive = _isInteractive = false;
                _panel.SetActive(false);

                if (invokeEvent)
                    RaiseOnHide(this, new ButtonUIEventArgs());

                ExecuteFeatureInstant(UIStates.Hide, playAudio);
            }
        }

        public override void SetInteractive(bool state, bool playAudio = true, bool invokeEvent = true)
        {
            _cts.Cancel();
            _cts = new CancellationTokenSource();

            if (state)
            {
                _currentState = UIStates.Interactive;
                if (!_panel.activeSelf)
                    _panel.SetActive(true);

                if (invokeEvent)
                    RaiseOnInteractive(this, EventArgs.Empty);

                _featureTasks = ExecuteFeaturesAsync(UIStates.Interactive, playAudio, _cts, () =>
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

                _featureTasks = ExecuteFeaturesAsync(UIStates.NonInteractive, playAudio, _cts);
            }
        }

        public void Default(bool playAudio = true, bool invokeEvent = true)
        {
            if (!_isInteractive) return;

            _cts.Cancel();
            _cts = new CancellationTokenSource();

            _currentState = UIStates.Default;
            _isHovered = _isPressed = _isSelected = false;
            if (invokeEvent)
                RaiseOnDefault(this, EventArgs.Empty);

            _featureTasks = ExecuteFeaturesAsync(UIStates.Default, playAudio, _cts);
        }

        #endregion
    }
}
