using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Broccollie.Core;
using UnityEngine;

namespace Broccollie.UI
{
    public class TransformUIFeature : BaseUIFeature
    {
        [Header("Transform Feature")]
        [SerializeField] private Element[] _elements = null;

        private Vector3[] _startPositions = null;
        private bool _initialized = false;

        #region Override Functions
        protected override List<Task> GetFeatures(string state, CancellationToken ct)
        {
            Initialize();

            List<Task> features = new List<Task>();
            if (_elements == null) return features;

            for (int i = 0; i < _elements.Length; i++)
            {
                if (!_elements[i].IsEnabled || _elements[i].Preset == null) continue;

                TransformUIPreset.TransformSetting setting = Array.Find(_elements[i].Preset.Settings, x => x.ExecutionState == state);
                if (setting == null || !setting.IsEnabled) continue;

                if (setting.IsPositionEnabled)
                {
                    Vector3 targetValue = state != UIStates.Default.ToString() ? _startPositions[i] + setting.TargetPosition : _startPositions[i];
                    features.Add(_elements[i].Target.LerpLocalPositionAsync(targetValue, setting.PositionDuration, ct, setting.PositionCurve));
                }

                if (setting.IsRotationEnabled)
                    features.Add(_elements[i].Target.LerpRotationAsync(Quaternion.Euler(setting.TargetRotation), setting.RotationDuration, ct, setting.RotationCurve));

                if (setting.IsScaleEnabled)
                    features.Add(_elements[i].Target.LerpScaleAsync(setting.TargetScale, setting.ScaleDuration, ct, setting.ScaleCurve));
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

                TransformUIPreset.TransformSetting setting = Array.Find(_elements[i].Preset.Settings, x => x.ExecutionState == state);
                if (setting == null || !setting.IsEnabled) continue;

                int index = i;
                if (setting.IsPositionEnabled)
                {
                    Vector3 targetValue = state != UIStates.Default.ToString() ? _startPositions[i] + setting.TargetPosition : setting.TargetPosition;
                    features.Add(() => _elements[index].Target.localPosition = setting.TargetPosition);
                }

                if (setting.IsRotationEnabled)
                    features.Add(() => _elements[index].Target.rotation = Quaternion.Euler(setting.TargetRotation));

                if (setting.IsScaleEnabled)
                    features.Add(() => _elements[index].Target.localScale = setting.TargetScale);
            }
            return features;
        }

        #endregion

        private void Initialize()
        {
            if (!_initialized || _startPositions.Length != _elements.Length)
            {
                List<Vector3> positions = new List<Vector3>();
                for (int i = 0; i < _elements.Length; i++)
                    positions.Add(_elements[i].Target.localPosition);
                _startPositions = positions.ToArray();
                _initialized = true;
            }
        }

        [Serializable]
        public class Element
        {
            public bool IsEnabled;
            public Transform Target;
            public TransformUIPreset Preset;
        }
    }
}
