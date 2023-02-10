using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CollieMollie.System
{
    public class SceneAssetLoader : MonoBehaviour
    {
        #region Variable Field
        public event Func<Task> OnBeforeSceneUnload = null;
        public event Func<Task> OnAfterSceneLoad = null;

        [SerializeField] private SceneAssetPreset _loadingScene = null;

        private SceneAssetPreset _currentActiveScene = null;
        private bool _sceneUnloading = false;
        private bool _sceneLoading = false;

        #endregion

        #region Public Functions
        public async Task UnloadActiveScene(bool showLoading)
        {
            if (_sceneUnloading) return;
            _sceneUnloading = true;

            await OnBeforeSceneUnload?.Invoke();
            if (_currentActiveScene != null)
                await UnloadSceneAsync(_currentActiveScene);

            if (showLoading)
            {
                await LoadSceneAsync(_loadingScene);
                await OnAfterSceneLoad?.Invoke();
            }
            _sceneUnloading = false;
        }

        public async Task LoadNewScene(SceneAssetPreset newScene)
        {
            if (_sceneLoading) return;
            _sceneLoading = true;

            Scene activeScene = SceneManager.GetActiveScene();
            if (activeScene.name == _loadingScene.SceneName)
            {
                await OnBeforeSceneUnload?.Invoke();
                await UnloadSceneAsync(_loadingScene);
            }

            await LoadSceneAsync(newScene);
            _currentActiveScene = newScene;

            await OnAfterSceneLoad?.Invoke();
            _sceneLoading = false;
        }

        #endregion

        #region Private Functions
        private async Task UnloadSceneAsync(SceneAssetPreset preset)
        {
            string sceneName = null;
            if (preset == null)
            {
                Scene scene = SceneManager.GetActiveScene();
                if (!scene.IsValid() || scene.buildIndex == 0) return;
                sceneName = scene.name;
            }
            else
            {
                if (preset.SceneType == SceneType.Persistent) return;
                sceneName = preset.SceneName;
            }

            AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(sceneName);
            if (unloadOperation == null) return;

            while (!unloadOperation.isDone)
            {
                float progress = Mathf.Clamp01(unloadOperation.progress / 0.9f);
                //Debug.Log($"[GameSceneManager] Unload {progress}");
                await Task.Yield();
            }
        }

        private async Task LoadSceneAsync(SceneAssetPreset preset)
        {
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(preset.SceneName, LoadSceneMode.Additive);
            if (loadOperation == null) return;

            while (!loadOperation.isDone)
            {
                float progress = Mathf.Clamp01(loadOperation.progress / 0.9f);
                //Debug.Log($"[GameSceneManager] Load {progress}");
                await Task.Yield();
            }
        }

        #endregion
    }
}
