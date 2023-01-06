using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CollieMollie.UI;
using UnityEngine;

namespace CollieMollie.UI
{
    [Serializable]
    public class UITransformPreset
    {
        public Setting[] States = null;

        [Serializable]
        public struct Setting
        {
            public bool IsEnabled;
            public BaseUI.State ExecutionState;

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
