using System.Collections;
using System.Collections.Generic;
using CollieMollie.UI;
using UnityEngine;

public class SampleUITestBoard : MonoBehaviour
{
    public UIButton ShowTriggerButton = null;
    public UIButton ShowButton = null;

    private void Start()
    {
        ShowTriggerButton.OnSelected += (eventArgs) => ShowButton.Show(4f);
    }
}
