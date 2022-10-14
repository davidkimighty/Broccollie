using System.Collections;
using System.Collections.Generic;
using CollieMollie.Core;
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
        private void ChangeOthersToDefault(InteractableEventArgs args)
        {
            if (!args.IsValid()) return;

            foreach (UIButton button in _buttons)
            {
                if (button == args.Sender) continue;
                button.ChangeState(InteractionState.Default, false, false, false);
            }
        }
        #endregion

        #region Public Functions
        public void AddButton(UIButton button)
        {
            button.OnSelected += ChangeOthersToDefault;
            _buttons.Add(button);
        }

        public void RemoveButton(UIButton button)
        {
            button.OnSelected -= ChangeOthersToDefault;
            if (_buttons.Contains(button))
                _buttons.Remove(button);
        }
        #endregion
    }
}
