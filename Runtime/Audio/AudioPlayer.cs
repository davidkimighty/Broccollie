using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Pool;

namespace Broccollie.Audio
{
    public class AudioPlayer : MonoBehaviour
    {
        [SerializeField] private AudioSource _source = null;

        private AudioPreset _injectedPreset = null;
        public AudioPreset InjectedPreset
        {
            get => _injectedPreset;
        }

        private IObjectPool<AudioPlayer> _pool = null;
        private CancellationTokenSource _cts = new CancellationTokenSource();
        private bool _paused = false;

        #region Public Functions
        public void Init(IObjectPool<AudioPlayer> pool, AudioPreset preset)
        {
            _pool = pool;
            _injectedPreset = preset;
        }

        public void Play(AudioData data)
        {
            if (!_paused)
            {
                _source.clip = data.Clip;
                _source.outputAudioMixerGroup = data.Group;
                _source.volume = data.Volume;
                _source.loop = data.Loop;

                if (_cts != null)
                {
                    _cts.Cancel();
                    _cts = new CancellationTokenSource();
                }
            }
            _paused = false;
            PlayAudioSourceAsync(data.Loop);
        }

        public void Play()
        {
            if (_injectedPreset == null) return;
            _paused = false;
            PlayAudioSourceAsync(_injectedPreset.Loop);
        }

        public void Pause()
        {
            if (_cts != null)
                _cts.Cancel();
            _paused = true;
            _source.Pause();
        }

        public void Stop()
        {
            if (_cts != null)
                _cts.Cancel();
            _source.Stop();
        }

        #endregion

        #region AudioPlayer Features
        private async void PlayAudioSourceAsync(bool loop)
        {
            try
            {
                _source.Play();
                while (_source.isPlaying)
                {
                    if (_cts.IsCancellationRequested)
                        _cts.Token.ThrowIfCancellationRequested();
                    await Task.Yield();
                }
            }
            finally
            {
                if (!loop)
                    _pool.Release(this);
            }
        }

        #endregion

        public struct AudioData
        {
            public AudioClip Clip;
            public AudioMixerGroup Group;
            public float Volume;
            public bool Loop;
        }
    }
}
