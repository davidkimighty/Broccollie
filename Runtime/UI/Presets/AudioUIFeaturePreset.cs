using System;
using Broccollie.Audio;
using Broccollie.UI;
using UnityEngine;

namespace Broccollie.UI
{
    [CreateAssetMenu(fileName = "AudioPreset", menuName = "Broccollie/UI/Preset/Audio")]
    public class AudioUIFeaturePreset : ScriptableObject
    {
        public Setting[] Settings;

        [Serializable]
        public struct Setting
        {
            public bool IsEnabled;
            public UIStates ExecutionState;
            public AudioPreset Audio;
            public float Delay;
        }
    }
}
