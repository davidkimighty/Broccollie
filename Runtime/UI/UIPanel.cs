using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using CollieMollie.Core;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CollieMollie.UI
{
    public class UIPanel : BasePointerInteractable
    {
        #region Variable Field
        [Header("Panel")]
        // Nested Canvas method has lot of bugs in Unity 2021.3.
        // [SerializeField] private Canvas _canvas = null;
        [SerializeField] private GameObject _popupObject = null;

        [SerializeField] private UIColorFeature _colorFeature = null;
        [SerializeField] private UIAudioFeature _audioFeature = null;
        [SerializeField] private UISpriteFeature _spriteFeature = null;
        [SerializeField] private UIAnimationFeature _animationFeature = null;

        #endregion

        #region Panel Interactions
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

        protected override void InvokeClickAction(PointerEventData eventData = null)
        {
            base.InvokeClickAction(eventData);
        }
        #endregion

        #region Features
        protected override void SetActive(bool state)
        {
            _popupObject.SetActive(state);
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

        #endregion
    }
}
