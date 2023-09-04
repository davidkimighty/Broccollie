using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Broccollie.System
{
    [DefaultExecutionOrder(-100)]
    public class AssetSceneLoader : MonoBehaviour
    {
        [SerializeField] private AssetSceneEventChannel _eventChannel = null;
        [SerializeField] private AssetScenePreset _loadingScene = null;
        [SerializeField] private AssetScenePreset _persistentScene = null;

        private AssetScenePreset _currentlyLoadedScene = null;

        private void OnEnable()
        {
            _eventChannel.OnSceneLoadAsync += SceneLoadAsync;
        }

        private void OnDisable()
        {
            _eventChannel.OnSceneLoadAsync -= SceneLoadAsync;
        }

        #region Subscribers
        private async Task SceneLoadAsync(AssetScenePreset scene, bool showLoading)
        {
            await UnloadSceneAsync(showLoading);
            await LoadSceneAsync(scene);
        }

        #endregion

        #region Public Functions
        public async Task UnloadSceneAsync(bool showLoading)
        {
            if (_currentlyLoadedScene != null)
            {
                _eventChannel.RaiseBeforeSceneUnload(_currentlyLoadedScene);
                await _eventChannel.RaiseBeforeSceneUnloadAsync(_currentlyLoadedScene);
            }
            await UnloadActiveSceneAsync();

            if (showLoading)
            {
                _currentlyLoadedScene = _loadingScene;
                await LoadNewSceneAsync(_loadingScene);

                _eventChannel.RaiseAfterLoadingSceneLoad(_loadingScene);
                await _eventChannel.RaiseAfterLoadingSceneLoadAsync(_loadingScene);
            }
        }

        public async Task LoadSceneAsync(AssetScenePreset scene)
        {
            if (_currentlyLoadedScene == _loadingScene)
            {
                _eventChannel.RaiseBeforeLoadingSceneUnload(_loadingScene);
                await _eventChannel.RaiseBeforeLoadingSceneUnloadAsync(_loadingScene);

                await UnloadActiveSceneAsync();
            }

            _currentlyLoadedScene = scene;
            await LoadNewSceneAsync(scene);

            _eventChannel.RaiseAfterSceneLoad(_currentlyLoadedScene);
            await _eventChannel.RaiseAfterSceneLoadAsync(_currentlyLoadedScene);
        }

        #endregion

        private async Task UnloadActiveSceneAsync()
        {
            if (_currentlyLoadedScene == null ||
                (_persistentScene != null && _currentlyLoadedScene.SceneName == _persistentScene.SceneName)) return;

            try
            {
                AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(_currentlyLoadedScene.SceneName);
                if (unloadOperation == null) return;

                while (!unloadOperation.isDone)
                {
                    float progress = Mathf.Clamp01(unloadOperation.progress / 0.9f);
                    //Debug.Log($"[ SceneLoader ] Unload progress: {progress}");
                    await Task.Yield();
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private async Task LoadNewSceneAsync(AssetScenePreset scene)
        {
            try
            {
                AsyncOperation loadOperation = SceneManager.LoadSceneAsync(scene.SceneName, LoadSceneMode.Additive);
                if (loadOperation == null) return;

                while (!loadOperation.isDone)
                {
                    float progress = Mathf.Clamp01(loadOperation.progress / 0.9f);
                    //Debug.Log($"[ SceneLoader ] Load progress: {progress}");
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
