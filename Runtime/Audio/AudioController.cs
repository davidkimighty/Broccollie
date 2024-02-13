using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Pool;

namespace Broccollie.Audio
{
    [DefaultExecutionOrder(-100)]
    public class AudioController : MonoBehaviour
    {
        [Header("Event")]
        [SerializeField] private AudioEventChannel _audioEventChannel = null;

        [Header("Mixer")]
        [SerializeField] private AudioMixer _mixer = null;

        [Header("Pool")]
        [SerializeField] private AudioPlayer _audioPlayerRef = null;
        [SerializeField] private Transform _poolHolder = null;
        [SerializeField] private IObjectPool<AudioPlayer> _pool = null;
        [SerializeField] private List<AudioPlayer> _activeAudio = new List<AudioPlayer>();

        private void Awake()
        {
            _pool = new ObjectPool<AudioPlayer>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject);
        }

        private void OnEnable()
        {
            _audioEventChannel.OnPlayAudio += PlayAudio;
            _audioEventChannel.OnPauseAudio += PauseAudio;
            _audioEventChannel.OnStopAudio += StopAudio;
        }

        private void OnDisable()
        {
            _audioEventChannel.OnPlayAudio -= PlayAudio;
            _audioEventChannel.OnPauseAudio -= PauseAudio;
            _audioEventChannel.OnStopAudio -= StopAudio;
        }

        #region Subscribers
        private void PlayAudio(AudioPreset preset)
        {
            AudioPlayer audioPlayer = _activeAudio.Find(a => a.InjectedPreset == preset);
            if (audioPlayer == null)
            {
                audioPlayer = _pool.Get();
                audioPlayer.Init(_pool, preset);
                AudioPlayer.AudioData audioPlayerData = new AudioPlayer.AudioData
                {
                    Clip = preset.GetAudioClip(),
                    Group = preset.MixerGroup,
                    Volume = preset.Volume,
                    Loop = preset.Loop
                };
                audioPlayer.Play(audioPlayerData);
            }
            else
            {
                audioPlayer.Play();
            }
        }

        private void PauseAudio(AudioPreset preset)
        {
            AudioPlayer audio = _activeAudio.Find(a => a.InjectedPreset == preset);
            if (audio == null) return;

            audio.Pause();
        }

        private void StopAudio(AudioPreset preset)
        {
            AudioPlayer audio = _activeAudio.Find(a => a.InjectedPreset == preset);
            if (audio == null) return;

            audio.Stop();
        }

        #endregion

        #region Pool
        private AudioPlayer CreatePooledItem()
        {
            AudioPlayer audioPlayer = Instantiate(_audioPlayerRef, _poolHolder);
            return audioPlayer;
        }

        private void OnTakeFromPool(AudioPlayer audioPlayer)
        {
            audioPlayer.gameObject.SetActive(true);
            if (!_activeAudio.Contains(audioPlayer))
                _activeAudio.Add(audioPlayer);
        }

        private void OnReturnedToPool(AudioPlayer audioPlayer)
        {
            audioPlayer.gameObject.SetActive(false);
            if (_activeAudio.Contains(audioPlayer))
                _activeAudio.Remove(audioPlayer);
        }

        private void OnDestroyPoolObject(AudioPlayer audioPlayer)
        {
            Destroy(audioPlayer.gameObject);
        }

        #endregion
    }
}
