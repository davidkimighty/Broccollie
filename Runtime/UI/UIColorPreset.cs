using UnityEngine;

namespace CollieMollie.UI
{
    [CreateAssetMenu(fileName = "UIColorPreset", menuName = "CollieMollie/UI/ColorPreset")]
    public class UIColorPreset : ScriptableObject
    {
        public ColorState[] colorStates = null;

        [System.Serializable]
        public struct ColorState
        {
            public Color targetColor;
            public ButtonState executionState;
            public bool isEnabled;
            public float duration;
            public AnimationCurve curve;
        }
    }
}
