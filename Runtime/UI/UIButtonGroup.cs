using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CollieMollie.UI
{
    public class UIButtonGroup : MonoBehaviour
    {
        #region Variable Field
        [SerializeField] private List<UIButton> _buttons = null;
        #endregion

        private void OnEnable()
        {
            foreach (UIButton button in _buttons)
                button.OnSelected += ChangeOthersToDefault;
        }

        private void OnDisable()
        {
            foreach (UIButton button in _buttons)
                button.OnSelected -= ChangeOthersToDefault;
        }

        #region Subscribers
        private void ChangeOthersToDefault(UIEventArgs args)
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
