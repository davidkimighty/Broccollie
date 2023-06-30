using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Broccollie.System
{
    [CreateAssetMenu(fileName = "EventChannel_Scene", menuName = "Broccollie/Event Channels/Scene")]
    public class SceneAddressableEventChannel : ScriptableObject
    {
        public event Func<SceneAddressablePreset, bool, Task> OnRequestLoadSceneAsync = null;

        public event Action<SceneAddressablePreset> OnBeforeSceneUnload = null;
        public event Func<SceneAddressablePreset, Task> OnBeforeSceneUnloadAsync = null;

        public event Action<SceneAddressablePreset> OnAfterSceneLoad = null;
        public event Func<SceneAddressablePreset, Task> OnAfterSceneLoadAsync = null;

        public event Action<SceneAddressablePreset> OnAfterLoadingSceneLoad = null;
        public event Func<SceneAddressablePreset, Task> OnAfterLoadingSceneLoadAsync = null;

        public event Action<SceneAddressablePreset> OnBeforeLoadingSceneUnload = null;
        public event Func<SceneAddressablePreset, Task> OnBeforeLoadingSceneUnloadAsync = null;

        #region Publishers
        public async Task RequestSceneLoadAsync(SceneAddressablePreset scene, bool showLoadingScene)
        {
            if (scene == null) return;
            await OnRequestLoadSceneAsync.Invoke(scene, showLoadingScene);
        }

        public void RaiseBeforeSceneUnload(SceneAddressablePreset scene) => OnBeforeSceneUnload?.Invoke(scene);

        public async Task RaiseBeforeSceneUnloadAsync(SceneAddressablePreset scene)
        {
            if (OnBeforeSceneUnloadAsync != null)
                await OnBeforeSceneUnloadAsync.Invoke(scene);
        }

        public void RaiseAfterSceneLoad(SceneAddressablePreset scene) => OnAfterSceneLoad?.Invoke(scene);

        public async Task RaiseAfterSceneLoadAsync(SceneAddressablePreset scene)
        {
            if (OnAfterSceneLoadAsync != null)
                await OnAfterSceneLoadAsync.Invoke(scene);
        }

        public void RaiseAfterLoadingSceneLoad(SceneAddressablePreset scene) => OnAfterLoadingSceneLoad?.Invoke(scene);

        public async Task RaiseAfterLoadingSceneLoadAsync(SceneAddressablePreset scene)
        {
            if (OnAfterLoadingSceneLoadAsync != null)
                await OnAfterLoadingSceneLoadAsync.Invoke(scene);
        }

        public void RaiseBeforeLoadingSceneUnload(SceneAddressablePreset scene) => OnBeforeLoadingSceneUnload?.Invoke(scene);

        public async Task RaiseBeforeLoadingSceneUnloadAsync(SceneAddressablePreset scene)
        {
            if (OnBeforeLoadingSceneUnloadAsync != null)
                await OnBeforeLoadingSceneUnloadAsync.Invoke(scene);
        }

        #endregion
    }
}
