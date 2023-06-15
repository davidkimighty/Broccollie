using System;
using UnityEngine;

namespace Broccollie.UI
{
    [CreateAssetMenu(fileName = "TransformPreset", menuName = "Broccollie/UI/Preset/Transform")]
    public class TransformUIPreset : BaseUIPreset
    {
        public TransformSetting[] Settings = null;

        [Serializable]
        public class TransformSetting : Setting
        {
            [Header("Position")]
            public bool IsPositionEnabled;
            public Vector3 AnchoredPosition;
            public float PositionDuration;
            public AnimationCurve PositionCurve;

            [Header("Rotation")]
            public bool IsRotationEnabled;
            public Vector3 TargetRotation;
            public float RotationDuration;
            public AnimationCurve RotationCurve;

            [Header("Scale")]
            public bool IsScaleEnabled;
            public Vector3 TargetScale;
            public float ScaleDuration;
            public AnimationCurve ScaleCurve;
        }
    }
}
