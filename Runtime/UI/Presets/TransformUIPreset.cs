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

#if UNITY_EDITOR
        private void OnValidate()
        {
            for (int i = 0; i < Settings.Length; i++)
            {
                if (Settings[i].ExecutionState == null || Settings[i].ExecutionState == string.Empty)
                    Settings[i].ExecutionState = Settings[i].ExecutionStateHelper.ToString();
            }
        }
#endif
    }
}
