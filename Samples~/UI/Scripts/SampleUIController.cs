using Broccollie.UI;
using UnityEngine;

public class SampleUIController : MonoBehaviour
{
    [SerializeField] private ButtonUI _leftButton = null;
    [SerializeField] private ButtonUI _rightButton = null;
    [SerializeField] private ScrollUI _scroll = null;

    private int _scrollIndex = 0;

    private void Start()
    {
        _leftButton.OnClick += (eventArgs, sender) => ChangeScrollPage(Mathf.Abs((_scrollIndex == 0 ?
            _scrollIndex - _scroll.GetPageCount() + 1 : _scrollIndex - 1)) % _scroll.GetPageCount());
        _rightButton.OnClick += (eventArgs, sender) => ChangeScrollPage(Mathf.Abs((_scrollIndex + 1)) % _scroll.GetPageCount());

        _scroll.SelectPageWithIndex(_scrollIndex);
    }

    private void ChangeScrollPage(int index)
    {
        _scrollIndex = index;
        _scroll.SelectPageWithIndex(_scrollIndex);
    }
}
