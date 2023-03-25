using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Broccollie.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Broccollie.UI
{
    public class UIColorFeature : UIBaseFeature
    {
        #region Variable Field
        [Header("Feature")]
        [SerializeField] private Element[] _elements = null;

        #endregion

        #region Override Functions
        protected override List<IEnumerator> GetFeatures(UIStates state)
        {
            List<IEnumerator> features = new List<IEnumerator>();
            for (int i = 0; i < _elements.Length; i++)
            {
                if (!_elements[i].IsEnabled || _elements[i].Preset == null) continue;

                UIColorPreset.ColorSetting setting = Array.Find(_elements[i].Preset.Settings, x => x.ExecutionState == state);
                if (setting == null || !setting.IsEnabled) continue;

                features.Add(ChangeColor(_elements[i].Graphic, setting));
            }
            return features;
        }

        public override void ExecuteFeatureInstant(UIStates state)
        {
            base.ExecuteFeatureInstant(state);

            for (int i = 0; i < _elements.Length; i++)
            {
                if (!_elements[i].IsEnabled || _elements[i].Preset == null) continue;

                UIColorPreset.ColorSetting setting = Array.Find(_elements[i].Preset.Settings, x => x.ExecutionState == state);
                if (setting == null || !setting.IsEnabled) continue;

                ChangeColorInstant(_elements[i].Graphic, setting);
            }
        }

        #endregion

        #region Private Functions
        private IEnumerator ChangeColor(MaskableGraphic graphic, UIColorPreset.ColorSetting setting)
        {
            yield return graphic.ChangeColorGradually(setting.TargetColor, setting.Duration, setting.Curve);
        }

        private void ChangeColorInstant(MaskableGraphic graphic, UIColorPreset.ColorSetting setting)
        {
            graphic.color = setting.TargetColor;
        }

        #endregion

        [Serializable]
        public struct Element
        {
            public bool IsEnabled;
            public MaskableGraphic Graphic;
            public UIColorPreset Preset;
        }
    }
}
