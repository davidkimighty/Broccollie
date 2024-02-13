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
        [SerializeField] private Element[] _elements = null;

        private Vector3[] _startPositions = null;
        private bool _initialized = false;

        #region Public Functions
        public override List<Task> GetFeatures(UIStates state, bool instantChange, bool playAudio, CancellationToken ct)
        {
            try
            {
                if (_elements == null) return default;
                Initialize();

                List<Task> features = new();
                for (int i = 0; i < _elements.Length; i++)
                {
                    ct.ThrowIfCancellationRequested();
                    if (!_elements[i].IsEnabled || _elements[i].Preset == null) continue;

                    TransformUIFeaturePreset.Setting setting = Array.Find(_elements[i].Preset.Settings, x => x.ExecutionState == state);
                    if (setting.IsPositionEnabled)
                    {
                        Vector3 targetValue = state != UIStates.Default ? _startPositions[i] + setting.TargetPosition : _startPositions[i];
                        features.Add(_elements[i].Target.LerpLocalPositionAsync(targetValue, setting.PositionDuration, ct, setting.PositionCurve));
                    }

                    if (setting.IsRotationEnabled)
                        features.Add(_elements[i].Target.LerpRotationAsync(Quaternion.Euler(setting.TargetRotation), setting.RotationDuration, ct, setting.RotationCurve));

                    if (setting.IsScaleEnabled)
                        features.Add(_elements[i].Target.LerpScaleAsync(setting.TargetScale, setting.ScaleDuration, ct, setting.ScaleCurve));
                }
                return features;
            }
            catch (OperationCanceledException)
            {
                return default;
            }
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
        public struct Element
        {
            public bool IsEnabled;
            public Transform Target;
            public TransformUIFeaturePreset Preset;
        }
    }
}
