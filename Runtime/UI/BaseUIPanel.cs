using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CollieMollie.UI
{
    public abstract class BaseUIPanel : BaseUI
    {
        #region Variable Field
        public event Action<UIEventArgs> OnShow = null;
        public event Action<UIEventArgs> OnHide = null;

        [SerializeField] private Canvas _canvas = null;

        [SerializeField] protected bool _visible = true;
        public bool IsVisible
        {
            get => _visible;
        }
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

        #endregion

        #region Panel Features
        protected virtual void OnPanelShow()
        {
            OnShow?.Invoke(new UIEventArgs(this));
        }

        protected virtual void OnPanelHide()
        {
            OnHide?.Invoke(new UIEventArgs(this));
        }
        #endregion
    }
}
