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
        [SerializeField] private ButtonType _type = ButtonType.Button;
        [SerializeField] private UIColorFeature _colorFeature = null;
        [SerializeField] private UIAudioFeature _audioFeature = null;
        [SerializeField] private UISpriteFeature _spriteFeature = null;
        [SerializeField] private UIAnimationFeature _animationFeature = null;

        #endregion

        protected override void Awake()
        {
            _interactableTarget = gameObject;

            base.Awake();
        }

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
        #endregion

        #region Button Features
        protected override void ChangeColors(InteractionState state, bool instantChange = false)
        {
            if (_colorFeature == null) return;

            if (instantChange)
                _colorFeature.ChangeInstantly(state);
            else
                _colorFeature.ChangeGradually(state);
        }

        protected override void ChangeSprites(InteractionState state)
        {
            if (_spriteFeature == null) return;

            _spriteFeature.Change(state);
        }

        protected override void PlayAudio(InteractionState state)
        {
            if (_audioFeature == null) return;

            _audioFeature.Play(state);
        }

        protected override void PlayAnimation(InteractionState state)
        {
            if (_animationFeature == null) return;

            _animationFeature.Change(state);
        }
        #endregion
    }

    public enum ButtonType { Button, Radio, Checkbox }
    
}
