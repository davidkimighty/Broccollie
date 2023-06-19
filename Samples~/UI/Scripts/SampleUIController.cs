using Broccollie.UI;
using TMPro;
using UnityEngine;

public class SampleUIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _title = null;
    [SerializeField] private ButtonUI _leftButton = null;
    [SerializeField] private ButtonUI _rightButton = null;
    [SerializeField] private ScrollUI _scroll = null;

    [SerializeField] private PanelUI _panelInteractive = null;
    [SerializeField] private ButtonUI _toggleInteractive = null;

    private int _scrollIndex = 0;
    private string[] _titles = { "Buttons", "Panels" };

    private void Start()
    {
        _leftButton.OnClick += (eventArgs, sender) => ChangeScrollPage(Mathf.Abs((_scrollIndex == 0 ?
            _scrollIndex - _scroll.GetPageCount() + 1 : _scrollIndex - 1)) % _scroll.GetPageCount());
        _rightButton.OnClick += (eventArgs, sender) => ChangeScrollPage(Mathf.Abs((_scrollIndex + 1)) % _scroll.GetPageCount());

        _scroll.SelectPageWithIndex(_scrollIndex);

        _toggleInteractive.OnClick += (eventArgs, sender) => _panelInteractive.SetVisible(true);
        _toggleInteractive.OnDefault += (eventArgs, sender) => _panelInteractive.SetVisible(false);

        _panelInteractive.SetVisible(false);
    }

    private void ChangeScrollPage(int index)
    {
        _scrollIndex = index;
        _scroll.SelectPageWithIndex(_scrollIndex);
        _title.text = _titles[index];
    }
}
