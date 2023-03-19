using System;
using Broccollie.Audio;
using UnityEngine;

namespace Broccollie.UI
{
    [CreateAssetMenu(fileName = "AudioPreset", menuName = "Broccollie/UI/Preset/Audio")]
    public class UIAudioPreset : UIBasePreset
    {
        public AudioSetting[] Settings = null;

        [Serializable]
        public class AudioSetting : Setting
        {
            public AudioPreset Audio;
            public float Duration;
        }
    }
}
