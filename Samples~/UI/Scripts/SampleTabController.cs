using System.Collections;
using System.Collections.Generic;
using CollieMollie.UI;
using UnityEngine;

public class SampleTabController : MonoBehaviour
{
    [SerializeField] private UIButton _triggerButton = null;
    [SerializeField] private UIPanel _panel = null;

    private void Awake()
    {
        _triggerButton.OnSelected += (eventArgs) => _panel.ChangeState(BaseUI.State.Show);
        _triggerButton.OnDefault += (eventArgs) => _panel.ChangeState(BaseUI.State.Hide);
    }

}
