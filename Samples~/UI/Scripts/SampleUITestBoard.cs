using System.Collections;
using System.Collections.Generic;
using CollieMollie.Core;
using CollieMollie.UI;
using UnityEngine;

public class SampleUITestBoard : MonoBehaviour
{
    #region Variable Field
    [Header("Button Popup")]
    [SerializeField] private ButtonUI _triggerButtonOne = null;
    [SerializeField] private ButtonUI _triggerButtonTwo = null;
    [SerializeField] private ButtonUI _triggerButtonThree = null;
    [SerializeField] private ButtonUI _popupButtonOne = null;
    [SerializeField] private ButtonUI _popupButtonTwo = null;
    [SerializeField] private ButtonUI _popupButtonThree = null;

    [Header("Tab Menu")]
    [SerializeField] private UIButton _firstTab = null;

    [Header("Position")]
    [SerializeField] private UIButton _clearButton = null;
    [SerializeField] private UIButton _resetButton = null;
    [SerializeField] private UIGroup[] _groups = null;

    #endregion

    private void Awake()
    {
        _triggerButtonOne.OnSelect += () => PopupButton(_popupButtonOne);
        _triggerButtonTwo.OnSelect += () => PopupButton(_popupButtonTwo);
        _triggerButtonThree.OnSelect += () => PopupButton(_popupButtonThree);

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
    private void PopupButton(ButtonUI targetButton)
    {
        targetButton.SetActive(!targetButton.gameObject.activeSelf);
    }
    #endregion
}
