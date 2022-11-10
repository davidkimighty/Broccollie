using System.Collections;
using System.Collections.Generic;
using CollieMollie.Core;
using CollieMollie.UI;
using TMPro;
using UnityEngine;

public class SampleUIConsole : MonoBehaviour
{
    private const string ONSELECT = "OnSelect";
    private const string ONSHOW = "OnShow";

    [SerializeField] private TextMeshProUGUI _console = null;

    [SerializeField] private GameObject _testBoard = null;
    [SerializeField] private UIButton[] _buttons = null;
    [SerializeField] private UIPanel[] _panels = null;

    private void Awake()
    {
        _buttons = _testBoard.GetComponentsInChildren<UIButton>();
        _panels = _testBoard.GetComponentsInChildren<UIPanel>();

        for (int i = 0; i < _buttons.Length; i++)
        {
            _buttons[i].OnSelected += (eventArgs) => PrintName(eventArgs, ONSELECT);
            _buttons[i].OnBeginDrag += (eventArgs) => PrintName(eventArgs, "Begin Drag");
        }

        for (int i = 0; i < _panels.Length; i++)
        {
            _panels[i].OnShow += (eventArgs) => PrintName(eventArgs, ONSHOW);
        }
    }

    private void PrintName(InteractableEventArgs args, string msg)
    {
        _console.text += $"{args.Sender.name} {msg}\n";
    }
}
