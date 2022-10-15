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
        public AudioState[] AudioStates = null;

        [System.Serializable]
        public struct AudioState
        {
            public AudioPreset AudioPreset;
            public InteractionState ExecutionState;
            public bool IsEnabled;

            public bool IsValid() => ExecutionState != InteractionState.None;
        }
    }
}
