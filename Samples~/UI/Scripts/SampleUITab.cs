using System;
using System.Collections;
using System.Collections.Generic;
using CollieMollie.Core;
using CollieMollie.UI;
using UnityEngine;

public class SampleUITab : BaseUIPanel
{
    [SerializeField] private UIButton _triggerButton = null;
    [SerializeField] private UIAnimationFeature _animationFeature = null;


    private void Awake()
    {
        if (_triggerButton != null)
        {
            _triggerButton.OnSelected += (eventArgs) => Show(3f);
        }
    }

    public void Show(float duration, bool playAudio = true)
    {
        SetPanelVisible(true);
        StartCoroutine(ShowButton(duration, playAudio, () =>
        {

        }));
    }

    private IEnumerator ShowButton(float duration, bool playAudio = true, Action done = null)
    {
        _animationFeature.Change(InteractionState.Show);

        yield return new WaitForSeconds(duration);

        done?.Invoke();
    }
}
