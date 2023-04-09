using System;
using Broccollie.Audio;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

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
        protected override List<IEnumerator> GetFeatures(UIStates state)
        {
            List<IEnumerator> features = new List<IEnumerator>();
            for (int i = 0; i < _elements.Length; i++)
            {
                if (!_elements[i].IsEnabled) continue;

                UIAudioPreset.AudioSetting setting = Array.Find(_elements[i].Preset.Settings, x => x.ExecutionState == state);
                if (setting == null || !setting.IsEnabled) continue;

                features.Add(PlayAudio(setting));
            }
            return features;
        }

        #endregion

        #region Private Functions
        private IEnumerator PlayAudio(UIAudioPreset.AudioSetting setting)
        {
            _eventChannel.RaisePlayAudioEvent(setting.Audio);
            yield return new WaitForSeconds(setting.Duration);
        }

        #endregion

        [Serializable]
        public struct Element
        {
            public bool IsEnabled;
            public UIAudioPreset Preset;
        }
    }
}
