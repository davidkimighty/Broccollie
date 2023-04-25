using System;
using Broccollie.Audio;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Threading.Tasks;
using System.Threading;

namespace Broccollie.UI
{
    public class UIAudioFeature : UIBaseFeature
    {
        #region Variable Field
        [Header("Audio Feature")]
        [SerializeField] private AudioEventChannel _eventChannel = null;
        [SerializeField] private Element[] _elements = null;

        #endregion

        #region Override Functions
        protected override List<Task> GetFeatures(UIStates state, CancellationToken ct)
        {
            List<Task> features = new List<Task>();
            if (_elements == null) return features;

            for (int i = 0; i < _elements.Length; i++)
            {
                if (!_elements[i].IsEnabled) continue;

                UIAudioPreset.AudioSetting setting = Array.Find(_elements[i].Preset.Settings, x => x.ExecutionState == state);
                if (setting == null || !setting.IsEnabled) continue;

                features.Add(PlayAudioAsync(setting, ct));
            }
            return features;
        }

        #endregion

        #region Private Functions
        private async Task PlayAudioAsync(UIAudioPreset.AudioSetting setting, CancellationToken ct)
        {
            _eventChannel.RaisePlayAudioEvent(setting.Audio);
            await Task.Delay((int)(setting.Duration * 1000f), ct);
        }

        #endregion

        [Serializable]
        public class Element
        {
            public bool IsEnabled;
            public UIAudioPreset Preset;
        }
    }
}
