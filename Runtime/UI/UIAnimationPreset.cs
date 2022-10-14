using System.Collections;
using System.Collections.Generic;
using CollieMollie.Audio;
using UnityEngine;

namespace CollieMollie.UI
{
    [CreateAssetMenu(fileName = "UIAnimationPreset", menuName = "CollieMollie/UI/AnimationPreset")]
    public class UIAnimationPreset : ScriptableObject
    {
        public AnimationState[] AnimationStates = null;

        [System.Serializable]
        public struct AnimationState
        {
            public AnimationClip Animation;
            public ButtonState ExecutionState;
            public bool IsEnabled;

            public bool IsValid() => ExecutionState != ButtonState.None;
        }
    }
}
