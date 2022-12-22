using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Pool;

namespace CollieMollie.Audio
{
    public class AudioPlayer : MonoBehaviour
    {
        #region Variable Field
        [SerializeField] private AudioSource _source = null;

        private AudioPreset _injectedPreset = null;
        public AudioPreset InjectedPreset
        {
            get => _injectedPreset;
        }

        private IObjectPool<AudioPlayer> _pool = null;
        private CancellationTokenSource _cts = new CancellationTokenSource();

        #endregion

        #region Public Functions
        public void Init(IObjectPool<AudioPlayer> pool, AudioPreset preset)
        {
            _pool = pool;
            _injectedPreset = preset;
        }

        public void Play(AudioData data)
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

            Task audioPlay = PlayAudioSourceAsync(data.Loop);
        }

        public void Stop()
        {
            if (_cts != null)
                _cts.Cancel();
            _source.Stop();
        }

        #endregion

        #region AudioPlayer Features
        private async Task PlayAudioSourceAsync(bool loop)
        {
            try
            {
                _source.Play();
                while (_source.isPlaying)
                {
                    if (_cts != null)
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
