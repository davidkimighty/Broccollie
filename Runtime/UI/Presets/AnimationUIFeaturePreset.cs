using System;
using UnityEngine;

namespace Broccollie.UI
{
    [CreateAssetMenu(fileName = "AnimationPreset", menuName = "Broccollie/UI/Preset/Animation")]
    public class AnimationUIFeaturePreset : ScriptableObject
    {
        public AnimatorOverrideController OverrideAnimator;
        public Setting[] Settings;

        [Serializable]
        public struct Setting
        {
            public bool IsEnabled;
            public UIStates ExecutionState;
            public AnimationClip Animation;
        }
    }
}
