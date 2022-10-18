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

    [SerializeField] private GameObject _testBoard = null;
    [SerializeField] private UIButton[] _buttons = null;

    private void Awake()
    {
        _buttons = _testBoard.GetComponentsInChildren<UIButton>();

        for (int i = 0; i < _buttons.Length; i++)
        {
            _buttons[i].OnSelected += (eventArgs) => PrintName(eventArgs, ONSELECT);
        }
    }

    private void PrintName(InteractableEventArgs args, string msg)
    {
        _console.text += $"{args.Sender.name} {msg}\n";
    }
}
