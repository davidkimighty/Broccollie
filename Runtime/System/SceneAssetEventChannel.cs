using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CollieMollie.System
{
    [CreateAssetMenu(fileName = "EventChannel_SceneAsset", menuName = "CollieMollie/System/SceneAssetEventChannel")]
    public class SceneAssetEventChannel : ScriptableObject
    {
        #region Events
        public event Action<SceneAssetPreset, bool> OnSceneLoadRequest = null;

        #endregion

        #region Publishers
        public void RaiseSceneLoadEvent(SceneAssetPreset targetScene, bool showLoadingScreen)
        {
            if (targetScene == null) return;

            OnSceneLoadRequest?.Invoke(targetScene, showLoadingScreen);
        }
        #endregion
    }
}
