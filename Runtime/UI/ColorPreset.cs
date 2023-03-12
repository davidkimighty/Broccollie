using System;
using UnityEngine;

namespace CollieMollie.UI
{
    [CreateAssetMenu(fileName = "ColorPreset", menuName = "CollieMollie/UI/Preset/Color")]
    public class ColorPreset : ScriptableObject
    {
        public Setting[] Settings = null;

        [Serializable]
        public struct Setting
        {
            public bool IsEnabled;
            public UIStates ExecutionState;
            public Color TargetColor;
            public float Duration;
            public AnimationCurve Curve;
        }
    }
}
