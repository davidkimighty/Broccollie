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
            _sceneEventChannel.RaiseBeforeSceneUnload();
            await _sceneEventChannel.RaiseBeforeSceneUnloadAsync();

            await UnloadActiveScene();

            if (showLoading)
            {
                await LoadSceneAsync(_loadingScene);
                _currentlyLoadedScene = _loadingScene;

                _sceneEventChannel.RaiseAfterLoadingSceneLoad();
                await _sceneEventChannel.RaiseAfterLoadingSceneLoadAsync();
            }
        }

        public async Task LoadNewSceneAsync(SceneAddressablePreset newScene)
        {
            if (_currentlyLoadedScene == _loadingScene)
            {
                _sceneEventChannel.RaiseBeforeLoadingSceneUnload();
                await _sceneEventChannel.RaiseBeforeLoadingSceneUnloadAsync();

                await UnloadActiveScene();
            }

            await LoadSceneAsync(newScene);
            _currentlyLoadedScene = newScene;

            _sceneEventChannel.RaiseAfterSceneLoad();
            await _sceneEventChannel.RaiseAfterSceneLoadAsync();
        }

        #endregion

        private async Task UnloadActiveScene()
        {
            if (_currentlyLoadedScene == null || _currentlyLoadedScene.SceneId == 0) return;

            AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(_currentlyLoadedScene.SceneReference.editorAsset.name);
            while (!unloadOperation.isDone)
            {
                //Debug.Log($"[SceneLoader] Unload progress {unloadOperation.progress}");
                await Task.Yield();
            }
        }

        private async Task LoadSceneAsync(SceneAddressablePreset scene)
        {
            AsyncOperationHandle<SceneInstance> loadOperation = scene.SceneReference.LoadSceneAsync(LoadSceneMode.Additive);
            if (!loadOperation.IsValid()) return;
            
            while (!loadOperation.IsDone)
            {
                //Debug.Log($"[SceneLoader] Load progress {loadOperation.PercentComplete}");
                await Task.Yield();
            }
        }
    }
}
