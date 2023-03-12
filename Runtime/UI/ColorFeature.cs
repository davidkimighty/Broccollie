using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using CollieMollie.Core;
using UnityEngine;
using UnityEngine.UI;

namespace CollieMollie.UI
{
    public class ColorFeature : MonoBehaviour
    {
        #region Variable Field
        [Header("Feature")]
        [SerializeField] private bool _isEnabled = true;
        [SerializeField] private Element[] _elements = null;

        private List<IEnumerator> _colorChangeCoroutines = new List<IEnumerator>();

        #endregion

        #region Public Functions
        public async Task ChangeColorAsync(UIStates state)
        {
            if (!_isEnabled) return;

            foreach (IEnumerator coroutine in _colorChangeCoroutines)
                StopCoroutine(coroutine);

            for (int i = 0; i < _elements.Length; i++)
            {
                if (!_elements[i].IsEnabled) continue;

                ColorPreset.Setting setting = Array.Find(_elements[i].Preset.Settings, x => x.ExecutionState == state);
                if (!setting.IsEnabled) continue;

                _colorChangeCoroutines.Add(ChangeColor(_elements[i].Graphic, setting));
            }

            foreach (IEnumerator coroutine in _colorChangeCoroutines)
                StartCoroutine(coroutine);
            await Task.Yield();
        }

        #endregion

        #region Private Functions
        public IEnumerator ChangeColor(MaskableGraphic graphic, ColorPreset.Setting setting)
        {
            yield return graphic.ChangeColorGradually(setting.TargetColor, setting.Duration, setting.Curve);
        }

        public void ChangeColorInstant(MaskableGraphic graphic, ColorPreset.Setting setting)
        {
            graphic.color = setting.TargetColor;
        }

        #endregion

        [Serializable]
        public struct Element
        {
            public bool IsEnabled;
            public MaskableGraphic Graphic;
            public ColorPreset Preset;
        }
    }
}
