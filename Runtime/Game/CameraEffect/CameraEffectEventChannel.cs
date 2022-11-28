using System;
using System.Collections;
using System.Collections.Generic;
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
