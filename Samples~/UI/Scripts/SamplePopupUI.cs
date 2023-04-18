using Broccollie.UI;
using UnityEngine;

public class SamplePopupUI : MonoBehaviour
{
    [SerializeField] private PanelUI _panel = null;
    [SerializeField] private ButtonUI _triggerButton = null;
    [SerializeField] private ButtonUI _closeButton = null;

    private void Awake()
    {
        _triggerButton.OnSelect += (sender, args) => _panel.SetVisible(true);
        _closeButton.OnSelect += (sender, args) => _panel.SetVisible(false);

        if (_panel.IsActive)
            _panel.SetVisible(false);
    }
}
