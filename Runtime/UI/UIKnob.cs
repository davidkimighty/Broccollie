using System.Collections;
using System.Collections.Generic;
using CollieMollie.Core;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CollieMollie.UI
{
    public class UIKnob : BasePointerInteractable
    {
        #region Variable Field
        [SerializeField] private GameObject _knobObject = null;

        [SerializeField] private UIColorFeature _colorFeature = null;

        #endregion

        #region Button Interactions
        protected override void InvokeClickAction(PointerEventData eventData = null)
        {
            if (!_interactable || _selected) return;

            _selected = true;

            RaiseSelectedEvent(new InteractableEventArgs(this));

            if (_selected)
                SelectedBehavior(false, true, false);
        }

        #endregion

        #region Button Features
        protected override void SetActive(bool state)
        {
            
        }

        protected override void ChangeColorFeature(InteractionState state, bool instantChange = false)
        {
            if (_colorFeature == null) return;

            if (instantChange)
                _colorFeature.ChangeInstantly(state);
            else
                _colorFeature.ChangeGradually(state);
        }
        #endregion
    }
}
