using System;
using UnityEngine;

namespace Broccollie.UI
{
    [CreateAssetMenu(fileName = "ColorPreset", menuName = "Broccollie/UI/Preset/Color")]
    public class ColorUIFeaturePreset : ScriptableObject
    {
        public Setting[] Settings;

        [Serializable]
        public struct Setting
        {
            public bool IsEnabled;
            public UIStates ExecutionState;
            public Color TargetColor;
            public string ColorPaletteKey;
            public float Duration;
            public AnimationCurve Curve;
        }
    }
}
