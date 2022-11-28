using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CollieMollie.Game
{
    public class ParallaxController : MonoBehaviour
    {
        #region Variable Field
        [SerializeField] private float _fixedParallaxSpeed = 3f;
        [SerializeField] private float _minParallaxSpeed = 3f;
        [SerializeField] private float _maxParallaxSpeed = 8f;
        [SerializeField] private BackgroundLayer[] _layers = null;
        [SerializeField] private Material _material = null;
        [SerializeField] private AnimationCurve _parallaxCurve = null;

        private IEnumerator _parallaxAction = null;

        #endregion

        private void Awake()
        {
            for (int i = 0; i < _layers.Length; i++)
                _layers[i].Background.material = new Material(_material);

        }

        #region Public Functions
        public void StartParallax(int dir, float duration, Action done = null)
        {
            if (_parallaxAction != null)
                StopCoroutine(_parallaxAction);

            _parallaxAction = Parallax(dir, duration, _parallaxCurve, done);
            StartCoroutine(_parallaxAction);
        }

        public void StartParallaxLoop(int dir)
        {
            if (_parallaxAction != null)
                StopCoroutine(_parallaxAction);

            _parallaxAction = ParallaxLoop(dir);
            StartCoroutine(_parallaxAction);
        }

        public void StopParallax()
        {
            if (_parallaxAction != null)
                StopCoroutine(_parallaxAction);
        }

        #endregion

        #region Private Functions
        private IEnumerator Parallax(int dir, float duration, AnimationCurve curve, Action done = null)
        {
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                float lerpSpeed = Mathf.Lerp(_minParallaxSpeed, _maxParallaxSpeed, curve.Evaluate(elapsedTime / duration));
                for (int i = 0; i < _layers.Length; i++)
                {
                    Vector2 offset = _layers[i].Background.materialForRendering.mainTextureOffset;
                    offset.x += dir * (lerpSpeed * Time.deltaTime) * _layers[i].ParallaxValue;
                    _layers[i].Background.materialForRendering.mainTextureOffset = offset;
                }
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            done?.Invoke();
        }

        private IEnumerator ParallaxLoop(int dir)
        {
            while (true)
            {
                for (int i = 0; i < _layers.Length; i++)
                {
                    Vector2 offset = _layers[i].Background.materialForRendering.mainTextureOffset;
                    offset.x += dir * (_fixedParallaxSpeed * Time.deltaTime) * _layers[i].ParallaxValue;
                    _layers[i].Background.materialForRendering.mainTextureOffset = offset;
                }
                yield return null;
            }
        }
        #endregion
    }

    [Serializable]
    public class BackgroundLayer
    {
        public Image Background = null;
        public float ParallaxValue = 0.1f;
    }
}
