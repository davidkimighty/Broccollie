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
            SetPanelVisibleQuietly(_visible);
        }

        #region Panel Features
        public virtual void SetPanelVisible(bool isVisible)
        {
            if (_canvas.enabled != isVisible)
            {
                _canvas.enabled = isVisible;
                _visible = isVisible;
            }

            if (isVisible)
                OnShow?.Invoke(new UIEventArgs(this));
            else
                OnHide?.Invoke(new UIEventArgs(this));
        }

        public virtual void SetPanelVisibleQuietly(bool isVisible)
        {
            if (_canvas.enabled != isVisible)
                _canvas.enabled = isVisible;
        }
        #endregion
    }
}
