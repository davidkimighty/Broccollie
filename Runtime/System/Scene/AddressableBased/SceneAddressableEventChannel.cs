using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CollieMollie.System
{
    [CreateAssetMenu(fileName = "EventChannel_SceneAddressable", menuName = "CollieMollie/Event Channels/Scene Addressable")]
    public class SceneAddressableEventChannel : ScriptableObject
    {
        #region Events
        public event Action<SceneAddressablePreset, bool> OnSceneLoadRequest = null;

        #endregion

        #region Publishers
        public void RaiseSceneLoadEvent(SceneAddressablePreset targetScene, bool showLoadingScreen)
        {
            if (targetScene == null) return;

            OnSceneLoadRequest?.Invoke(targetScene, showLoadingScreen);
        }
        #endregion
    }
}
