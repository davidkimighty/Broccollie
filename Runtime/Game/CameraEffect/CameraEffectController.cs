using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CollieMollie.Game
{
    public class CameraEffectController : MonoBehaviour
    {
        #region Variable Field
        [SerializeField] private CameraEffectEventChannel _eventChannel = null;
        [SerializeField] private Camera _mainCam = null;

        #endregion

        private void Awake()
        {
            _eventChannel.OnPlayCameraEffectRequest += PlayCameraEffect;
        }

        #region Subscribers
        private void PlayCameraEffect(CameraEffect effect)
        {
            if (effect == null) return;

            effect.Play(this, _mainCam.transform);
        }

        #endregion
    }
}
