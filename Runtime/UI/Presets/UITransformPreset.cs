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
            public float PositionDuration;
            public AnimationCurve PositionCurve;

            [Header("Rotation")]
            public bool IsRotationEnabled;
            public float RotationDuration;
            public AnimationCurve RotationCurve;

            [Header("Scale")]
            public bool IsScaleEnabled;
            public float ScaleDuration;
            public AnimationCurve ScaleCurve;
        }
    }
}
