using System;
using System.Collections;
using System.Collections.Generic;
using CollieMollie.Core;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CollieMollie.UI
{
    [DefaultExecutionOrder(-100)]
    public class UISlot : BaseUI, IDropHandler
    {
        #region Variable Field
        public event Action<UIEventArgs> OnSlotEnter = null;
        public event Action<UIEventArgs> OnSlotExit = null;

        private InteractableUI _insertedInteractable = null;

        #endregion

        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag != null)
            {
                InteractableUI interactable = eventData.pointerDrag.GetComponent<InteractableUI>();
                if (interactable == null) return;

                OnSlotEnter?.Invoke(new UIEventArgs(_insertedInteractable));

                _insertedInteractable = interactable;
                interactable.transform.position = transform.position;
            }
        }
    }
}
