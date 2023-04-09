using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

namespace Broccollie.System
{
    [DefaultExecutionOrder(-100)]
    public class SceneAddressableLoader : MonoBehaviour
    {
        #region Variable Field
        public event Func<Task> OnBeforeSceneUnloadAsync = null;
        public event Func<Task> OnAfterSceneLoadAsync = null;

        [SerializeField] private SceneAddressableEventChannel _sceneEventChannel = null;
        [SerializeField] private SceneAddressablePreset _loadingScene = null;

        private SceneAddressablePreset _currentlyLoadedScene = null;
        private bool _sceneUnloading = false;
        private bool _sceneLoading = false;
        private bool _loadingSceneLoaded = false;

        #endregion

        private void OnEnable()
        {
            if (_sceneEventChannel != null)
                _sceneEventChannel.OnRequestLoadSceneAsync += SceneLoad;
        }

        private void OnDisable()
        {
            if (_sceneEventChannel != null)
                _sceneEventChannel.OnRequestLoadSceneAsync -= SceneLoad;
        }

        #region Subscribers
        private async Task SceneLoad(SceneAddressablePreset scene, bool showLoading)
        {
            await UnloadActiveSceneAsync(showLoading);

            await LoadNewSceneAsync(scene);
        }

        #endregion

        #region Public Functions
        public async Task UnloadActiveSceneAsync(bool showLoading)
        {
            if (_sceneUnloading) return;
            _sceneUnloading = true;

            if (OnBeforeSceneUnloadAsync != null)
                await OnBeforeSceneUnloadAsync?.Invoke();

            if (_currentlyLoadedScene != null)
                SceneUnload(_currentlyLoadedScene);

            if (showLoading)
            {
                await SceneLoadAsync(_loadingScene, true);
                _loadingSceneLoaded = true;

                if (OnAfterSceneLoadAsync != null)
                    await OnAfterSceneLoadAsync?.Invoke();
            }
            _sceneUnloading = false;
        }

        public async Task LoadNewSceneAsync(SceneAddressablePreset newScene)
        {
            if (_sceneLoading) return;
            _sceneLoading = true;

            if (_loadingSceneLoaded)
            {
                if (OnBeforeSceneUnloadAsync != null)
                    await OnBeforeSceneUnloadAsync?.Invoke();
                SceneUnload(_loadingScene);
                _loadingSceneLoaded = false;
            }

            await SceneLoadAsync(newScene, true);
            _currentlyLoadedScene = newScene;

            if (OnAfterSceneLoadAsync != null)
                await OnAfterSceneLoadAsync?.Invoke();
            _sceneLoading = false;
        }

        #endregion

        #region Private Functions
        private void SceneUnload(SceneAddressablePreset scene)
        {
            scene.SceneReference.UnLoadScene();
        }

        private async Task SceneLoadAsync(SceneAddressablePreset scene, bool activate)
        {
            AsyncOperationHandle loadOperation = scene.SceneReference.LoadSceneAsync(LoadSceneMode.Additive, activate);
            if (!loadOperation.IsValid()) return;
            
            while (!loadOperation.IsDone)
            {
                float progress = loadOperation.PercentComplete;
                //Debug.Log($"[SceneLoader] Load {progress}");
                await Task.Yield();
            }
        }
        #endregion
    }
}
