using System.Collections;
using System.Collections.Generic;
using CollieMollie.Audio;
using UnityEngine;

namespace CollieMollie.UI
{
    [CreateAssetMenu(fileName = "UIAudioPreset", menuName = "CollieMollie/UI/AudioPreset")]
    public class UIAudioPreset : ScriptableObject
    {
        public AudioState[] audioStates = null;

        [System.Serializable]
        public struct AudioState
        {
            public AudioPreset audioPreset;
            public ButtonState executionState;
            public bool isEnabled;
        }
    }
}
