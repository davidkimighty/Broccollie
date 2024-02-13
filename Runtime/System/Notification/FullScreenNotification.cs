using System;
using Broccollie.UI;
using TMPro;
using UnityEngine;

namespace Broccollie.System
{
    public class FullScreenNotification : MonoBehaviour
    {
        [SerializeField] private PanelUI _fullScreenPanel;
        [SerializeField] private TMP_Text _fullScreenMessageText;

        #region Public Functions
        public async void ShowMessageAsync(bool state, string message)
        {
            if (_fullScreenMessageText != null)
                _fullScreenMessageText.text = message;
            await _fullScreenPanel.SetActiveAsync(state);
        }

        #endregion
    }

    public class FullScreenMessage : IDisposable
    {
        private NotificationsEventChannel _eventChannel;

        public FullScreenMessage(NotificationsEventChannel eventChannel, string message)
        {
            _eventChannel = eventChannel;
            _eventChannel.RequestFullscreenMessage(true, message);
        }

        public void Dispose()
        {
            if (_eventChannel == null) return;
            _eventChannel.RequestFullscreenMessage(false);
        }
    }
}
