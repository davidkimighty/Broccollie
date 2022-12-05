using System;
using CollieMollie.Core;
using UnityEngine;

namespace CollieMollie.UI
{
    [CreateAssetMenu(fileName = "UIColorPreset", menuName = "CollieMollie/UI/ColorPreset")]
    public class UIColorPreset : ScriptableObject, IUIPreset
    {
        public Setting[] States = null;

        public float GetDuration(string state)
        {
            Setting setting = Array.Find(States, x => x.ExecutionState.ToString() == state);
            if (IsValid(setting.ExecutionState))
                return setting.Duration;
            return 0;
        }

        public bool IsValid(UIAllState state)
        {
            return state != UIAllState.None;
        }

        [Serializable]
        public struct Setting
        {
            public bool IsEnabled;
            public Color TargetColor;
            public float Duration;
            public AnimationCurve Curve;
            public UIAllState ExecutionState;
        }
    }
}
