using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Broccollie.System
{
    [DefaultExecutionOrder(-100)]
    public class SceneAddressableLoader : MonoBehaviour
    {
        [SerializeField] private SceneAddressableEventChannel _sceneEventChannel = null;
        [SerializeField] private SceneAddressablePreset _loadingScene = null;

        private SceneAddressablePreset _currentlyLoadedScene = null;

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
            await UnloadActiveScene();

            if (showLoading)
            {
                _currentlyLoadedScene = _loadingScene;
                await LoadSceneAsync(_loadingScene);

                _sceneEventChannel.RaiseAfterLoadingSceneLoad(_loadingScene);
                await _sceneEventChannel.RaiseAfterLoadingSceneLoadAsync(_loadingScene);
            }
        }

        public async Task LoadNewSceneAsync(SceneAddressablePreset newScene)
        {
            if (_currentlyLoadedScene == _loadingScene)
            {
                _sceneEventChannel.RaiseBeforeLoadingSceneUnload(_loadingScene);
                await _sceneEventChannel.RaiseBeforeLoadingSceneUnloadAsync(_loadingScene);

                await UnloadActiveScene();
            }

            _currentlyLoadedScene = newScene;
            await LoadSceneAsync(newScene);
        }

        #endregion

        private async Task UnloadActiveScene()
        {
            if (_currentlyLoadedScene == null || _currentlyLoadedScene.SceneId == 0) return;

            _sceneEventChannel.RaiseBeforeSceneUnload(_currentlyLoadedScene);
            await _sceneEventChannel.RaiseBeforeSceneUnloadAsync(_currentlyLoadedScene);

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

        private async Task LoadSceneAsync(SceneAddressablePreset scene)
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

                _sceneEventChannel.RaiseAfterSceneLoad(_currentlyLoadedScene);
                await _sceneEventChannel.RaiseAfterSceneLoadAsync(_currentlyLoadedScene);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}
