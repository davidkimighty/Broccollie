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

        private void OnEnable()
        {
            _eventChannel.OnPlayCameraEffectRequest += PlayCameraEffect;
        }

        private void OnDisable()
        {
            _eventChannel.OnPlayCameraEffectRequest -= PlayCameraEffect;
        }

        #region Subscribers
        private void PlayCameraEffect(CameraEffect effect)
        {
            if (effect == null)
            {
                Helper.Helper.Log("Camera effect is null.", Helper.Helper.Broccollie, this);
                return;
            }

            effect.Play(this, _mainCam.transform);
        }

        #endregion
    }
}
