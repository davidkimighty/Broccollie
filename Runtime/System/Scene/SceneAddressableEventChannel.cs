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

        public event Action OnBeforeSceneUnload = null;
        public event Func<Task> OnBeforeSceneUnloadAsync = null;

        public event Action OnAfterSceneLoad = null;
        public event Func<Task> OnAfterSceneLoadAsync = null;

        public event Func<Task> OnBeforeTransitionAsync = null;
        public event Func<Task> OnAfterTransitionAsync = null;

        #endregion

        #region Publishers
        public async Task RequestSceneLoadAsync(SceneAddressablePreset scene, bool showLoadingScene)
        {
            if (scene == null) return;
            await OnRequestLoadSceneAsync?.Invoke(scene, showLoadingScene);
        }

        public void RaiseBeforeSceneUnload() => OnBeforeSceneUnload?.Invoke();

        public async Task RaiseBeforeSceneUnloadAsync()
        {
            if (OnBeforeSceneUnloadAsync != null)
                await OnBeforeSceneUnloadAsync?.Invoke();
        }

        public void RaiseAfterSceneLoad() => OnAfterSceneLoad?.Invoke();

        public async Task RaiseAfterSceneLoadAsync()
        {
            if (OnAfterSceneLoadAsync != null)
                await OnAfterSceneLoadAsync?.Invoke();
        }

        public async Task RaiseBeforeTransitionAsync()
        {
            if (OnBeforeTransitionAsync != null)
                await OnBeforeTransitionAsync?.Invoke();
        }

        public async Task RaiseAfterTransitionAsync()
        {
            if (OnAfterTransitionAsync != null)
                await OnAfterTransitionAsync?.Invoke();
        }

        #endregion
    }
}
