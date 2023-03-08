using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class SampleInputManager : MonoBehaviour
{
    [SerializeField] private PlayerInput _playerInput = null;
    [SerializeField] private DefaultInputActions _defaultInputActions = null;

    [SerializeField] private TextMeshProUGUI _inputTypeText = null;
    [SerializeField] private TextMeshProUGUI _moveText = null;

    private void Awake()
    {
        _defaultInputActions = new DefaultInputActions();
    }

    private void Start()
    {
        _inputTypeText.text = _playerInput.currentControlScheme.ToString();
    }

    private void OnEnable()
    {
        _defaultInputActions.Player.Enable();
        _defaultInputActions.Player.Move.performed += PrintMove;

        _playerInput.onControlsChanged += PrintInputType;
    }

    private void OnDisable()
    {
        _defaultInputActions.Player.Disable();
        _defaultInputActions.Player.Move.performed -= PrintMove;

        _playerInput.onControlsChanged -= PrintInputType;
    }

    private void PrintInputType(PlayerInput input)
    {
        _inputTypeText.text = input.currentControlScheme.ToString();
    }

    private void PrintMove(InputAction.CallbackContext context)
    {
        _moveText.text = context.ReadValue<Vector2>().ToString();
    }
}
