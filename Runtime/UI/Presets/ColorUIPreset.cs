using System;
using UnityEngine;

namespace Broccollie.UI
{
    [CreateAssetMenu(fileName = "ColorPreset", menuName = "Broccollie/UI/Preset/Color")]
    public class ColorUIPreset : BaseUIPreset
    {
        public ColorSetting[] Settings = null;

        [Serializable]
        public class ColorSetting : Setting
        {
            public Color TargetColor;
            public float Duration;
            public AnimationCurve Curve;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            for (int i = 0; i < Settings.Length; i++)
            {
                if (Settings[i].ExecutionState == null || Settings[i].ExecutionState == string.Empty)
                    Settings[i].ExecutionState = Settings[i].ExecutionStateHelper.ToString();
            }
        }
#endif
    }
}
