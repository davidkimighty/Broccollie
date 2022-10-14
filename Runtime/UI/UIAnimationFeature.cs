using System;
using System.Collections;
using System.Collections.Generic;
using CollieMollie.Helper;
using UnityEngine;

namespace CollieMollie.UI
{
    [RequireComponent(typeof(Animator))]
    public class UIAnimationFeature : MonoBehaviour
    {
        #region Variable Field
        [SerializeField] private bool _isEnabled = true;
        [SerializeField] private Animator _animator = null;
        #endregion

        #region Public Functions
        public void Change(ButtonState state)
        {
            if (!_isEnabled || _animator == null) return;

            switch (state)
            {
                case ButtonState.Default:
                    _animator.SetBool(ButtonState.Hovered.ToString(), false);
                    _animator.SetBool(ButtonState.Pressed.ToString(), false);
                    _animator.SetBool(ButtonState.Selected.ToString(), false);
                    break;

                case ButtonState.Hovered:
                    _animator.SetBool(ButtonState.Hovered.ToString(), true);
                    break;

                case ButtonState.Pressed:
                    _animator.SetBool(ButtonState.Pressed.ToString(), true);
                    break;

                case ButtonState.Selected:
                    _animator.SetBool(ButtonState.Hovered.ToString(), false);
                    _animator.SetBool(ButtonState.Pressed.ToString(), false);
                    _animator.SetBool(ButtonState.Selected.ToString(), true);
                    break;

                case ButtonState.Disabled:
                    _animator.SetBool(ButtonState.Hovered.ToString(), false);
                    _animator.SetBool(ButtonState.Pressed.ToString(), false);
                    _animator.SetBool(ButtonState.Selected.ToString(), false);
                    _animator.SetBool(ButtonState.Disabled.ToString(), true);
                    break;
            }
        }

        #endregion
    }
}
