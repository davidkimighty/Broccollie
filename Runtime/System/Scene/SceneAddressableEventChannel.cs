using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Broccollie.System
{
    [CreateAssetMenu(fileName = "EventChannel_SceneAddressable", menuName = "Broccollie/Event Channels/Scene Addressable")]
    public class SceneAddressableEventChannel : ScriptableObject
    {
        #region Events
        public event Func<SceneAddressablePreset, bool, Task> OnRequestLoadSceneAsync = null;

        #endregion

        #region Publishers
        public async Task RaiseSceneLoadEventAsync(SceneAddressablePreset scene, bool showLoadingScene)
        {
            if (scene == null) return;
            await OnRequestLoadSceneAsync?.Invoke(scene, showLoadingScene);
        }

        #endregion
    }
}
