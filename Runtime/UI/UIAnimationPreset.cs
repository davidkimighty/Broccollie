using System;
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
        public Setting[] States = null;

        [Serializable]
        public struct Setting
        {
            public bool IsEnabled;
            public AnimationClip Animation;
            public BaseUI.State ExecutionState;
        }
    }
}
