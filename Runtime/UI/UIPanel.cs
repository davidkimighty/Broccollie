using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CollieMollie.UI
{
    public class UIPanel : BaseUI
    {
        #region Variable Field
        public event Action<UIEventArgs> OnShow = null;
        public event Action<UIEventArgs> OnHide = null;

        [SerializeField] private Canvas _canvas = null;
        [SerializeField] private UIButton _triggerButton = null;

        [SerializeField] private bool _visible = true;
        public bool IsVisible
        {
            get => _visible;
        }
        #endregion

        private void Awake()
        {
            if (_triggerButton != null)
            {
                _triggerButton.OnSelected += (eventArgs) => SetVisible(true);
            }
        }

        private void Start()
        {
            if (_canvas.enabled != _visible)
                _canvas.enabled = _visible;
        }

        #region Public Functions
        public void SetVisible(bool isVisible)
        {
            if (_canvas.enabled != isVisible)
                _canvas.enabled = isVisible;

            if (isVisible)
                OnShow?.Invoke(new UIEventArgs(this));
            else
                OnHide?.Invoke(new UIEventArgs(this));
        }

        public void SetVisibleQuietly(bool isVisible)
        {
            if (_canvas.enabled != isVisible)
                _canvas.enabled = isVisible;
        }
        #endregion
    }
}
