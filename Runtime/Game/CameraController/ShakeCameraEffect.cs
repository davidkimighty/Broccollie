using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

namespace CollieMollie.Game
{
    [CreateAssetMenu(fileName = "CameraEffect_Shake", menuName = "CollieMollie/Camera Effect/Shake")]
    public class ShakeCameraEffect : CameraEffect
    {
        #region Variable Field
        [SerializeField] private float _strength = 2f;
        [SerializeField] private float _duration = 1f;
        [SerializeField] private AnimationCurve _curve = null;

        private IEnumerator _shakeAction = null;
        #endregion

        public override void Play(MonoBehaviour mono, Transform camera)
        {
            if (_shakeAction != null)
                mono.StopCoroutine(_shakeAction);

            _shakeAction = Shake(camera);
            mono.StartCoroutine(_shakeAction);
        }

        #region
        private IEnumerator Shake(Transform camera)
        {
            float elapsedTime = 0f;
            Vector3 originalPosition = camera.position;

            while (elapsedTime < _duration)
            {
                camera.position = originalPosition + Random.insideUnitSphere * _strength
                    * _curve.Evaluate(elapsedTime / _duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            camera.position = originalPosition;
        }

        #endregion
    }
}
