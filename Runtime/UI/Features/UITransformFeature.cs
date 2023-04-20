using System;
using System.Collections;
using System.Collections.Generic;
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
        protected override List<IEnumerator> GetFeatures(UIStates state)
        {
            List<IEnumerator> features = new List<IEnumerator>();
            if (_elements == null) return features;

            for (int i = 0; i < _elements.Length; i++)
            {
                if (!_elements[i].IsEnabled || _elements[i].Preset == null) continue;

                UITransformPreset.TransformSetting setting = Array.Find(_elements[i].Preset.Settings, x => x.ExecutionState == state);
                if (setting == null || !setting.IsEnabled) continue;

                Element.ReferenceTransform refTarget = _elements[i].ReferenceTransforms.Find(x => x.ExecutionState == state);
                if (refTarget == null) continue;

                if (setting.IsPositionEnabled)
                    features.Add(TransformPosition(_elements[i].Target, setting, refTarget.Reference.position));

                if (setting.IsScaleEnabled)
                    features.Add(TransformScale(_elements[i].Target, setting, refTarget.Reference.localScale));
            }
            return features;
        }

        public override void ExecuteFeatureInstant(UIStates state)
        {
            if (_elements == null) return;

            base.ExecuteFeatureInstant(state);
            for (int i = 0; i < _elements.Length; i++)
            {
                if (!_elements[i].IsEnabled || _elements[i].Preset == null) continue;

                UITransformPreset.TransformSetting setting = Array.Find(_elements[i].Preset.Settings, x => x.ExecutionState == state);
                if (setting == null || !setting.IsEnabled) continue;

                Element.ReferenceTransform refTarget = _elements[i].ReferenceTransforms.Find(x => x.ExecutionState == state);
                if (refTarget == null) continue;

                if (setting.IsPositionEnabled)
                    TransformPositionInstant(_elements[i].Target, setting, refTarget.Reference.position);

                if (setting.IsScaleEnabled)
                    TransformScaleInstant(_elements[i].Target, setting, refTarget.Reference.localScale);
            }
        }

        #endregion

        #region Private Functions
        private IEnumerator TransformPosition(Transform target, UITransformPreset.TransformSetting setting, Vector3 targetPosition)
        {
            yield return target.LerpPosition(targetPosition, setting.PositionDuration, setting.PositionCurve);
        }

        private IEnumerator TransformScale(Transform target, UITransformPreset.TransformSetting setting, Vector3 targetScale)
        {
            yield return target.LerpScale(targetScale, setting.ScaleDuration, setting.ScaleCurve);
        }

        private void TransformPositionInstant(Transform target, UITransformPreset.TransformSetting setting, Vector3 targetPosition)
        {
            target.position = targetPosition;
        }

        private void TransformScaleInstant(Transform target, UITransformPreset.TransformSetting setting, Vector3 targetScale)
        {
            target.localScale = targetScale;
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
