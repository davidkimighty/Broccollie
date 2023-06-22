using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

namespace Broccollie.System
{
    [DefaultExecutionOrder(-100)]
    public class SceneAddressableLoader : MonoBehaviour
    {
        [SerializeField] private SceneAddressableEventChannel _sceneEventChannel = null;
        [SerializeField] private SceneAddressablePreset _loadingScene = null;

        private SceneAddressablePreset _currentlyLoadedScene = null;
        private bool _sceneUnloading = false;
        private bool _sceneLoading = false;
        private bool _loadingSceneLoaded = false;

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

            _sceneEventChannel.RaiseBeforeSceneUnload();
            await _sceneEventChannel.RaiseBeforeSceneUnloadAsync();

            if (_currentlyLoadedScene != null)
                SceneUnload(_currentlyLoadedScene);

            if (showLoading)
            {
                await SceneLoadAsync(_loadingScene, true);
                _loadingSceneLoaded = true;

                _sceneEventChannel.RaiseAfterLoadingSceneLoad();
                await _sceneEventChannel.RaiseAfterLoadingSceneLoadAsync();
            }
            _sceneUnloading = false;
        }

        public async Task LoadNewSceneAsync(SceneAddressablePreset newScene)
        {
            if (_sceneLoading) return;
            _sceneLoading = true;

            if (_loadingSceneLoaded)
            {
                _sceneEventChannel.RaiseBeforeLoadingSceneUnload();
                await _sceneEventChannel.RaiseBeforeLoadingSceneUnloadAsync();

                SceneUnload(_loadingScene);
                _loadingSceneLoaded = false;
            }

            await SceneLoadAsync(newScene, true);
            _currentlyLoadedScene = newScene;

            _sceneEventChannel.RaiseAfterSceneLoad();
            await _sceneEventChannel.RaiseAfterSceneLoadAsync();

            _sceneLoading = false;
        }

        #endregion

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
                //float progress = loadOperation.PercentComplete;
                //Debug.Log($"[SceneLoader] Load progress {progress}");
                await Task.Yield();
            }
        }
    }
}
