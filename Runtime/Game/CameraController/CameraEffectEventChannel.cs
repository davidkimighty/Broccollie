using System;
using UnityEngine;

namespace CollieMollie.Game
{
    [CreateAssetMenu(fileName = "EventChannel_CameraEffect", menuName = "CollieMollie/Event Channels/Camera Effect")]
    public class CameraEffectEventChannel : ScriptableObject
    {
        #region Events
        public event Action<CameraEffect> OnPlayCameraEffectRequest = null;

        #endregion

        #region Publishers
        public void RaisePlayCameraEffectEvent(CameraEffect effect)
        {
            if (effect == null) return;

            OnPlayCameraEffectRequest?.Invoke(effect);
        }

        #endregion
    }
}
