using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

namespace CollieMollie.System
{
    public class SceneAddressableLoader : MonoBehaviour
    {
        #region Variable Field
        public event Func<Task> OnBeforeSceneUnload = null;
        public event Func<Task> OnAfterSceneLoad = null;

        [SerializeField] private SceneAddressablePreset _loadingScene = null;

        private SceneAddressablePreset _currentlyLoadedScene = null;
        private bool _sceneUnloading = false;
        private bool _sceneLoading = false;
        private bool _loadingSceneLoaded = false;

        #endregion

        #region Public Functions
        public async Task UnloadActiveSceneAsync(bool showLoading)
        {
            if (_sceneUnloading) return;
            _sceneUnloading = true;

            await OnBeforeSceneUnload?.Invoke();
            if (_currentlyLoadedScene != null)
                SceneUnload(_currentlyLoadedScene);

            if (showLoading)
            {
                await SceneLoadAsync(_loadingScene, true);
                _loadingSceneLoaded = true;
                await OnAfterSceneLoad?.Invoke();
            }
            _sceneUnloading = false;
        }

        public async Task LoadNewSceneAsync(SceneAddressablePreset newScene)
        {
            if (_sceneLoading) return;
            _sceneLoading = true;

            if (_loadingSceneLoaded)
            {
                await OnBeforeSceneUnload?.Invoke();
                SceneUnload(_loadingScene);
                _loadingSceneLoaded = false;
            }

            await SceneLoadAsync(newScene, true);
            _currentlyLoadedScene = newScene;

            await OnAfterSceneLoad?.Invoke();
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
