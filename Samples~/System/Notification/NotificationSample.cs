using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Broccollie.System.Sample
{
    public class NotificationSample : MonoBehaviour
    {
        [SerializeField] private NotificationsEventChannel _eventChannel = null;

        [SerializeField] private TMP_Text _progressText = null;

        private int _count = 0;

        private async void Start()
        {
            await FullScreenAsync();
            //PopupConfirm();
        }

        private async Task FullScreenAsync()
        {
            using (new FullScreenMessage(_eventChannel, "Full Screen Message..."))
            {
                await ProgressAsync();
            }
        }

        private void PopupConfirm()
        {
            _eventChannel.RequestPopupConfirmMessage(true, "Popup Screen Message...");
        }

        private async Task ProgressAsync()
        {
            while (_count < 10)
            {
                _count++;
                _progressText.text = _count.ToString();
                await Task.Delay(1000);
            }
            _progressText.text = "0";
        }
    }
}
