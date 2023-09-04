
using Broccollie.Core;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Broccollie.Game.CameraEffect
{
    public class CameraEffectController : MonoBehaviour
    {
        #region Variable Field
        [SerializeField] private CameraEffectEventChannel _eventChannel = null;
        [SerializeField] private Camera _mainCam = null;
        [SerializeField] private Volume _volume = null;

        private DepthOfField _dof = null;

        #endregion

        private void Awake()
        {
            _ = _volume.profile.TryGet(out _dof);
        }

        private void OnEnable()
        {
            _eventChannel.OnPlayCameraEffect += PlayCameraEffect;
        }

        private void OnDisable()
        {
            _eventChannel.OnPlayCameraEffect -= PlayCameraEffect;
        }

        #region Subscribers
        private void PlayCameraEffect(CameraEffectPreset effect)
        {
            if (effect == null)
            {
                Helper.Log("Camera effect is null.", Helper.Broccollie, this);
                return;
            }

            effect.Play(this, _mainCam.transform);
        }

        #endregion
    }
}
