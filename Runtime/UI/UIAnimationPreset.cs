using System;
using System.Collections;
using System.Collections.Generic;
using CollieMollie.Audio;
using CollieMollie.Core;
using UnityEngine;

namespace CollieMollie.UI
{
    [CreateAssetMenu(fileName = "UIAnimationPreset", menuName = "CollieMollie/UI/AnimationPreset")]
    public class UIAnimationPreset : ScriptableObject, IUIPreset
    {
        public AnimatorOverrideController OverrideAnimator = null;
        public Setting[] States = null;

        public float GetDuration(string state)
        {
            Setting setting = Array.Find(States, x => x.ExecutionState.ToString() == state);
            if (IsValid(setting.ExecutionState) && setting.Animation != null)
                return setting.Animation.length;
            return 0;
        }

        public bool IsValid(UIAllState state)
        {
            return state != UIAllState.None;
        }

        [Serializable]
        public struct Setting
        {
            public bool IsEnabled;
            public AnimationClip Animation;
            public UIAllState ExecutionState;
        }
    }
}
