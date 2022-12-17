using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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

        private Task _behaviorTask = null;
        private readonly CancellationTokenSource _cancelSource = new CancellationTokenSource();

        #endregion

        #region Interactions
        protected override void InvokeEnterAction(PointerEventData eventData = null)
        {
            if (!_interactable) return;

            _hovering = true;
            HoveredBehavior();
        }

        protected override void InvokeExitAction(PointerEventData eventData = null)
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

        protected override void InvokeDownAction(PointerEventData eventData = null)
        {
            if (!_interactable) return;

            _pressed = true;
            PressedBehavior();
        }

        protected override void InvokeUpAction(PointerEventData eventData = null)
        {
            if (!_interactable) return;

            // Cancel Interaction
            _pressed = false;
            if (!_selected && !_hovering)
            {
                _selected = _pressed = _hovering = false;
                DefaultBehavior(false);
            }
            else if (_selected && !_hovering)
            {
                SelectedBehavior(false, false);
            }
        }

        protected override sealed void InvokeClickAction(PointerEventData eventData = null)
        {
            if (!_interactable) return;

            if (_type == ButtonType.Button)
            {
                RaiseSelectedEvent(new UIEventArgs(this));

                if (_hovering)
                    HoveredBehavior(false, false, () => RaiseDefaultEvent(new UIEventArgs(this)));
                else
                    DefaultBehavior(true, false, () => RaiseDefaultEvent(new UIEventArgs(this)));
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

        protected override void InvokeBeginDragAction(PointerEventData eventData = null)
        {
            if (!_interactable) return;

            _dragging = true;
            BeginDragBehavior(eventData);

            if (_dragFeature != null)
                _dragFeature.SetBlocksRaycasts(false);
        }

        protected override void InvokeDragAction(PointerEventData eventData = null)
        {
            if (!_interactable) return;

            DragBehavior(eventData, false, true);
        }

        protected override void InvokeEndDragAction(PointerEventData eventData = null)
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
            _currentState = State.Default;
            _selected = false;

            if (invokeEvent)
                RaiseDefaultEvent(new UIEventArgs(this));

            _behaviorTask = ExecuteFeaturesAsync(State.Default.ToString(), playAudio, done);
        }

        protected override void InteractiveBehavior(bool playAudio = true, bool invokeEvent = true, Action done = null)
        {
            _currentState = State.Interactive;

            if (invokeEvent)
                RaiseInteractiveEvent(new UIEventArgs(this));

            _behaviorTask = ExecuteFeaturesAsync(State.Interactive.ToString(), playAudio, done);
        }

        protected override void NonInteractiveBehavior(bool playAudio = true, bool invokeEvent = true, Action done = null)
        {
            _currentState = State.NonInteractive;

            if (invokeEvent)
                RaiseNonInteractiveEvent(new UIEventArgs(this));

            _behaviorTask = ExecuteFeaturesAsync(State.NonInteractive.ToString(), playAudio, done);
        }

        protected override void ShowBehavior(bool playAudio = true, bool invokeEvent = true, Action done = null)
        {
            _currentState = State.Show;

            if (invokeEvent)
                RaiseShowEvent(new UIEventArgs(this));

            _behaviorTask = ExecuteFeaturesAsync(State.Show.ToString(), playAudio, done);
        }

        protected override void HideBehavior(bool playAudio = true, bool invokeEvent = true, Action done = null)
        {
            _currentState = State.Hide;

            if (invokeEvent)
                RaiseHideEvent(new UIEventArgs(this));

            _behaviorTask = ExecuteFeaturesAsync(State.Hide.ToString(), playAudio, done);
        }

        protected override void HoveredBehavior(bool playAudio = true, bool invokeEvent = true, Action done = null)
        {
            _currentState = State.Hovered;

            if (invokeEvent)
                RaiseHoveredEvent(new UIEventArgs(this));

            _behaviorTask = ExecuteFeaturesAsync(State.Hovered.ToString(), playAudio, done);
        }

        protected override void PressedBehavior(bool playAudio = true, bool invokeEvent = true, Action done = null)
        {
            _currentState = State.Pressed;

            if (invokeEvent)
                RaisePressedEvent(new UIEventArgs(this));

            _behaviorTask = ExecuteFeaturesAsync(State.Pressed.ToString(), playAudio, done);
        }

        protected override void SelectedBehavior(bool playAudio = true, bool invokeEvent = true, Action done = null)
        {
            _currentState = State.Selected;

            if (invokeEvent)
                RaiseSelectedEvent(new UIEventArgs(this));

            _behaviorTask = ExecuteFeaturesAsync(State.Selected.ToString(), playAudio, done);
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
                _behaviorTask = _dragFeature.ExecuteAsync(eventData);
        }

        protected override void EndDragBehavior(PointerEventData eventData, bool invokeEvent = true)
        {
            if (invokeEvent)
                RaiseEndDragEvent(new UIEventArgs(this));
        }
        
        #endregion

        #region Features
        protected override void SetActive(bool state)
        {
            if (_buttonObject.activeSelf != state)
                _buttonObject.SetActive(state);
        }

        private async Task ExecuteFeaturesAsync(string state, bool playAudio, Action done = null)
        {
            List<Task> featureTasks = new List<Task>();
            if (_colorFeature != null)
                featureTasks.Add(_colorFeature.ExecuteAsync(state));

            if (_spriteFeature != null)
                featureTasks.Add(_spriteFeature.ExecuteAsync(state));

            if (_transformFeature != null)
                featureTasks.Add(_transformFeature.ExecuteAsync(state));

            if (_animationFeature != null)
                featureTasks.Add(_animationFeature.ExecuteAsync(state));

            if (_audioFeature != null && playAudio)
                featureTasks.Add(_audioFeature.ExecuteAsync(state));

            await Task.WhenAll(featureTasks);
            done?.Invoke();
        }

        #endregion
    }

    public enum ButtonType { Button, Radio, Checkbox }
}
