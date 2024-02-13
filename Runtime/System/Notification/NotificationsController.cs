using UnityEngine;

namespace Broccollie.System
{
    public class NotificationsController : MonoBehaviour
    {
        [SerializeField] private NotificationsEventChannel _eventChannel;
        [SerializeField] private FullScreenNotification _fullScreenNotification;
        [SerializeField] private PopupConfirmNotification _popupConfirmNotification;

        private void OnEnable()
        {
            if (_fullScreenNotification != null)
                _eventChannel.OnFullscreenMessage += ShowFullScreenNotification;
            if (_popupConfirmNotification != null)
                _eventChannel.OnPopupConfirmMessage += ShowPopupConfirmNotification;
        }

        private void OnDisable()
        {
            if (_fullScreenNotification != null)
                _eventChannel.OnFullscreenMessage -= ShowFullScreenNotification;
            if (_popupConfirmNotification != null)
                _eventChannel.OnPopupConfirmMessage -= ShowPopupConfirmNotification;
        }

        #region Subscribers
        private void ShowFullScreenNotification(bool state, string message) => _fullScreenNotification.ShowMessageAsync(state, message);

        private void ShowPopupConfirmNotification(bool state, string message) => _popupConfirmNotification.ShowMessageAsync(state, message);

        #endregion
    }
}
