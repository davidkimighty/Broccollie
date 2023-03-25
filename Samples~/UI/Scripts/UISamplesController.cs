using System.Collections;
using System.Collections.Generic;
using Broccollie.UI;
using UnityEngine;

public class UISamplesController : MonoBehaviour
{
    [SerializeField] private ButtonUI _buttonsButton = null;
    [SerializeField] private ButtonUI _panelsButton = null;
    [SerializeField] private PanelUI _buttonsPanel = null;
    [SerializeField] private PanelUI _panelsPanel = null;

    private void Awake()
    {
        _buttonsButton.OnSelect += (ui) => _buttonsPanel.SetActive(true);
        _panelsButton.OnSelect += (ui) => _panelsPanel.SetActive(true);

    }

    private void Start()
    {
        _buttonsButton.Select();
    }
}