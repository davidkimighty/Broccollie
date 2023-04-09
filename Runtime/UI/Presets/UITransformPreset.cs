using System;
using UnityEngine;

namespace Broccollie.UI
{
    [CreateAssetMenu(fileName = "TransformPreset", menuName = "Broccollie/UI/Preset/Transform")]
    public class UITransformPreset : UIBasePreset
    {
        public TransformSetting[] Settings = null;

        [Serializable]
        public class TransformSetting : Setting
        {
            [Header("Position")]
            public bool IsPositionEnabled;
            public float TargetPosition;
            public float PositionDuration;
            public AnimationCurve PositionCurve;

            [Header("Scale")]
            public bool IsScaleEnabled;
            public float TargetScale;
            public float ScaleDuration;
            public AnimationCurve ScaleCurve;
        }
    }
}
