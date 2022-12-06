using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CollieMollie.Core;
using CollieMollie.Helper;
using UnityEditor.Presets;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CollieMollie.UI
{
    public class UIColorFeature : BaseUIFeature
    {
        #region Variable Field
        [SerializeField] private bool _isEnabled = true;
        [SerializeField] private List<Element> _elements = null;

        private Operation _featureOperation = new Operation();

        #endregion

        #region Public Functions
        public override void Execute(string state, out float duration, Action done = null)
        {
            duration = 0;
            if (!_isEnabled) return;

            _featureOperation.Stop(this);
            List<float> durations = new List<float>();
            foreach (Element element in _elements)
            {
                if (!element.IsEnabled) continue;
                _featureOperation.Add(element.ChangeColor(state));
                durations.Add(element.Preset.GetDuration(state));
            }
            duration = durations.Count > 0 ? durations.Max() : 0;
            _featureOperation.Start(this, duration, done);
        }

        #endregion

        [Serializable]
        public class Element
        {
            public bool IsEnabled = true;
            public MaskableGraphic Graphic = null;
            public UIColorPreset Preset = null;

            public IEnumerator ChangeColor(string state)
            {
                UIColorPreset.Setting setting = Array.Find(Preset.States, x => x.ExecutionState.ToString() == state);
                if (Preset.IsValid(setting.ExecutionState) && setting.IsEnabled)
                {
                    yield return Graphic.ChangeColorGradually(setting.TargetColor, setting.Duration, setting.Curve);
                }
            }

            public void ChangeColorInstant(string state)
            {
                UIColorPreset.Setting setting = Array.Find(Preset.States, x => x.ExecutionState.ToString() == state);
                if (Preset.IsValid(setting.ExecutionState) && setting.IsEnabled)
                {
                    Graphic.color = setting.TargetColor;
                }
            }
        }
    }
}
