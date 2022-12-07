using System.Collections;
using System.Collections.Generic;
using CollieMollie.Core;
using UnityEngine;

namespace CollieMollie.UI
{
    public class UIInteractableGroup : MonoBehaviour
    {
        #region Variable Field
        [SerializeField] private List<InteractableUI> _interactables = null;

        #endregion

        private void OnEnable()
        {
            foreach (InteractableUI interactable in _interactables)
                interactable.OnSelected += ChangeOthersToDefault;
        }

        private void OnDisable()
        {
            foreach (InteractableUI interactable in _interactables)
                interactable.OnSelected -= ChangeOthersToDefault;
        }

        #region Subscribers
        private void ChangeOthersToDefault(UIEventArgs args)
        {
            if (!args.IsValid()) return;

            foreach (InteractableUI interactable in _interactables)
            {
                if (interactable == args.Sender) continue;
                interactable.ChangeState(UIState.Default, false, false);
            }
        }

        #endregion

        #region Public Functions
        public void AddInteractable(InteractableUI interactable)
        {
            interactable.OnSelected += ChangeOthersToDefault;
            _interactables.Add(interactable);
        }

        public void RemoveInteractable(InteractableUI interactable)
        {
            interactable.OnSelected -= ChangeOthersToDefault;
            if (_interactables.Contains(interactable))
                _interactables.Remove(interactable);
        }

        #endregion
    }
}
