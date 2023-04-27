using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Broccollie.Core;
using UnityEngine;

namespace Broccollie.UI
{
    public class UITransformFeature : UIBaseFeature
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

                UITransformPreset.TransformSetting setting = Array.Find(_elements[i].Preset.Settings, x => x.ExecutionState == state);
                if (setting == null || !setting.IsEnabled) continue;

                Element.ReferenceTransform refTarget = _elements[i].ReferenceTransforms.Find(x => x.ExecutionState == state);
                if (refTarget == null) continue;

                if (setting.IsPositionEnabled)
                    features.Add(_elements[i].Target.LerpPositionAsync(refTarget.Reference.position, setting.PositionDuration, ct, setting.PositionCurve));

                if (setting.IsRotationEnabled)
                    features.Add(_elements[i].Target.LerpRotationAsync(refTarget.Reference.rotation, setting.RotationDuration, ct, setting.RotationCurve));

                if (setting.IsScaleEnabled)
                    features.Add(_elements[i].Target.LerpScaleAsync(refTarget.Reference.localScale, setting.ScaleDuration, ct, setting.ScaleCurve));
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

                UITransformPreset.TransformSetting setting = Array.Find(_elements[i].Preset.Settings, x => x.ExecutionState == state);
                if (setting == null || !setting.IsEnabled) continue;

                Element.ReferenceTransform refTarget = _elements[i].ReferenceTransforms.Find(x => x.ExecutionState == state);
                if (refTarget == null) continue;

                int index = i;
                if (setting.IsPositionEnabled)
                    features.Add(() => _elements[index].Target.position = refTarget.Reference.position);

                if (setting.IsRotationEnabled)
                    features.Add(() => _elements[index].Target.rotation = refTarget.Reference.rotation);

                if (setting.IsScaleEnabled)
                    features.Add(() => _elements[index].Target.localScale = refTarget.Reference.localScale);
            }
            return features;
        }

        #endregion

        [Serializable]
        public class Element
        {
            public bool IsEnabled;
            public Transform Target;
            public UITransformPreset Preset;
            public List<ReferenceTransform> ReferenceTransforms = null;

            [Serializable]
            public class ReferenceTransform
            {
                public UIStates ExecutionState = UIStates.Default;
                public Transform Reference = null;
            }
        }
    }
}
