using System;
using System.Collections;
using System.Collections.Generic;
using CollieMollie.Core;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CollieMollie.UI
{
    public class UISlot : MonoBehaviour, IDropHandler
    {
        #region Variable Field
        public event Action<InteractableEventArgs> OnSlotEnter = null;
        public event Action<InteractableEventArgs> OnSlotExit = null;

        private BasePointerInteractable _insertedInteractable = null;
        #endregion

        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag != null)
            {
                BasePointerInteractable interactable = eventData.pointerDrag.GetComponent<BasePointerInteractable>();
                if (interactable == null) return;

                OnSlotEnter?.Invoke(new InteractableEventArgs(_insertedInteractable));

                _insertedInteractable = interactable;
                interactable.transform.position = transform.position;
            }
        }
    }
}
