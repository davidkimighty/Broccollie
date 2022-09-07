using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace CollieMollie.Audio
{
    [CreateAssetMenu(fileName = "AudioPreset", menuName = "CollieMollie/Audio/AudioPreset")]
    public class AudioPreset : ScriptableObject
    {
        #region Variable Field
        public AudioClip[] audioClips = null;
        public AudioMixerGroup mixerGroup = null;
        public Vector2 volume = new Vector2(0.5f, 0.5f);
        public Vector2 pitch = new Vector2(1f, 1f);
        public float delay = 0f;

        [SerializeField] private int _playIndex = 0;
        [SerializeField] private AudioPlayOrder _playOrder = AudioPlayOrder.Random;
        #endregion

        #region Public Functions
        public AudioClip GetAudioClip()
        {
            if (audioClips == null) return null;

            switch (_playOrder)
            {
                case AudioPlayOrder.Random:
                    _playIndex = Random.Range(0, audioClips.Length - 1);
                    break;

                case AudioPlayOrder.InOrder:
                    int nextIndex = _playIndex + 1;
                    _playIndex = nextIndex > audioClips.Length - 1 ? 0 : nextIndex;
                    break;

                case AudioPlayOrder.Reverse:
                    int previousIndex = _playIndex - 1;
                    _playIndex = previousIndex < 0 ? audioClips.Length - 1 : previousIndex;
                    break;
            }
            return audioClips[_playIndex];
        }
        #endregion
    }

    public enum AudioPlayOrder { Random, InOrder, Reverse }
}
