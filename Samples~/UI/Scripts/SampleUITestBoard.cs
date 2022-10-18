using System.Collections;
using System.Collections.Generic;
using CollieMollie.Core;
using CollieMollie.UI;
using UnityEngine;

public class SampleUITestBoard : MonoBehaviour
{
    #region Variable Field
    [Header("Button Popup")]
    [SerializeField] private UIButton _triggerButtonOne = null;
    [SerializeField] private UIButton _triggerButtonTwo = null;
    [SerializeField] private UIButton _triggerButtonThree = null;
    [SerializeField] private UIButton _popupButtonOne = null;
    [SerializeField] private UIButton _popupButtonTwo = null;
    [SerializeField] private UIButton _popupButtonThree = null;

    [Header("Tab Menu")]
    [SerializeField] private UIButton _firstTab = null;
    #endregion

    private void Awake()
    {
        _triggerButtonOne.OnSelected += (eventArgs) => _popupButtonOne.SetVisible(!_popupButtonOne.IsVisible);
        _triggerButtonTwo.OnSelected += (eventArgs) => _popupButtonTwo.SetVisible(!_popupButtonTwo.IsVisible);
        _triggerButtonThree.OnSelected += (eventArgs) => _popupButtonThree.SetVisible(!_popupButtonThree.IsVisible);

    }

    private void Start()
    {
        _firstTab.ChangeState(InteractionState.Selected, true, false);
    }
}
