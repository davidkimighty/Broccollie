using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Broccollie.System.Scene.Addressables
{
    [DefaultExecutionOrder(-100)]
    public class AddressableSceneLoader : MonoBehaviour
    {
        [SerializeField] private AddressableSceneEventChannel _sceneEventChannel = null;
        [SerializeField] private AddressableScenePreset _loadingScene = null;
        [SerializeField] private AddressableScenePreset _persistentScene = null;

        private AddressableScenePreset _currentlyLoadedScene = null;

        private void OnEnable()
        {
            if (_sceneEventChannel != null)
                _sceneEventChannel.OnRequestLoadSceneAsync += SceneLoadAsync;
        }

        private void OnDisable()
        {
            if (_sceneEventChannel != null)
                _sceneEventChannel.OnRequestLoadSceneAsync -= SceneLoadAsync;
        }

        #region Subscribers
        private async Task SceneLoadAsync(AddressableScenePreset scene, bool showLoading)
        {
            await UnloadActiveSceneAsync(showLoading);
            await LoadNewSceneAsync(scene);
        }

        #endregion

        #region Public Functions
        public async Task UnloadActiveSceneAsync(bool showLoading)
        {
            _sceneEventChannel.RaiseBeforeSceneUnload(_currentlyLoadedScene);
            await _sceneEventChannel.RaiseBeforeSceneUnloadAsync(_currentlyLoadedScene);

            await UnloadActiveSceneAsync();

            if (showLoading)
            {
                _currentlyLoadedScene = _loadingScene;
                await LoadSceneAsync(_loadingScene);

                _sceneEventChannel.RaiseAfterLoadingSceneLoad(_loadingScene);
                await _sceneEventChannel.RaiseAfterLoadingSceneLoadAsync(_loadingScene);
            }
        }

        public async Task LoadNewSceneAsync(AddressableScenePreset newScene)
        {
            if (_currentlyLoadedScene == _loadingScene)
            {
                _sceneEventChannel.RaiseBeforeLoadingSceneUnload(_loadingScene);
                await _sceneEventChannel.RaiseBeforeLoadingSceneUnloadAsync(_loadingScene);

                await UnloadActiveSceneAsync();
            }

            _currentlyLoadedScene = newScene;
            await LoadSceneAsync(newScene);

            _sceneEventChannel.RaiseAfterSceneLoad(_currentlyLoadedScene);
            await _sceneEventChannel.RaiseAfterSceneLoadAsync(_currentlyLoadedScene);
        }

        #endregion

        private async Task UnloadActiveSceneAsync()
        {
            if (_currentlyLoadedScene == null) return;

            var activeScene = SceneManager.GetActiveScene();
            if (!activeScene.IsValid()) return;

            if (_persistentScene != null && activeScene.name == _persistentScene.SceneName) return;

            try
            {
                AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(_currentlyLoadedScene.SceneName);
                while (!unloadOperation.isDone)
                {
                    //Debug.Log($"[SceneLoader] Unload progress {_currentlyLoadedScene.SceneName} {unloadOperation.progress}");
                    await Task.Yield();
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private async Task LoadSceneAsync(AddressableScenePreset scene)
        {
            try
            {
                AsyncOperationHandle<SceneInstance> loadOperation = scene.SceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);
                if (!loadOperation.IsValid()) return;

                while (!loadOperation.IsDone)
                {
                    //Debug.Log($"[SceneLoader] Load progress {scene.SceneName} {loadOperation.PercentComplete}");
                    await Task.Yield();
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}
