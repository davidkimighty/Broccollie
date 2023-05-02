using System.Collections;
using System.Collections.Generic;
using Broccollie.UI;
using UnityEngine;

public class SamplesUIController : MonoBehaviour
{
    [SerializeField] private ButtonUI _buttonsSideButton = null;
    [SerializeField] private ButtonUI _buttonsCloseButton = null;
    [SerializeField] private ButtonUI _panelsSideButton = null;
    [SerializeField] private ButtonUI _panelsCloseButton = null;

    [SerializeField] private PanelUI _buttonsPanel = null;
    [SerializeField] private PanelUI _panelsPanel = null;

    [SerializeField] private ButtonUI _checkbox1 = null;
    [SerializeField] private ButtonUI _checkbox2 = null;
    [SerializeField] private ButtonUI _checkbox3 = null;

    [SerializeField] private ButtonUI _showButton1 = null;
    [SerializeField] private ButtonUI _showButton2 = null;
    [SerializeField] private ButtonUI _showButton3 = null;

    private void Awake()
    {
        _buttonsSideButton.OnSelect += (sender, args) => _buttonsPanel.SetVisible(true);
        _panelsSideButton.OnSelect += (sender, args) => _panelsPanel.SetVisible(true);
        _buttonsCloseButton.OnSelect += (sender, args) => _buttonsPanel.SetVisible(false);
        _panelsCloseButton.OnSelect += (sender, args) => _panelsPanel.SetVisible(false);

        _checkbox1.OnSelect += (sender, args) => _showButton1.SetVisible(true);
        _checkbox1.OnDefault += (sender, args) => _showButton1.SetVisible(false);
        _checkbox2.OnSelect += (sender, args) => _showButton2.SetVisible(true);
        _checkbox2.OnDefault += (sender, args) => _showButton2.SetVisible(false);
        _checkbox3.OnSelect += (sender, args) => _showButton3.SetVisible(true);
        _checkbox3.OnDefault += (sender, args) => _showButton3.SetVisible(false);

        _buttonsSideButton.Select();
    }

    private void Start()
    {
        _checkbox1.Select();
        _checkbox2.Select();
        _checkbox3.Default();
    }
}