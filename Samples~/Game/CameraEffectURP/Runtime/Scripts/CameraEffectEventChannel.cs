using System;
using UnityEngine;

namespace Broccollie.Game.CameraEffect
{
    [CreateAssetMenu(fileName = "EventChannel_CameraEffect", menuName = "Broccollie/EventChannels/CameraEffect")]
    public class CameraEffectEventChannel : ScriptableObject
    {
        public event Action<CameraEffectPreset> OnPlayCameraEffect = null;

        #region Publishers
        public void RequestPlayCameraEffectEvent(CameraEffectPreset effect)
        {
            if (effect == null) return;

            OnPlayCameraEffect?.Invoke(effect);
        }

        #endregion
    }
}
