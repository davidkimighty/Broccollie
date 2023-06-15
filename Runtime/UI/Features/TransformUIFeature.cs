using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Broccollie.Core;
using UnityEngine;

namespace Broccollie.UI
{
    public class TransformUIFeature : BaseUIFeature
    {
        #region Variable Field
        [Header("Transform Feature")]
        [SerializeField] private Element[] _elements = null;

        #endregion

        #region Override Functions
        protected override List<Task> GetFeatures(UIStates state, CancellationToken ct)
        {
            List<Task> features = new List<Task>();
            if (_elements == null) return features;

            for (int i = 0; i < _elements.Length; i++)
            {
                if (!_elements[i].IsEnabled || _elements[i].Preset == null) continue;

                TransformUIPreset.TransformSetting setting = Array.Find(_elements[i].Preset.Settings, x => x.ExecutionState == state);
                if (setting == null || !setting.IsEnabled) continue;

                if (setting.IsPositionEnabled)
                    features.Add(_elements[i].Target.LerpAnchoredPositionAsync(setting.AnchoredPosition, setting.PositionDuration, ct, setting.PositionCurve));

                if (setting.IsRotationEnabled)
                    features.Add(_elements[i].Target.LerpRotationAsync(Quaternion.Euler(setting.TargetRotation), setting.RotationDuration, ct, setting.RotationCurve));

                if (setting.IsScaleEnabled)
                    features.Add(_elements[i].Target.LerpScaleAsync(setting.TargetScale, setting.ScaleDuration, ct, setting.ScaleCurve));
            }
            return features;
        }

        protected override List<Action> GetFeaturesInstant(UIStates state)
        {
            List<Action> features = new List<Action>();
            if (_elements == null) return features;

            for (int i = 0; i < _elements.Length; i++)
            {
                if (!_elements[i].IsEnabled || _elements[i].Preset == null) continue;

                TransformUIPreset.TransformSetting setting = Array.Find(_elements[i].Preset.Settings, x => x.ExecutionState == state);
                if (setting == null || !setting.IsEnabled) continue;

                int index = i;
                if (setting.IsPositionEnabled)
                    features.Add(() => _elements[index].Target.anchoredPosition = setting.AnchoredPosition);

                if (setting.IsRotationEnabled)
                    features.Add(() => _elements[index].Target.rotation = Quaternion.Euler(setting.TargetRotation));

                if (setting.IsScaleEnabled)
                    features.Add(() => _elements[index].Target.localScale = setting.TargetScale);
            }
            return features;
        }

        #endregion

        [Serializable]
        public class Element
        {
            public bool IsEnabled;
            public RectTransform Target;
            public TransformUIPreset Preset;
        }
    }
}
