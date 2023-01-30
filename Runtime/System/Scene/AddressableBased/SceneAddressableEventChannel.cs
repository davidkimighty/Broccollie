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
        public event Action OnBeforeSceneUnload = null;
        public event Action OnAfterSceneLoad = null;

        public event Action<SceneAddressablePreset, bool, float> OnSceneLoadRequest = null;

        #endregion

        #region Publishers
        public void InvokeBeforeSceneUnload()
        {
            OnBeforeSceneUnload?.Invoke();
        }

        public void InvokeAfterSceneLoad()
        {
            OnAfterSceneLoad?.Invoke();
        }

        public void RaiseSceneLoadEvent(SceneAddressablePreset targetScene, bool showLoadingScreen, float loadingScreenDuration = 1f)
        {
            if (targetScene == null) return;

            OnSceneLoadRequest?.Invoke(targetScene, showLoadingScreen, loadingScreenDuration);
        }

        #endregion
    }
}
