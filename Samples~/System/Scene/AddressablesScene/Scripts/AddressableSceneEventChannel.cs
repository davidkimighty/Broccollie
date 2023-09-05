using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Broccollie.System.Scene.Addressables
{
    [CreateAssetMenu(fileName = "EventChannel_AddressableScene", menuName = "Broccollie/EventChannels/AddressableScene")]
    public class AddressableSceneEventChannel : ScriptableObject
    {
        public event Action<AddressableScenePreset> OnBeforeSceneUnload = null;
        public event Func<AddressableScenePreset, Task> OnBeforeSceneUnloadAsync = null;

        public event Action<AddressableScenePreset> OnAfterSceneLoad = null;
        public event Func<AddressableScenePreset, Task> OnAfterSceneLoadAsync = null;

        public event Action<AddressableScenePreset> OnAfterLoadingSceneLoad = null;
        public event Func<AddressableScenePreset, Task> OnAfterLoadingSceneLoadAsync = null;

        public event Action<AddressableScenePreset> OnBeforeLoadingSceneUnload = null;
        public event Func<AddressableScenePreset, Task> OnBeforeLoadingSceneUnloadAsync = null;

        public event Func<AddressableScenePreset, bool, Task> OnRequestLoadSceneAsync = null;

        #region Publishers
        public void RaiseBeforeSceneUnload(AddressableScenePreset scene) => OnBeforeSceneUnload?.Invoke(scene);
        public async Task RaiseBeforeSceneUnloadAsync(AddressableScenePreset scene)
        {
            if (OnBeforeSceneUnloadAsync != null)
                await OnBeforeSceneUnloadAsync.Invoke(scene);
        }

        public void RaiseAfterSceneLoad(AddressableScenePreset scene) => OnAfterSceneLoad?.Invoke(scene);
        public async Task RaiseAfterSceneLoadAsync(AddressableScenePreset scene)
        {
            if (OnAfterSceneLoadAsync != null)
                await OnAfterSceneLoadAsync.Invoke(scene);
        }

        public void RaiseAfterLoadingSceneLoad(AddressableScenePreset scene) => OnAfterLoadingSceneLoad?.Invoke(scene);
        public async Task RaiseAfterLoadingSceneLoadAsync(AddressableScenePreset scene)
        {
            if (OnAfterLoadingSceneLoadAsync != null)
                await OnAfterLoadingSceneLoadAsync.Invoke(scene);
        }

        public void RaiseBeforeLoadingSceneUnload(AddressableScenePreset scene) => OnBeforeLoadingSceneUnload?.Invoke(scene);
        public async Task RaiseBeforeLoadingSceneUnloadAsync(AddressableScenePreset scene)
        {
            if (OnBeforeLoadingSceneUnloadAsync != null)
                await OnBeforeLoadingSceneUnloadAsync.Invoke(scene);
        }

        public async Task RequestSceneLoadAsync(AddressableScenePreset scene, bool showLoadingScene)
        {
            if (scene == null) return;
            await OnRequestLoadSceneAsync.Invoke(scene, showLoadingScene);
        }

        #endregion
    }
}
