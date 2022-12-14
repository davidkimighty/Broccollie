using System;
using System.Collections;
using System.Collections.Generic;
using CollieMollie.Audio;
using CollieMollie.Core;
using UnityEngine;

namespace CollieMollie.UI
{
    [CreateAssetMenu(fileName = "UIAudioPreset", menuName = "CollieMollie/UI/AudioPreset")]
    public class UIAudioPreset : ScriptableObject
    {
        public Setting[] States = null;

        [Serializable]
        public struct Setting
        {
            public bool IsEnabled;
            public AudioPreset AudioPreset;
            public BaseUI.State ExecutionState;
        }
    }
}
