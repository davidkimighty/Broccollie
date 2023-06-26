using System;
using Broccollie.Audio;
using UnityEngine;

namespace Broccollie.UI
{
    [CreateAssetMenu(fileName = "AudioPreset", menuName = "Broccollie/UI/Preset/Audio")]
    public class AudioUIPreset : BaseUIPreset
    {
        public AudioSetting[] Settings = null;

        [Serializable]
        public class AudioSetting : Setting
        {
            public AudioPreset Audio;
            public float Duration;
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
