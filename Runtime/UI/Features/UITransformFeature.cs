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

        #endregion

        #region Private Functions
        private IEnumerator TransformScale(Transform target, UITransformPreset.TransformSetting setting)
        {
            yield return target.LerpScale(Vector3.one * setting.TargetScale, setting.ScaleDuration, setting.ScaleCurve);
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
