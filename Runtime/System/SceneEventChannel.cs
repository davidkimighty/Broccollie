using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CollieMollie.System
{
    [CreateAssetMenu(fileName = "EventChannel_Scene", menuName = "CollieMollie/System/SceneEventChannel")]
    public class SceneEventChannel : ScriptableObject
    {
        #region Events
        public event Action<ScenePreset, bool> OnSceneLoadRequest = null;

        #endregion

        #region Publishers
        public void RaiseSceneLoadEvent(ScenePreset targetScene, bool showLoadingScreen)
        {
            if (targetScene == null) return;

            OnSceneLoadRequest?.Invoke(targetScene, showLoadingScreen);
        }
        #endregion
    }
}
