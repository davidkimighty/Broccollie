using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CollieMollie.System
{
    [CreateAssetMenu(fileName = "EventChannel", menuName = "CollieMollie/System/EventChannel")]
    public class EventChannel : ScriptableObject
    {
        public event Action<ScenePreset, bool> OnSceneLoadRequest = null;

        public void RaiseEvent(ScenePreset targetScene, bool showLoadingScreen)
        {
            if (targetScene == null) return;

            OnSceneLoadRequest?.Invoke(targetScene, showLoadingScreen);
        }
    }
}
