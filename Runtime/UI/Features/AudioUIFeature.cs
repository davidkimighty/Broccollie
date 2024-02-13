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
        [SerializeField] private AudioEventChannel _eventChannel = null;
        [SerializeField] private Element[] _elements = null;

        #region Public Functions
        public override List<Task> GetFeatures(UIStates state, bool instantChange, bool playAudio, CancellationToken ct)
        {
            try
            {
                if (!playAudio || _elements == null) return default;

                List<Task> features = new();
                for (int i = 0; i < _elements.Length; i++)
                {
                    ct.ThrowIfCancellationRequested();
                    if (!_elements[i].IsEnabled || _elements[i].Preset == null) continue;

                    AudioUIFeaturePreset.Setting setting = Array.Find(_elements[i].Preset.Settings, x => x.ExecutionState == state);
                    if (!setting.IsEnabled) continue;

                    if (instantChange)
                        features.Add(PlayAudioInstantAsync(setting));
                    else
                        features.Add(PlayAudioAsync(setting, ct));
                }
                return features;
            }
            catch (OperationCanceledException)
            {
                return default;
            }
        }
        #endregion

        private async Task PlayAudioAsync(AudioUIFeaturePreset.Setting setting, CancellationToken ct)
        {
            await Task.Delay((int)(setting.Delay * 1000f), ct);
            _eventChannel.RequestPlayAudio(setting.Audio);
        }

        private async Task PlayAudioInstantAsync(AudioUIFeaturePreset.Setting setting)
        {
            _eventChannel.RequestPlayAudio(setting.Audio);
            await Task.Yield();
        }

        [Serializable]
        public struct Element
        {
            public bool IsEnabled;
            public AudioUIFeaturePreset Preset;
        }
    }
}
