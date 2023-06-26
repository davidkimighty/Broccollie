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
