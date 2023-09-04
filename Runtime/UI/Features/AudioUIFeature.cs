using System;
using Broccollie.Audio;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Threading;

namespace Broccollie.UI
{
    public class AudioUIFeature : BaseUIFeature
    {
        [Header("Audio Feature")]
        [SerializeField] private AudioEventChannel _eventChannel = null;
        [SerializeField] private Element[] _elements = null;

        protected override List<Task> GetFeatures(string state, CancellationToken ct)
        {
            List<Task> features = new List<Task>();
            if (_elements == null) return features;

            for (int i = 0; i < _elements.Length; i++)
            {
                if (!_elements[i].IsEnabled) continue;

                AudioUIPreset.AudioSetting setting = Array.Find(_elements[i].Preset.Settings, x => x.ExecutionState == state);
                if (setting == null || !setting.IsEnabled) continue;

                features.Add(PlayAudioAsync(setting, ct));
            }
            return features;
        }

        private async Task PlayAudioAsync(AudioUIPreset.AudioSetting setting, CancellationToken ct)
        {
            _eventChannel.RequestPlayAudio(setting.Audio);
            await Task.Delay((int)(setting.Duration * 1000f), ct);
        }

        [Serializable]
        public class Element
        {
            public bool IsEnabled;
            public AudioUIPreset Preset;
        }
    }
}
