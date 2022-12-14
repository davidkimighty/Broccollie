using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
    [SerializeField] private UIAnimationFeature _animationFeature = null;
    [SerializeField] private UIAudioFeature _audioFeature = null;

    private bool _isFocused = false;
    private Task _behaviorTask = null;
    #endregion

    #region Public Functions
    public void Focus(string focusState, bool playAudio = true, bool fireEvent = true)
    {
        if (_isFocused) return;
        _isFocused = true;

        if (fireEvent)
            OnFocus?.Invoke();

        _behaviorTask = ExecuteFeaturesAsync(focusState, playAudio);
    }

    public void Unfocus(string unfocusState, bool playAudio = true, bool fireEvent = true)
    {
        if (!_isFocused) return;
        _isFocused = false;

        if (fireEvent)
            OnUnfocus?.Invoke();

        _behaviorTask = ExecuteFeaturesAsync(unfocusState, playAudio);
    }

    #endregion

    private async Task ExecuteFeaturesAsync(string state, bool playAudio, Action done = null)
    {
        List<Task> featureTasks = new List<Task>();
        if (_colorFeature != null)
            featureTasks.Add(_colorFeature.ExecuteAsync(state));

        if (_spriteFeature != null)
            featureTasks.Add(_spriteFeature.ExecuteAsync(state));

        if (_transformFeature != null)
            featureTasks.Add(_transformFeature.ExecuteAsync(state));

        if (_animationFeature != null)
            featureTasks.Add(_animationFeature.ExecuteAsync(state));

        if (_audioFeature != null && playAudio)
            featureTasks.Add(_audioFeature.ExecuteAsync(state));

        await Task.WhenAll(featureTasks);
        done?.Invoke();
    }
}
