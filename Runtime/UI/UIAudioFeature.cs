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
        public void Play(InteractionState state)
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

            #region Features
            public void PlayAudio(MonoBehaviour mono, InteractionState state, AudioEventChannel eventChannel)
            {
                if (_audioPlayAction != null)
                    mono.StopCoroutine(_audioPlayAction);

                UIAudioPreset.AudioState audioState = Array.Find(Preset.AudioStates, x => x.ExecutionState == state);
                if (!audioState.IsValid())
                    audioState = Array.Find(Preset.AudioStates, x => x.ExecutionState == InteractionState.Default);

                if (audioState.IsValid())
                {
                    if (!audioState.IsEnabled) return;
                    eventChannel.RaisePlayAudioEvent(audioState.AudioPreset);
                }
            }
            #endregion
        }
    }
}
