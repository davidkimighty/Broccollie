using System.Collections;
using System.Collections.Generic;
using CollieMollie.UI;
using UnityEngine;

public class SamplePopupController : MonoBehaviour
{
    [SerializeField] private UIButton _triggerButton = null;
    [SerializeField] private UIPanel _panel = null;
    [SerializeField] private UIButton _closeButton = null;

    private void Awake()
    {
        _triggerButton.OnSelected += (eventArgs) => _panel.ChangeState(BaseUI.State.Show);
        _closeButton.OnSelected += (eventArgs) => _panel.ChangeState(BaseUI.State.Hide);
    }
}
