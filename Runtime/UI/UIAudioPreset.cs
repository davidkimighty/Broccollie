using System;
using System.Collections;
using System.Collections.Generic;
using CollieMollie.Audio;
using CollieMollie.Core;
using UnityEngine;

namespace CollieMollie.UI
{
    [CreateAssetMenu(fileName = "UIAudioPreset", menuName = "CollieMollie/UI/AudioPreset")]
    public class UIAudioPreset : ScriptableObject, IUIPreset
    {
        public Setting[] States = null;

        public float GetDuration(string state)
        {
            Setting setting = Array.Find(States, x => x.ExecutionState.ToString() == state);
            if (IsValid(setting.ExecutionState) && setting.AudioPreset != null)
                return setting.AudioPreset.AudioClips.Length;
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
            public AudioPreset AudioPreset;
            public UIAllState ExecutionState;
        }
    }
}
