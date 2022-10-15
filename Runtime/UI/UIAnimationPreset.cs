using System.Collections;
using System.Collections.Generic;
using CollieMollie.Audio;
using CollieMollie.Core;
using UnityEngine;

namespace CollieMollie.UI
{
    [CreateAssetMenu(fileName = "UIAnimationPreset", menuName = "CollieMollie/UI/AnimationPreset")]
    public class UIAnimationPreset : ScriptableObject
    {
        public AnimatorOverrideController OverrideAnimator = null;
        public AnimationState[] AnimationStates = null;

        [System.Serializable]
        public struct AnimationState
        {
            public AnimationClip Animation;
            public InteractionState ExecutionState;
            public bool IsEnabled;

            public bool IsValid() => ExecutionState != InteractionState.None;
        }
    }
}
