using System;
using UnityEngine;

namespace Broccollie.UI
{
    [CreateAssetMenu(fileName = "AnimationPreset", menuName = "Broccollie/UI/Preset/Animation")]
    public class AnimationUIPreset : BaseUIPreset
    {
        public AnimatorOverrideController OverrideAnimator = null;
        public AnimationSetting[] Settings = null;

        [Serializable]
        public class AnimationSetting : Setting
        {
            public AnimationClip Animation;
        }
    }
}
