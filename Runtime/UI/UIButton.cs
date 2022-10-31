using System;
using System.Collections;
using CollieMollie.Core;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CollieMollie.UI
{
    public class UIButton : BasePointerInteractable
    {
        #region Variable Field
        [Header("Button")]
        [SerializeField] private GameObject _buttonObject = null;
        [SerializeField] private ButtonType _type = ButtonType.Button;
        [SerializeField] private UIColorFeature _colorFeature = null;
        [SerializeField] private UIAudioFeature _audioFeature = null;
        [SerializeField] private UISpriteFeature _spriteFeature = null;
        [SerializeField] private UIAnimationFeature _animationFeature = null;
        [SerializeField] private UIDragFeature _dragFeature = null;

        #endregion

        #region Button Interactions
        protected override void InvokeEnterAction(PointerEventData eventData = null)
        {
            base.InvokeEnterAction(eventData);
        }

        protected override void InvokeExitAction(PointerEventData eventData = null)
        {
            base.InvokeExitAction(eventData);
        }

        protected override void InvokeDownAction(PointerEventData eventData = null)
        {
            base.InvokeDownAction(eventData);
        }

        protected override void InvokeUpAction(PointerEventData eventData = null)
        {
            base.InvokeUpAction(eventData);
        }

        protected override sealed void InvokeClickAction(PointerEventData eventData = null)
        {
            if (!_interactable) return;

            _selected = _type switch
            {
                ButtonType.Radio => true,
                ButtonType.Checkbox => !_selected,
                _ => false
            };

            if (_selected)
                SelectedBehavior(false, true, false);
            else
                DefaultBehavior(false, true, false);

            if (_hovering)
                HoveredBehavior(false, true, false);

            RaiseSelectedEvent(new InteractableEventArgs(this));
        }

        protected override void BeginDragAction(PointerEventData eventData = null)
        {
            base.BeginDragAction(eventData);

            if (_dragFeature != null)
                _dragFeature.SetBlocksRaycasts(false);
        }

        protected override void DragAction(PointerEventData eventData = null)
        {
            base.DragAction(eventData);
        }

        protected override void EndDragAction(PointerEventData eventData = null)
        {
            base.EndDragAction(eventData);

            if (_dragFeature != null)
                _dragFeature.SetBlocksRaycasts(true);
        }
        #endregion

        #region Button Features
        protected override void SetActive(bool state)
        {
            _buttonObject.SetActive(state);
        }

        protected override void ChangeColorFeature(InteractionState state, bool instantChange = false)
        {
            if (_colorFeature == null) return;

            if (instantChange)
                _colorFeature.ChangeInstantly(state);
            else
                _colorFeature.ChangeGradually(state);
        }

        protected override void ChangeSpriteFeature(InteractionState state)
        {
            if (_spriteFeature == null) return;

            _spriteFeature.Change(state);
        }

        protected override void PlayAudioFeature(InteractionState state)
        {
            if (_audioFeature == null) return;

            _audioFeature.Play(state);
        }

        protected override void PlayAnimationFeature(InteractionState state)
        {
            if (_animationFeature == null) return;

            _animationFeature.Change(state);
        }

        protected override void DragFeature(PointerEventData eventData)
        {
            if (_dragFeature == null) return;

            _dragFeature.Drag(eventData);
        }
        #endregion
    }

    public enum ButtonType { Button, Radio, Checkbox }
    
}
