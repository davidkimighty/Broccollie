using UnityEngine;

namespace CollieMollie.UI
{
    [CreateAssetMenu(fileName = "UIColorPreset", menuName = "CollieMollie/UI/ColorPreset")]
    public class UIColorPreset : ScriptableObject
    {
        public ColorState[] ColorStates = null;

        [System.Serializable]
        public struct ColorState
        {
            public Color TargetColor;
            public ButtonState ExecutionState;
            public bool IsEnabled;
            public float Duration;
            public AnimationCurve Curve;
        }
    }
}
