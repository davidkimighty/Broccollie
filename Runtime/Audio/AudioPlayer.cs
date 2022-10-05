using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Pool;

namespace CollieMollie.Audio
{
    public class AudioPlayer : MonoBehaviour
    {
        #region Variable Field
        public IObjectPool<AudioPlayer> Pool = null;

        [SerializeField] private AudioSource _source = null;

        private Data _data;
        private IEnumerator _audioPlayAction = null;
        #endregion

        #region Public Functions
        public void Play(Data data)
        {
            _source.clip = data.Clip;
            _source.outputAudioMixerGroup = data.Group;
            _source.loop = data.Loop;

            if (_audioPlayAction != null)
                StopCoroutine(_audioPlayAction);

            _audioPlayAction = PlayAudioSource(data.Loop);
            StartCoroutine(_audioPlayAction);
        }

        #endregion

        #region AudioPlayer Features
        private IEnumerator PlayAudioSource(bool loop)
        {
            _source.Play();
            while (_source.isPlaying)
                yield return null;

            if (!loop)
                Pool.Release(this);
        }
        #endregion

        public struct Data
        {
            public AudioClip Clip;
            public AudioMixerGroup Group;
            public bool Loop;
        }
    }
}
