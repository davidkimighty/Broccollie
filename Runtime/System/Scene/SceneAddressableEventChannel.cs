using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Broccollie.System
{
    [CreateAssetMenu(fileName = "EventChannel_Scene", menuName = "Broccollie/Event Channels/Scene")]
    public class SceneAddressableEventChannel : ScriptableObject
    {
        public event Func<SceneAddressablePreset, bool, Task> OnRequestLoadSceneAsync = null;

        public event Action OnBeforeSceneUnload = null;
        public event Func<Task> OnBeforeSceneUnloadAsync = null;

        public event Action OnAfterSceneLoad = null;
        public event Func<Task> OnAfterSceneLoadAsync = null;

        public event Action OnAfterLoadingSceneLoad = null;
        public event Func<Task> OnAfterLoadingSceneLoadAsync = null;

        public event Action OnBeforeLoadingSceneUnload = null;
        public event Func<Task> OnBeforeLoadingSceneUnloadAsync = null;

        #region Publishers
        public async Task RequestSceneLoadAsync(SceneAddressablePreset scene, bool showLoadingScene)
        {
            if (scene == null) return;
            if (OnRequestLoadSceneAsync != null)
                await OnRequestLoadSceneAsync.Invoke(scene, showLoadingScene);
        }

        public void RaiseBeforeSceneUnload() => OnBeforeSceneUnload?.Invoke();

        public async Task RaiseBeforeSceneUnloadAsync()
        {
            if (OnBeforeSceneUnloadAsync != null)
                await OnBeforeSceneUnloadAsync.Invoke();
        }

        public void RaiseAfterSceneLoad() => OnAfterSceneLoad?.Invoke();

        public async Task RaiseAfterSceneLoadAsync()
        {
            if (OnAfterSceneLoadAsync != null)
                await OnAfterSceneLoadAsync.Invoke();
        }

        public void RaiseAfterLoadingSceneLoad() => OnAfterLoadingSceneLoad?.Invoke();

        public async Task RaiseAfterLoadingSceneLoadAsync()
        {
            if (OnAfterLoadingSceneLoadAsync != null)
                await OnAfterLoadingSceneLoadAsync.Invoke();
        }

        public void RaiseBeforeLoadingSceneUnload() => OnBeforeLoadingSceneUnload?.Invoke();

        public async Task RaiseBeforeLoadingSceneUnloadAsync()
        {
            if (OnBeforeLoadingSceneUnloadAsync != null)
                await OnBeforeLoadingSceneUnloadAsync.Invoke();
        }

        #endregion
    }
}
