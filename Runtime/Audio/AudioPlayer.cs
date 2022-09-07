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
            _source.clip = data.clip;
            _source.outputAudioMixerGroup = data.group;

            if (_audioPlayAction != null)
                StopCoroutine(_audioPlayAction);

            _audioPlayAction = PlayAudioSource();
            StartCoroutine(_audioPlayAction);
        }

        #endregion

        #region AudioPlayer Features
        private IEnumerator PlayAudioSource()
        {
            _source.Play();
            while (_source.isPlaying)
                yield return null;
            Pool.Release(this);
        }
        #endregion

        public struct Data
        {
            public AudioClip clip;
            public AudioMixerGroup group;
        }
    }
}
