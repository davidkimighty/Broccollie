using System;
using System.Collections;
using System.Collections.Generic;
using CollieMollie.Helper;
using UnityEngine;
using UnityEngine.UI;

namespace CollieMollie.UI
{
    public class UIColorFeature : MonoBehaviour
    {
        #region Variable Field
        [SerializeField] private bool _isEnabled = true;
        [SerializeField] private List<Element> _elements = null;
        #endregion

        #region ColorChanger Functions
        public void ChangeGradually(ButtonState state)
        {
            if (!_isEnabled) return;

            foreach (Element element in _elements)
            {
                if (!element.IsEnabled) continue;
                element.ChangeColor(this, state);
            }
        }

        public void ChangeInstantly(ButtonState state)
        {
            if (!_isEnabled) return;

            foreach (Element element in _elements)
            {
                if (!element.IsEnabled) continue;
                element.ChangeColor(this, state, true);
            }
        }
        #endregion

        [Serializable]
        public class Element
        {
            #region Variabled Field
            public bool IsEnabled = true;
            public MaskableGraphic Graphic = null;
            public UIColorPreset Preset = null;

            private IEnumerator _colorChangeAction = null;
            #endregion

            #region ColorChanger Element Functions
            public void ChangeColor(MonoBehaviour mono, ButtonState state, bool instantChange = false)
            {
                if (_colorChangeAction != null)
                    mono.StopCoroutine(_colorChangeAction);

                UIColorPreset.ColorState? colorState = Array.Find(Preset.ColorStates, x => x.ExecutionState == state);
                if (colorState == null)
                    colorState = Array.Find(Preset.ColorStates, x => x.ExecutionState == ButtonState.Default);

                if (colorState != null)
                {
                    if (!colorState.Value.IsEnabled) return;

                    if (!instantChange)
                    {
                        _colorChangeAction = Graphic.ChangeColorGradually(colorState.Value.TargetColor, colorState.Value.Duration, colorState.Value.Curve);
                        mono.StartCoroutine(_colorChangeAction);
                    }
                    else
                    {
                        Graphic.color = colorState.Value.TargetColor;
                    }
                }
            }
            #endregion
        }
    }
}
