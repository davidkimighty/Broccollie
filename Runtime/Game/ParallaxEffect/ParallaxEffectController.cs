using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace CollieMollie.Game
{
    public class ParallaxEffectController : MonoBehaviour
    {
        #region Variable Field
        public event Action OnBeginParallax = null;
        public event Action OnEndParallax = null;

        [SerializeField] private float _fixedParallaxSpeed = 3f;
        [SerializeField] private float _minParallaxSpeed = 3f;
        [SerializeField] private float _maxParallaxSpeed = 8f;
        [SerializeField] private BackgroundLayer[] _layers = null;
        [SerializeField] private Material _material = null;
        [SerializeField] private AnimationCurve _parallaxCurve = null;

        private Task _parallaxTask = null;
        private CancellationTokenSource _tokenSource = new CancellationTokenSource();

        #endregion

        private void Awake()
        {
            for (int i = 0; i < _layers.Length; i++)
                _layers[i].Background.material = new Material(_material);

        }

        #region Public Functions
        public void StartParallax(int dir, float duration)
        {
            StopParallax();
            _tokenSource = new CancellationTokenSource();
            _parallaxTask = Parallax(dir, duration, _parallaxCurve);
        }

        public void StartParallaxLoop(int dir)
        {
            StopParallax();
            _tokenSource = new CancellationTokenSource();
            _parallaxTask = ParallaxLoop(dir);
        }

        public void StopParallax()
        {
            _tokenSource.Cancel();
        }

        #endregion

        #region Private Functions
        private async Task Parallax(int dir, float duration, AnimationCurve curve)
        {
            OnBeginParallax?.Invoke();

            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                _tokenSource.Token.ThrowIfCancellationRequested();
                float lerpSpeed = Mathf.Lerp(_minParallaxSpeed, _maxParallaxSpeed, curve.Evaluate(elapsedTime / duration));
                for (int i = 0; i < _layers.Length; i++)
                {
                    Vector2 offset = _layers[i].Background.materialForRendering.mainTextureOffset;
                    offset.x += dir * (lerpSpeed * Time.deltaTime) * _layers[i].ParallaxValue;
                    _layers[i].Background.materialForRendering.mainTextureOffset = offset;
                }
                elapsedTime += Time.deltaTime;
                await Task.Yield();
            }
            OnEndParallax?.Invoke();
        }

        private async Task ParallaxLoop(int dir)
        {
            while (true)
            {
                _tokenSource.Token.ThrowIfCancellationRequested();
                for (int i = 0; i < _layers.Length; i++)
                {

                    Vector2 offset = _layers[i].Background.materialForRendering.mainTextureOffset;
                    offset.x += dir * (_fixedParallaxSpeed * Time.deltaTime) * _layers[i].ParallaxValue;
                    _layers[i].Background.materialForRendering.mainTextureOffset = offset;
                }
                await Task.Yield();
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
