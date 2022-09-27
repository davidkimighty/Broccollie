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

        [SerializeField] private bool _visible = true;
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
        public void SetPanelVisible(bool isVisible)
        {
            if (_canvas.enabled != isVisible)
                _canvas.enabled = isVisible;

            if (isVisible)
                OnShow?.Invoke(new UIEventArgs(this));
            else
                OnHide?.Invoke(new UIEventArgs(this));
        }

        public void SetPanelVisibleQuietly(bool isVisible)
        {
            if (_canvas.enabled != isVisible)
                _canvas.enabled = isVisible;
        }
        #endregion
    }
}
