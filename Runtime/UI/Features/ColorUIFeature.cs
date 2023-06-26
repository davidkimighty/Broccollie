using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Broccollie.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Broccollie.UI
{
    [DisallowMultipleComponent]
    public class ColorUIFeature : BaseUIFeature
    {
        [Header("Color Feature")]
        [SerializeField] private Element[] _elements = null;

        protected override List<Task> GetFeatures(string state, CancellationToken ct)
        {
            List<Task> features = new List<Task>();
            if (_elements == null) return features;

            for (int i = 0; i < _elements.Length; i++)
            {
                if (!_elements[i].IsEnabled || _elements[i].Preset == null) continue;

                ColorUIPreset.ColorSetting setting = Array.Find(_elements[i].Preset.Settings, x => x.ExecutionState == state);
                if (setting == null || !setting.IsEnabled) continue;

                features.Add(_elements[i].Graphic.LerpColorAsync(setting.TargetColor, setting.Duration, ct, setting.Curve));
            }
            return features;
        }

        protected override List<Action> GetFeaturesInstant(string state)
        {
            List<Action> features = new List<Action>();
            if (_elements == null) return features;

            for (int i = 0; i < _elements.Length; i++)
            {
                if (!_elements[i].IsEnabled || _elements[i].Preset == null) continue;

                ColorUIPreset.ColorSetting setting = Array.Find(_elements[i].Preset.Settings, x => x.ExecutionState == state);
                if (setting == null || !setting.IsEnabled) continue;

                int index = i;
                features.Add(() => _elements[index].Graphic.color = setting.TargetColor);
            }
            return features;
        }

        [Serializable]
        public class Element
        {
            public bool IsEnabled;
            public MaskableGraphic Graphic;
            public ColorUIPreset Preset;
        }
    }
}
