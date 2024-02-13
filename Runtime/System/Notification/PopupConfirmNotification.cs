using Broccollie.UI;
using TMPro;
using UnityEngine;

namespace Broccollie.System
{
    public class PopupConfirmNotification : MonoBehaviour
    {
        [SerializeField] private PanelUI _popupConfirmPanel;
        [SerializeField] private TMP_Text _popupConfirmMessageText;
        [SerializeField] private ButtonUI _popupConfirmButton;

        private void Awake()
        {
            _popupConfirmButton.OnSelect += (eventArgs) => PopupConfirmedAsync();
        }

        #region Public Functions
        public async void ShowMessageAsync(bool state, string message)
        {
            if (_popupConfirmMessageText != null)
                _popupConfirmMessageText.text = message;
            await _popupConfirmPanel.SetActiveAsync(state);
        }

        public async void PopupConfirmedAsync()
        {
            if (_popupConfirmMessageText != null)
                _popupConfirmMessageText.text = string.Empty;
            await _popupConfirmPanel.SetActiveAsync(false);
        }

        #endregion
    }
}
