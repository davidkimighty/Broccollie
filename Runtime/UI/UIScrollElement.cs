using System;
using System.Collections;
using System.Collections.Generic;
using CollieMollie.Core;
using CollieMollie.UI;
using UnityEngine;

public class UIScrollElement : MonoBehaviour
{
    #region Variable Field
    public event Action OnFocus = null;
    public event Action OnUnfocus = null;

    [SerializeField] private UIColorFeature _colorFeature = null;
    [SerializeField] private UIAudioFeature _audioFeature = null;
    [SerializeField] private UISpriteFeature _spriteFeature = null;
    [SerializeField] private UIScaleFeature _scaleFeature = null;

    private bool _isFocused = false;
    #endregion

    #region Public Functions
    public void Focus(bool fireEvent = true)
    {
        if (_isFocused) return;
        _isFocused = true;

        if (fireEvent)
            OnFocus?.Invoke();

        if (_scaleFeature != null)
            _scaleFeature.Change(InteractionState.Selected);
    }

    public void Unfocus(bool fireEvent = true)
    {
        if (!_isFocused) return;
        _isFocused = false;

        if (fireEvent)
            OnUnfocus?.Invoke();

        if (_scaleFeature != null)
            _scaleFeature.Change(InteractionState.Default);
    }

    #endregion
}
