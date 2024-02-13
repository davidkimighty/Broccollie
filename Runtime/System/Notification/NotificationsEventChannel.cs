using System;
using UnityEngine;

namespace Broccollie.System
{
    [CreateAssetMenu(fileName = "EventChannel_Notifications", menuName = "Broccollie/EventChannels/Notifications")]
    public class NotificationsEventChannel : ScriptableObject
    {
        public event Action<bool, string> OnFullscreenMessage;
        public event Action<bool, string> OnPopupConfirmMessage; 

        #region Publishers
        public void RequestFullscreenMessage(bool state, string message = null) => OnFullscreenMessage?.Invoke(state, message);

        public void RequestPopupConfirmMessage(bool state, string message = null) => OnPopupConfirmMessage?.Invoke(state, message);

        #endregion
    }

}
