using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using CollieMollie.Core;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CollieMollie.UI
{
    public class UIPanel : InteractableUI
    {
        #region Variable Field
        [Header("Panel")]
        // [SerializeField] private Canvas _canvas = null;
        [SerializeField] private GameObject _popupObject = null;

        [SerializeField] private UIColorFeature _colorFeature = null;
        [SerializeField] private UISpriteFeature _spriteFeature = null;
        [SerializeField] private UITransformFeature _transformFeature = null;
        [SerializeField] private UIAnimationFeature _animationFeature = null;
        [SerializeField] private UIAudioFeature _audioFeature = null;

        #endregion

        #region Panel Interactions
        protected override void InvokeEnterAction(PointerEventData eventData = null, UIEventArgs args = null)
        {
            base.InvokeEnterAction(eventData);
        }

        protected override void InvokeExitAction(PointerEventData eventData = null, UIEventArgs args = null)
        {
            base.InvokeExitAction(eventData);
        }

        protected override void InvokeDownAction(PointerEventData eventData = null, UIEventArgs args = null)
        {
            base.InvokeDownAction(eventData);
        }

        protected override void InvokeUpAction(PointerEventData eventData = null, UIEventArgs args = null)
        {
            base.InvokeUpAction(eventData);
        }

        protected override void InvokeClickAction(PointerEventData eventData = null, UIEventArgs args = null)
        {
            base.InvokeClickAction(eventData);
        }

        #endregion

        #region Features
        protected override void SetActive(bool state)
        {
            _popupObject.SetActive(state);
        }

        //private void ChangeColorFeature(State state)
        //{
        //    if (_colorFeature == null) return;

        //    _colorFeature.ChangeGradually(state);
        //}

        //private void ChangeSpriteFeature(State state)
        //{
        //    if (_spriteFeature == null) return;

        //    _spriteFeature.Change(state);
        //}

        //private void PlayAudioFeature(State state)
        //{
        //    if (_audioFeature == null) return;

        //    _audioFeature.Play(state);
        //}

        //private void PlayAnimationFeature(State state)
        //{
        //    if (_animationFeature == null) return;

        //    _animationFeature.Change(state);
        //}

        //private void ChangeScaleFeature(State state)
        //{
        //    if (_scaleFeature == null) return;

        //    _scaleFeature.Change(state);
        //}

        #endregion
    }
}
