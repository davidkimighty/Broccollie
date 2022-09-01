using System;
using System.Collections;
using System.Collections.Generic;
using CollieMollie.Helper;
using UnityEngine;
using UnityEngine.UI;

namespace CollieMollie.UI
{
    public class UIColorChanger : MonoBehaviour
    {
        #region Variable Field
        [Header("Color Changer")]
        [SerializeField] private bool isEnabled = true;
        [SerializeField] private List<Element> elements = null;
        #endregion

        #region ColorChanger Functions
        public void ChangeGradually(ButtonState state)
        {
            if (!isEnabled) return;

            foreach (Element element in elements)
            {
                if (!element.isEnabled) continue;
                element.ChangeColor(this, state);
            }
        }

        public void ChangeInstantly(ButtonState state)
        {
            if (!isEnabled) return;

            foreach (Element element in elements)
            {
                if (!element.isEnabled) continue;
                element.ChangeColor(this, state, true);
            }
        }
        #endregion

        [Serializable]
        public class Element
        {
            #region Variabled Field
            public bool isEnabled = true;
            public MaskableGraphic graphic = null;
            public UIColorPreset preset = null;

            private IEnumerator colorChangeAction = null;
            #endregion

            #region ColorChanger Element Functions
            public void ChangeColor(MonoBehaviour mono, ButtonState state, bool instantChange = false)
            {
                if (colorChangeAction != null)
                    mono.StopCoroutine(colorChangeAction);

                UIColorPreset.ColorState colorState = Array.Find(preset.colorStates, x => x.executionState == state);
                if (colorState == null)
                    colorState = Array.Find(preset.colorStates, x => x.executionState == ButtonState.Default);

                if (colorState != null)
                {
                    if (!colorState.isEnabled) return;

                    if (!instantChange)
                    {
                        colorChangeAction = graphic.ChangeColorGradually(colorState.targetColor, colorState.duration, colorState.curve);
                        mono.StartCoroutine(colorChangeAction);
                    }
                    else
                    {
                        graphic.color = colorState.targetColor;
                    }
                }
            }
            #endregion
        }
    }
}
