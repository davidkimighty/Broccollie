using System.Collections;
using System.Collections.Generic;
using CollieMollie.Core;
using CollieMollie.UI;
using TMPro;
using UnityEngine;

public class SampleUIConsole : MonoBehaviour
{
    private const string ONSELECT = "OnSelect";

    [SerializeField] private TextMeshProUGUI _console = null;

    [SerializeField] private UIButton[] _buttons = null;
    [SerializeField] private UIButton[] _radioButtons = null;
    [SerializeField] private UIButton[] _checkboxButtons = null;

    private void Awake()
    {
        for (int i = 0; i < _buttons.Length; i++)
        {
            _buttons[i].OnSelected += (eventArgs) => PrintName(eventArgs, ONSELECT);
        }

        for (int i = 0; i < _radioButtons.Length; i++)
        {
            _radioButtons[i].OnSelected += (eventArgs) => PrintName(eventArgs, ONSELECT);
        }

        for (int i = 0; i < _checkboxButtons.Length; i++)
        {
            _checkboxButtons[i].OnSelected += (eventArgs) => PrintName(eventArgs, ONSELECT);
        }
    }

    private void PrintName(InteractableEventArgs args, string msg)
    {
        _console.text += $"{args.Sender.name} {msg}\n";
    }
}
