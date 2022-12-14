using System;
using CollieMollie.Core;
using UnityEngine;

namespace CollieMollie.UI
{
    [CreateAssetMenu(fileName = "UIColorPreset", menuName = "CollieMollie/UI/ColorPreset")]
    public class UIColorPreset : ScriptableObject
    {
        public Setting[] States = null;

        [Serializable]
        public struct Setting
        {
            public bool IsEnabled;
            public Color TargetColor;
            public float Duration;
            public AnimationCurve Curve;
            public BaseUI.State ExecutionState;
        }
    }
}
