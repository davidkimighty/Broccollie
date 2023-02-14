using System;
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
        public void RaiseSceneLoadEvent(SceneAddressablePreset scene, bool showLoadingScene)
        {
            if (scene == null) return;
            OnSceneLoadRequest?.Invoke(scene, showLoadingScene);
        }

        #endregion
    }
}
