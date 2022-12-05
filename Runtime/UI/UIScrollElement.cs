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
    [SerializeField] private UISpriteFeature _spriteFeature = null;
    [SerializeField] private UITransformFeature _transformFeature = null;
    [SerializeField] private UIAudioFeature _audioFeature = null;

    private bool _isFocused = false;
    #endregion

    #region Public Functions
    public void Focus(string focusState, bool playAudio = true, bool fireEvent = true)
    {
        if (_isFocused) return;
        _isFocused = true;

        if (fireEvent)
            OnFocus?.Invoke();

        SetFeatures(focusState, playAudio);
    }

    public void Unfocus(string unfocusState, bool playAudio = true, bool fireEvent = true)
    {
        if (!_isFocused) return;
        _isFocused = false;

        if (fireEvent)
            OnUnfocus?.Invoke();

        SetFeatures(unfocusState, playAudio);
    }

    #endregion

    private void SetFeatures(string state, bool playAudio)
    {
        if (_colorFeature != null)
            _colorFeature.Execute(state);

        if (_spriteFeature != null)
            _spriteFeature.Execute(state);

        if (_transformFeature != null)
            _transformFeature.Execute(state);

        if (_audioFeature != null && playAudio)
            _audioFeature.Execute(state);
    }
}
