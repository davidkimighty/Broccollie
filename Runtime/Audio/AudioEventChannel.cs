using System;
using UnityEngine;

namespace Broccollie.Audio
{
    [CreateAssetMenu(fileName = "EventChannel_Audio", menuName = "Broccollie/EventChannels/Audio")]
    public class AudioEventChannel : ScriptableObject
    {
        public event Action<AudioPreset> OnPlayAudio = null;
        public event Action<AudioPreset> OnPauseAudio = null;
        public event Action<AudioPreset> OnStopAudio = null;

        #region Publishers
        public void RequestPlayAudio(AudioPreset audio)
        {
            if (audio == null) return;

            OnPlayAudio?.Invoke(audio);
        }

        public void RequestPauseAudio(AudioPreset audio)
        {
            if (audio == null) return;

            OnPauseAudio?.Invoke(audio);
        }

        public void RequestStopAudio(AudioPreset audio)
        {
            if (audio == null) return;

            OnStopAudio?.Invoke(audio);
        }

        #endregion
    }
}
