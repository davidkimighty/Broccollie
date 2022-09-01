using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CollieMollie.UI
{
    public class UIButtonGroup : MonoBehaviour
    {
        #region Variable Field
        [Header("Button Group")]
        [SerializeField] private List<UIButton> buttons = null;
        [SerializeField] private bool useRadioButtonGroup = false;
        #endregion

        private void OnEnable()
        {
            if (useRadioButtonGroup)
            {
                foreach (UIButton button in buttons)
                    button.OnSelected += RadioButtonGroup;
            }
        }

        private void OnDisable()
        {
            if (useRadioButtonGroup)
            {
                foreach (UIButton button in buttons)
                    button.OnSelected -= RadioButtonGroup;
            }
        }

        #region Button Group Functions
        public void RadioButtonGroup(UIEventArgs args)
        {
            if (!args.IsValid()) return;

            foreach (UIButton button in buttons)
            {
                if (button == args.sender) continue;
                button.ChangeStateQuietly(ButtonState.Default);
            }
        }
        #endregion
    }
}
