using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Broccollie.Core;
using Broccollie.UI;
using UnityEngine;
using UnityEngine.UI;

public class IpodController : MonoBehaviour
{
    private const string s_key = "pre";

    [SerializeField] private ScrollUI _scroll = null;
    [SerializeField] private HorizontalLayoutGroup _horizontal = null;

    [SerializeField] private float _zoomOutScale = 0.8f;
    [SerializeField] private float _zoomDuration = 0.5f;
    [SerializeField] private AnimationCurve _zoomCurve = null;

    [SerializeField] private ButtonUI _backButton = null;
    [SerializeField] private ButtonUI _forwardButton = null;
    [SerializeField] private Image _edgeHighlight = null;

    private int _index = -1;
    private CancellationTokenSource _cts = new CancellationTokenSource();

    private void Awake()
    {
        _scroll.OnBeginScroll += BeginScrollAsync;
        _scroll.OnEndScroll += EndScrollAsync;

        _scroll.OnFocusElement += Focus;
        _scroll.OnUnfocusElement += Unfocus;

        _backButton.OnSelect += (args) => ChangeMusicByDir(false);
        _forwardButton.OnSelect += (args) => ChangeMusicByDir(true);
    }

    private void Start()
    {
        _ = _forwardButton.SelectAsync();
    }

    #region Subscribers
    private async void BeginScrollAsync()
    {
        await ZoomAsync(Color.white, _zoomOutScale);
    }

    private async void EndScrollAsync()
    {
        Color color = Color.white;
        color.a = 0;
        await ZoomAsync(color, 1);
    }

    private void Focus(BaseUIElement baseUI, int index)
    {
        _index = index;
        if (baseUI.CurrentState == UIStates.Hover) return;

        if (baseUI.TryGetComponent<IHover>(out var hover))
            hover.HoverAsync();
    }

    private void Unfocus(BaseUIElement baseUI, int index)
    {
        if (baseUI.CurrentState == UIStates.Default) return;

        if (baseUI.TryGetComponent<IDefault>(out var deFault))
            deFault.DefaultAsync();
    }

    private void ChangeMusicByDir(bool forward)
    {
        _index = forward ? _index >= _scroll.GetPageCount(s_key) - 1 ? 0 : _index + 1 :
            _index <= 0 ? _scroll.GetPageCount(s_key) - 1 : _index - 1;
        _scroll.SelectPageWithIndex(_index);
    }

    #endregion

    private async Task ZoomAsync(Color targetColor, float targetScale)
    {
        try
        {
            _cts.Cancel();
            _cts = new CancellationTokenSource();

            List<Task> zoomTasks = new List<Task>
            {
                _edgeHighlight.LerpColorAsync(targetColor, _zoomDuration, _cts.Token, _zoomCurve)
            };
            foreach (BaseUIElement ui in _scroll.ScrollElements)
                zoomTasks.Add(ui.transform.LerpScaleAsync(Vector3.one * targetScale, _zoomDuration, _cts.Token, _zoomCurve));

            await Task.WhenAll(zoomTasks);
        }
        catch (OperationCanceledException) { }
    }
}
