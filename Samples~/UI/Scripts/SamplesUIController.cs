using System.Collections;
using System.Collections.Generic;
using Broccollie.UI;
using UnityEngine;

public class SamplesUIController : MonoBehaviour
{
    [Header("Boards")]
    [SerializeField] private ButtonUI _buttonsButton = null;
    [SerializeField] private ButtonUI _panelsButton = null;
    [SerializeField] private PanelUI _buttonsPanel = null;
    [SerializeField] private PanelUI _panelsPanel = null;

    [Header("Buttons")]
    [SerializeField] private ButtonUI _showButtonTrigger_0 = null;
    [SerializeField] private ButtonUI _showButton_0 = null;
    [SerializeField] private ButtonUI _showButtonTrigger_1 = null;
    [SerializeField] private ButtonUI _showButton_1 = null;
    [SerializeField] private ButtonUI _showButtonTrigger_2 = null;
    [SerializeField] private ButtonUI _showButton_2 = null;


    private void Awake()
    {
        _buttonsButton.OnSelect += (sender, args) => _buttonsPanel.SetActive(true);
        _panelsButton.OnSelect += (sender, args) => _panelsPanel.SetActive(true);

        _showButtonTrigger_0.OnSelect += (sender, args) => _showButton_0.SetActive(true);
        _showButtonTrigger_0.OnDefault += (sender, args) => _showButton_0.SetActive(false);
        _showButtonTrigger_1.OnSelect += (sender, args) => _showButton_1.SetActive(true);
        _showButtonTrigger_1.OnDefault += (sender, args) => _showButton_1.SetActive(false);
        _showButtonTrigger_2.OnSelect += (sender, args) => _showButton_2.SetActive(true);
        _showButtonTrigger_2.OnDefault += (sender, args) => _showButton_2.SetActive(false);
    }

    private void Start()
    {
        _buttonsButton.Select();
        _showButtonTrigger_0.Select();
        _showButtonTrigger_1.Select();
        _showButtonTrigger_2.Default();
    }
}