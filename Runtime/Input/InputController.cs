using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CollieMollie.Input
{
    public class InputController : MonoBehaviour
    {
        #region Variable Field
        private event Action<Vector2> OnMove = null;
        private event Action<bool> OnFire = null;

        [SerializeField] private PlayerInput _playerInput = null;
        [SerializeField] private DefaultInputActions _defaultInputActions = null;


        #endregion

        #region

        #endregion

    }
}
