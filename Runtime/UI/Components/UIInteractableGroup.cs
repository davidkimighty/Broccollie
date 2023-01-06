using System.Collections;
using System.Collections.Generic;
using CollieMollie.Core;
using UnityEngine;

namespace CollieMollie.UI
{
    public class UIInteractableGroup : MonoBehaviour
    {
        #region Variable Field
        [SerializeField] private BaseUI.State _defaultState = BaseUI.State.None;
        [SerializeField] private BaseUI.State _selectedState = BaseUI.State.None;
        [SerializeField] private List<InteractableUI> _interactables = new List<InteractableUI>();

        #endregion

        private void OnEnable()
        {
            foreach (InteractableUI interactable in _interactables)
                SubscribeEvent(interactable);
        }

        private void OnDisable()
        {
            foreach (InteractableUI interactable in _interactables)
                UnsubscribeEvent(interactable);
        }

        #region Subscribers
        private void ChangeOthersToDefault(UIEventArgs args)
        {
            if (!args.IsValid()) return;

            foreach (InteractableUI interactable in _interactables)
            {
                if (interactable == args.Sender) continue;
                interactable.ChangeState(_defaultState, false, false);
            }
        }

        #endregion

        #region Public Functions
        public void AddInteractable(InteractableUI interactable)
        {
            SubscribeEvent(interactable);
            _interactables.Add(interactable);
        }

        public void RemoveInteractable(InteractableUI interactable)
        {
            UnsubscribeEvent(interactable);
            if (_interactables.Contains(interactable))
                _interactables.Remove(interactable);
        }

        public void ChangeToDefault(InteractableUI exception = null)
        {
            foreach (InteractableUI interactable in _interactables)
            {
                if (interactable == exception) continue;
                interactable.ChangeState(_defaultState, false, false);
            }
        }

        #endregion

        #region Private Functions
        private void SubscribeEvent(InteractableUI interactable)
        {
            switch (_selectedState)
            {
                case BaseUI.State.Interactive:
                    interactable.OnInteractive += ChangeOthersToDefault;
                    break;

                case BaseUI.State.Show:
                    interactable.OnShow += ChangeOthersToDefault;
                    break;

                case BaseUI.State.Selected:
                    interactable.OnSelected += ChangeOthersToDefault;
                    break;
            }
        }

        private void UnsubscribeEvent(InteractableUI interactable)
        {
            switch (_selectedState)
            {
                case BaseUI.State.Interactive:
                    interactable.OnInteractive -= ChangeOthersToDefault;
                    break;

                case BaseUI.State.Show:
                    interactable.OnShow -= ChangeOthersToDefault;
                    break;

                case BaseUI.State.Selected:
                    interactable.OnSelected -= ChangeOthersToDefault;
                    break;
            }
        }

        #endregion
    }
}
