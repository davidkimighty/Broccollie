using System;
using UnityEngine;

namespace Broccollie.UI
{
    [CreateAssetMenu(fileName = "TransformPreset", menuName = "Broccollie/UI/Preset/Transform")]
    public class TransformUIFeaturePreset : ScriptableObject
    {
        public Setting[] Settings;

        [Serializable]
        public struct Setting
        {
            public UIStates ExecutionState;

            [Header("Position")]
            public bool IsPositionEnabled;
            public Vector3 TargetPosition;
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
