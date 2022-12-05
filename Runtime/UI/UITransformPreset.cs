using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CollieMollie.UI;
using UnityEngine;

namespace CollieMollie.UI
{
    [Serializable]
    public class UITransformPreset : IUIPreset
    {
        public Setting[] States = null;

        public float GetDuration(string state)
        {
            Setting setting = Array.Find(States, x => x.ExecutionState.ToString() == state);
            if (IsValid(setting.ExecutionState))
            {
                float[] durations = new float[]
                {
                    setting.PositionSettingDuration,
                    setting.ScaleSettingDuration
                };
                return durations.Max();
            }
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
            public UIAllState ExecutionState;

            [Header("Position")]
            public bool PositionSettingEnabled;
            public Transform TargetPosition;
            public float PositionSettingDuration;
            public AnimationCurve PositionSettingCurve;

            [Header("Scale")]
            public bool ScaleSettingEnabled;
            public float TargetScale;
            public float ScaleSettingDuration;
            public AnimationCurve ScaleSettingCurve;
        }
    }
}
