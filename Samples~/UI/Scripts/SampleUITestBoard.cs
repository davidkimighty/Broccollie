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

    [Header("Position")]
    [SerializeField] private UIButton _clearButton = null;
    [SerializeField] private UIButton _resetButton = null;
    [SerializeField] private UIGroup[] _groups = null;

    #endregion

    private void Awake()
    {
        _triggerButtonOne.OnSelected += (eventArgs) => PopupButton(_popupButtonOne);
        _triggerButtonTwo.OnSelected += (eventArgs) => PopupButton(_popupButtonTwo);
        _triggerButtonThree.OnSelected += (eventArgs) => PopupButton(_popupButtonThree);

        for (int i = 0; i < _groups.Length; i++)
        {
            int index = i;
            _clearButton.OnSelected += (eventArgs) => _groups[index].ChangeState(BaseUI.State.Hide);
            _resetButton.OnSelected += (eventArgs) => _groups[index].ChangeState(BaseUI.State.Show);
        }
    }

    private void Start()
    {
        _firstTab.ChangeState(BaseUI.State.Selected, false, false);
    }

    #region Subscribers
    private void PopupButton(UIButton targetButton)
    {
        if (targetButton.IsVisible)
            targetButton.ChangeState(BaseUI.State.Hide);
        else
            targetButton.ChangeState(BaseUI.State.Show);
    }
    #endregion
}
