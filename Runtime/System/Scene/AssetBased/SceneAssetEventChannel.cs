using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CollieMollie.System
{
    [CreateAssetMenu(fileName = "EventChannel_SceneAsset", menuName = "CollieMollie/Event Channels/Scene Asset")]
    public class SceneAssetEventChannel : ScriptableObject
    {
        #region Events
        public event Action<SceneAssetPreset, bool, float> OnSceneLoadRequest = null;

        #endregion

        #region Publishers
        public void RaiseSceneLoadEvent(SceneAssetPreset targetScene, bool showLoadingScreen, float loadingDuration = 1f)
        {
            if (targetScene == null) return;

            OnSceneLoadRequest?.Invoke(targetScene, showLoadingScreen, loadingDuration);
        }
        #endregion
    }
}
