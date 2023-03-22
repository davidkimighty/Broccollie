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
            for (int i = 0; i < _elements.Length; i++)
            {
                if (!_elements[i].IsEnabled) continue;

                UITransformPreset.TransformSetting setting = Array.Find(_elements[i].Preset.Settings, x => x.ExecutionState == state);
                if (!setting.IsEnabled) continue;

                features.Add(TransformScale(_elements[i].TargetTransform, setting));
            }
            return features;
        }

        public override void ExecuteFeatureInstant(UIStates state)
        {
            base.ExecuteFeatureInstant(state);

            for (int i = 0; i < _elements.Length; i++)
            {
                if (!_elements[i].IsEnabled) continue;

                UITransformPreset.TransformSetting setting = Array.Find(_elements[i].Preset.Settings, x => x.ExecutionState == state);
                if (!setting.IsEnabled) continue;

                TransformScaleInstant(_elements[i].TargetTransform, setting);
            }
        }

        #endregion

        #region Private Functions
        private IEnumerator TransformScale(Transform target, UITransformPreset.TransformSetting setting)
        {
            yield return target.LerpScale(Vector3.one * setting.TargetScale, setting.ScaleDuration, setting.ScaleCurve);
        }

        private void TransformScaleInstant(Transform target, UITransformPreset.TransformSetting setting)
        {
            target.localScale = Vector3.one * setting.TargetScale;
        }

        #endregion

        [Serializable]
        public struct Element
        {
            public bool IsEnabled;
            public Transform TargetTransform;
            public UITransformPreset Preset;
        }
    }
}
