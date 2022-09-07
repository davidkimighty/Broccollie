using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CollieMollie.UI
{
    public class UIButtonGroup : MonoBehaviour
    {
        #region Variable Field
        [SerializeField] private List<UIButton> _buttons = null;
        [SerializeField] private bool _useRadioButtonGroup = false;
        #endregion

        private void OnEnable()
        {
            if (_useRadioButtonGroup)
            {
                foreach (UIButton button in _buttons)
                    button.OnSelected += RadioButtonGroup;
            }
        }

        private void OnDisable()
        {
            if (_useRadioButtonGroup)
            {
                foreach (UIButton button in _buttons)
                    button.OnSelected -= RadioButtonGroup;
            }
        }

        #region Button Group Functions
        public void RadioButtonGroup(UIEventArgs args)
        {
            if (!args.IsValid()) return;

            foreach (UIButton button in _buttons)
            {
                if (button == args.Sender) continue;
                button.ChangeStateQuietly(ButtonState.Default);
            }
        }
        #endregion
    }
}
