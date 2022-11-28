using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CollieMollie.Audio
{
    [CreateAssetMenu(fileName = "EventChannel_Audio", menuName = "CollieMollie/Event Channels/Audio")]
    public class AudioEventChannel : ScriptableObject
    {
        #region Events
        public event Action<AudioPreset> OnPlayAudioRequest = null;

        #endregion

        #region Publishers
        public void RaisePlayAudioEvent(AudioPreset audio)
        {
            if (audio == null) return;

            OnPlayAudioRequest?.Invoke(audio);
        }
        #endregion
    }
}
