using UnityEngine;
using UnityEngine.Audio;

namespace Broccollie.Audio
{
    [CreateAssetMenu(fileName = "AudioPreset", menuName = "Broccollie/Audio/Preset/Audio")]
    public class AudioPreset : ScriptableObject
    {
        public AudioClip[] AudioClips = null;
        public AudioMixerGroup MixerGroup = null;
        [Range(0f, 1f)] public float Volume = 1f;
        public float Delay = 0f;
        public bool Loop = false;

        [SerializeField] private int _playIndex = 0;
        [SerializeField] private AudioPlayOrder _playOrder = AudioPlayOrder.Random;

        #region Public Functions
        public AudioClip GetAudioClip()
        {
            if (AudioClips == null) return null;

            switch (_playOrder)
            {
                case AudioPlayOrder.Random:
                    _playIndex = Random.Range(0, AudioClips.Length - 1);
                    break;

                case AudioPlayOrder.InOrder:
                    int nextIndex = _playIndex + 1;
                    _playIndex = nextIndex > AudioClips.Length - 1 ? 0 : nextIndex;
                    break;

                case AudioPlayOrder.Reverse:
                    int previousIndex = _playIndex - 1;
                    _playIndex = previousIndex < 0 ? AudioClips.Length - 1 : previousIndex;
                    break;
            }
            return AudioClips[_playIndex];
        }
        #endregion
    }

    public enum AudioPlayOrder { Random, InOrder, Reverse }
}
