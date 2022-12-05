using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CollieMollie.Core;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CollieMollie.UI
{
    public class UIButton : InteractableUI
    {
        #region Variable Field
        [Header("Button")]
        [SerializeField] private GameObject _buttonObject = null;
        [SerializeField] private ButtonType _type = ButtonType.Button;

        [Header("Button Features")]
        [SerializeField] private UIColorFeature _colorFeature = null;
        [SerializeField] private UISpriteFeature _spriteFeature = null;
        [SerializeField] private UITransformFeature _transformFeature = null;
        [SerializeField] private UIAnimationFeature _animationFeature = null;
        [SerializeField] private UIAudioFeature _audioFeature = null;
        [SerializeField] private UIDragFeature _dragFeature = null;

        #endregion

        #region Interactions
        protected override void InvokeEnterAction(PointerEventData eventData = null, UIEventArgs args = null)
        {
            if (!_interactable) return;

            _hovering = true;
            HoveredBehavior();
        }

        protected override void InvokeExitAction(PointerEventData eventData = null, UIEventArgs args = null)
        {
            if (!_interactable) return;

            _hovering = false;
            if (_selected)
            {
                SelectedBehavior(false, false);
            }
            else
            {
                _selected = _pressed = false;
                DefaultBehavior(false, false);
            }
        }

        protected override void InvokeDownAction(PointerEventData eventData = null, UIEventArgs args = null)
        {
            if (!_interactable) return;

            _pressed = true;
            PressedBehavior();
        }

        protected override void InvokeUpAction(PointerEventData eventData = null, UIEventArgs args = null)
        {
            if (!_interactable) return;

            // Cancel Interaction
            _pressed = false;
            if (!_selected && !_hovering)
            {
                _selected = _pressed = _hovering = false;
                DefaultBehavior(false, false);
            }
            else if (_selected && !_hovering)
            {
                SelectedBehavior(false, false);
            }
        }

        protected override sealed void InvokeClickAction(PointerEventData eventData = null, UIEventArgs args = null)
        {
            if (!_interactable) return;

            if (_type == ButtonType.Button)
            {
                RaiseSelectedEvent(new UIEventArgs(this));

                if (_hovering)
                    HoveredBehavior(false, false);
                else
                    DefaultBehavior(true, false);
            }
            else
            {
                _selected = _type switch
                {
                    ButtonType.Radio => true,
                    ButtonType.Checkbox => !_selected,
                    _ => false
                };

                if (_selected)
                    SelectedBehavior();
                else
                    DefaultBehavior();
            }
        }

        protected override void InvokeBeginDragAction(PointerEventData eventData = null, UIEventArgs args = null)
        {
            if (!_interactable) return;

            _dragging = true;
            BeginDragBehavior(eventData);

            if (_dragFeature != null)
                _dragFeature.SetBlocksRaycasts(false);
        }

        protected override void InvokeDragAction(PointerEventData eventData = null, UIEventArgs args = null)
        {
            if (!_interactable) return;

            DragBehavior(eventData, false, true);
        }

        protected override void InvokeEndDragAction(PointerEventData eventData = null, UIEventArgs args = null)
        {
            if (!_interactable) return;

            _dragging = false;
            EndDragBehavior(eventData);

            if (_dragFeature != null)
                _dragFeature.SetBlocksRaycasts(true);
        }

        #endregion

        #region Behaviors
        protected override void DefaultBehavior(bool playAudio = true, bool invokeEvent = true, Action done = null)
        {
            _currentState = UIState.Default;
            _currentInteractionState = UIInteractionState.None;
            _selected = false;

            if (invokeEvent)
                RaiseDefaultEvent(new UIEventArgs(this));
            
            SetFeatures(UIState.Default.ToString(), playAudio);
        }

        protected override void InteractiveBehavior(bool playAudio = true, bool invokeEvent = true, Action done = null)
        {
            _currentState = UIState.Interactive;

            if (invokeEvent)
                RaiseInteractiveEvent(new UIEventArgs(this));

            SetFeatures(UIState.Interactive.ToString(), playAudio);
        }

        protected override void NonInteractiveBehavior(bool playAudio = true, bool invokeEvent = true, Action done = null)
        {
            _currentState = UIState.NonInteractive;

            if (invokeEvent)
                RaiseNonInteractiveEvent(new UIEventArgs(this));

            SetFeatures(UIState.NonInteractive.ToString(), playAudio);
        }

        protected override void ShowBehavior(bool playAudio = true, bool invokeEvent = true, Action done = null)
        {
            _currentState = UIState.Show;

            if (invokeEvent)
                RaiseShowEvent(new UIEventArgs(this));

            SetFeatures(UIState.Show.ToString(), playAudio);
        }

        protected override void HideBehavior(bool playAudio = true, bool invokeEvent = true, Action done = null)
        {
            _currentState = UIState.Hide;

            if (invokeEvent)
                RaiseHideEvent(new UIEventArgs(this));

            SetFeatures(UIState.Hide.ToString(), playAudio);
        }

        protected override void HoveredBehavior(bool playAudio = true, bool invokeEvent = true)
        {
            _currentInteractionState = UIInteractionState.Hovered;

            if (invokeEvent)
                RaiseHoveredEvent(new UIEventArgs(this));

            SetFeatures(UIInteractionState.Hovered.ToString(), playAudio);
        }

        protected override void PressedBehavior(bool playAudio = true, bool invokeEvent = true)
        {
            _currentInteractionState = UIInteractionState.Pressed;

            if (invokeEvent)
                RaisePressedEvent(new UIEventArgs(this));

            SetFeatures(UIInteractionState.Pressed.ToString(), playAudio);
        }

        protected override void SelectedBehavior(bool playAudio = true, bool invokeEvent = true)
        {
            _currentInteractionState = UIInteractionState.Selected;

            if (invokeEvent)
                RaiseSelectedEvent(new UIEventArgs(this));

            SetFeatures(UIInteractionState.Selected.ToString(), playAudio);
        }

        protected override void BeginDragBehavior(PointerEventData eventData, bool invokeEvent = true)
        {
            if (invokeEvent)
                RaiseBeginDragEvent(new UIEventArgs(this));
        }

        protected override void DragBehavior(PointerEventData eventData, bool playAudio = true, bool invokeEvent = true)
        {
            if (invokeEvent)
                RaiseDragEvent(new UIEventArgs(this));

            if (_dragFeature != null)
                _dragFeature.Execute(UIInteractionState.Drag.ToString(), eventData);
        }

        protected override void EndDragBehavior(PointerEventData eventData, bool invokeEvent = true)
        {
            if (invokeEvent)
                RaiseEndDragEvent(new UIEventArgs(this));
        }
        
        #endregion

        #region Button Features
        protected override void SetActive(bool state)
        {
            _buttonObject.SetActive(state);
        }

        private void SetFeatures(string state, bool playAudio, PointerEventData eventData = null)
        {
            if (_colorFeature != null)
                _colorFeature.Execute(state);

            if (_spriteFeature != null)
                _spriteFeature.Execute(state);

            if (_transformFeature != null)
                _transformFeature.Execute(state);

            if (_animationFeature != null)
                _animationFeature.Execute(state);

            if (_audioFeature != null && playAudio)
                _audioFeature.Execute(state);
        }

        #endregion
    }

    public enum ButtonType { Button, Radio, Checkbox }
}
