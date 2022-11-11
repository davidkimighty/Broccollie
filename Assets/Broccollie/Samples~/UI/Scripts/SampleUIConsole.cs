using System.Collections;
using System.Collections.Generic;
using CollieMollie.Core;
using CollieMollie.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class SampleUIConsole : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _console = null;

    [SerializeField] private GameObject _testBoard = null;

    private UIButton[] _buttons = null;
    private UIPanel[] _panels = null;

    private void Awake()
    {
        _buttons = _testBoard.GetComponentsInChildren<UIButton>();
        _panels = _testBoard.GetComponentsInChildren<UIPanel>();

        for (int i = 0; i < _buttons.Length; i++)
        {
            _buttons[i].OnDefault += (eventArgs) => PrintName(eventArgs, "OnDefault");
            _buttons[i].OnHovered += (eventArgs) => PrintName(eventArgs, "OnHovered");
            _buttons[i].OnPressed += (eventArgs) => PrintName(eventArgs, "OnPressed");
            _buttons[i].OnSelected += (eventArgs) => PrintName(eventArgs, "OnSelect");
            _buttons[i].OnBeginDrag += (eventArgs) => PrintName(eventArgs, "Begin Drag");
        }

        for (int i = 0; i < _panels.Length; i++)
        {
            _panels[i].OnShow += (eventArgs) => PrintName(eventArgs, "OnShow");
        }
    }

    private void PrintName(InteractableEventArgs args, string msg)
    {
        _console.text += $"{args.Sender.name} {msg}\n";
    }
}
