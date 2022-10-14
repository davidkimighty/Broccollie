using System;
using System.Collections;
using System.Collections.Generic;
using CollieMollie.Core;
using UnityEngine;

namespace CollieMollie.UI
{
    public abstract class BaseUIPanel : BasePointerInteractable
    {
        #region Variable Field
        public event Action<InteractableEventArgs> OnShow = null;
        public event Action<InteractableEventArgs> OnHide = null;

        [SerializeField] private Canvas _canvas = null;

        [SerializeField] protected bool _visible = true;
        public bool IsVisible
        {
            get => _visible;
        }

        [SerializeField] private UIAnimationFeature _animationFeature = null;
        #endregion

        protected virtual void Awake()
        {
            if (_canvas.enabled != _visible)
                _canvas.enabled = _visible;
        }

        #region Public Functions
        public virtual void SetPanelVisible(bool isVisible, bool invokeEvent = true)
        {
            if (_canvas.enabled != isVisible)
            {
                _canvas.enabled = isVisible;
                _visible = isVisible;
            }

            if (invokeEvent)
            {
                if (isVisible)
                    OnPanelShow();
                else
                    OnPanelHide();
            }
        }

        public void Show(float duration, bool invokeEvent = true)
        {
            SetPanelVisible(true, invokeEvent);
            StartCoroutine(ShowPanel(duration, () =>
            {

            }));
        }

        #endregion



        #region Panel Behaviors
        private IEnumerator ShowPanel(float duration, Action done = null)
        {
            _animationFeature.Change(InteractionState.Show);

            yield return new WaitForSeconds(duration);

            done?.Invoke();
        }

        #endregion

        #region Panel Features
        protected virtual void OnPanelShow()
        {
            OnShow?.Invoke(new InteractableEventArgs(this));
        }

        protected virtual void OnPanelHide()
        {
            OnHide?.Invoke(new InteractableEventArgs(this));
        }
        #endregion
    }
}
