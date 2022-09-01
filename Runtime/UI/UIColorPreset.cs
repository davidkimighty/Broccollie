using UnityEngine;

namespace CollieMollie.UI
{
    [CreateAssetMenu(fileName = "UIColorPreset", menuName = "CollieMollie/UI/ColorPreset")]
    public class UIColorPreset : ScriptableObject
    {
        public ColorState[] colorStates = null;

        [System.Serializable]
        public class ColorState
        {
            public Color targetColor = Color.white;
            public ButtonState executionState = ButtonState.Default;
            public bool isEnabled = true;
            public float duration = 1f;
            public AnimationCurve curve = null;
        }
    }
}
