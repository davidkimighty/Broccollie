using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Broccollie.System
{
    [CreateAssetMenu(fileName = "EventChannel_AssetScene", menuName = "Broccollie/EventChannels/AssetScene")]
    public class AssetSceneEventChannel : ScriptableObject
    {
        public event Action<AssetScenePreset> OnBeforeSceneUnload = null;
        public event Func<AssetScenePreset, Task> OnBeforeSceneUnloadAsync = null;

        public event Action<AssetScenePreset> OnAfterSceneLoad = null;
        public event Func<AssetScenePreset, Task> OnAfterSceneLoadAsync = null;

        public event Action<AssetScenePreset> OnBeforeLoadingSceneUnload = null;
        public event Func<AssetScenePreset, Task> OnBeforeLoadingSceneUnloadAsync = null;

        public event Action<AssetScenePreset> OnAfterLoadingSceneLoad = null;
        public event Func<AssetScenePreset, Task> OnAfterLoadingSceneLoadAsync = null;

        public event Action<AssetScenePreset, bool> OnSceneLoad = null;
        public event Func<AssetScenePreset, bool, Task> OnSceneLoadAsync = null;

        #region Publishers
        public void RaiseBeforeSceneUnload(AssetScenePreset scene) => OnBeforeSceneUnload?.Invoke(scene);
        public async Task RaiseBeforeSceneUnloadAsync(AssetScenePreset scene)
        {
            if (OnBeforeSceneUnloadAsync != null)
                await OnBeforeSceneUnloadAsync.Invoke(scene);
        }

        public void RaiseAfterSceneLoad(AssetScenePreset scene) => OnAfterSceneLoad?.Invoke(scene);
        public async Task RaiseAfterSceneLoadAsync(AssetScenePreset scene)
        {
            if (OnAfterSceneLoadAsync != null)
                await OnAfterSceneLoadAsync.Invoke(scene);
        }

        public void RaiseBeforeLoadingSceneUnload(AssetScenePreset scene) => OnBeforeLoadingSceneUnload?.Invoke(scene);
        public async Task RaiseBeforeLoadingSceneUnloadAsync(AssetScenePreset scene)
        {
            if (OnBeforeLoadingSceneUnloadAsync != null)
                await OnBeforeLoadingSceneUnloadAsync.Invoke(scene);
        }

        public void RaiseAfterLoadingSceneLoad(AssetScenePreset scene) => OnAfterLoadingSceneLoad?.Invoke(scene);
        public async Task RaiseAfterLoadingSceneLoadAsync(AssetScenePreset scene)
        {
            if (OnAfterLoadingSceneLoadAsync != null)
                await OnAfterLoadingSceneLoadAsync.Invoke(scene);
        }

        public void RequestSceneLoad(AssetScenePreset scene, bool showLoading)
        {
            if (scene == null) return;
            OnSceneLoad?.Invoke(scene, showLoading);
        }

        public async Task RequestSceneLoadAsync(AssetScenePreset scene, bool showLoading)
        {
            if (scene == null || OnSceneLoadAsync == null) return;
            await OnSceneLoadAsync.Invoke(scene, showLoading);
        }

        #endregion
    }
}
