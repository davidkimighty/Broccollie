using System;
using System.Collections;
using System.Collections.Generic;
using CollieMollie.Audio;
using UnityEngine;

namespace CollieMollie.UI
{
    public class UIAudioFeature : MonoBehaviour
    {
        #region Variable Field
        [SerializeField] private AudioEventChannel _eventChannel = null;
        [SerializeField] private bool _isEnabled = true;
        [SerializeField] private List<Element> _elements = null;
        #endregion

        #region Public Functions
        public void Play(ButtonState state)
        {
            if (!_isEnabled) return;

            foreach (Element element in _elements)
            {
                if (!element.IsEnabled) continue;
                element.PlayAudio(this, state, _eventChannel);
            }
        }

        #endregion

        [Serializable]
        public class Element
        {
            #region Variabled Field
            public bool IsEnabled = true;
            public UIAudioPreset Preset = null;

            private IEnumerator _audioPlayAction = null;
            #endregion

            #region Public Functions
            public void PlayAudio(MonoBehaviour mono, ButtonState state, AudioEventChannel eventChannel)
            {
                if (_audioPlayAction != null)
                    mono.StopCoroutine(_audioPlayAction);

                UIAudioPreset.AudioState? audioState = Array.Find(Preset.AudioStates, x => x.ExecutionState == state);
                if (audioState == null)
                    audioState = Array.Find(Preset.AudioStates, x => x.ExecutionState == ButtonState.Default);

                if (audioState != null)
                {
                    if (!audioState.Value.IsEnabled) return;

                    eventChannel.RaisePlayAudioEvent(audioState.Value.AudioPreset);
                }
            }
            #endregion
        }
    }
}
