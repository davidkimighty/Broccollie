using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CollieMollie.System
{
    [DefaultExecutionOrder(-100)]
    public class SceneAssetLoader : MonoBehaviour
    {
        #region Variable Field
        [Header("Scene Loader")]
        [SerializeField] private SceneAssetEventChannel _sceneEventChannel = null;
        [SerializeField] private SceneAssetPreset _loadingScene = null;

        private SceneAssetPreset _currentActiveScene = null;
        private CancellationTokenSource _cts = new CancellationTokenSource();

        #endregion

        private void OnEnable()
        {
            _sceneEventChannel.OnSceneLoadRequest += LoadNewScene;
        }

        private void OnDisable()
        {
            _sceneEventChannel.OnSceneLoadRequest -= LoadNewScene;
        }

        #region Subscribers
        private void LoadNewScene(SceneAssetPreset scene, bool showLoadingScreen, float loadingScreenDuration)
        {
            _cts.Cancel();
            _cts = new CancellationTokenSource();

            Task sceneLoadTask = LoadSceneProcessAsync(scene, showLoadingScreen, loadingScreenDuration, _cts.Token);
        }

        #endregion

        #region Private Functions
        private async Task LoadSceneProcessAsync(SceneAssetPreset newScenePreset, bool showLoadingScreen, float loadingScreenDuration, CancellationToken token)
        {
            _sceneEventChannel.InvokeBeforeSceneUnload();

            if (_currentActiveScene != null)
                await UnloadSceneAsync(_currentActiveScene, token);

            if (showLoadingScreen)
            {
                await LoadSceneAsync(_loadingScene, token);
                _sceneEventChannel.InvokeAfterSceneLoad();

                await Task.Delay((int)loadingScreenDuration * 1000, token);
            }

            if (showLoadingScreen)
            {
                _sceneEventChannel.InvokeBeforeSceneUnload();
                await UnloadSceneAsync(_loadingScene, token);
            }

            await LoadSceneAsync(newScenePreset, token);
            _currentActiveScene = newScenePreset;

            _sceneEventChannel.InvokeAfterSceneLoad();
        }

        private async Task UnloadSceneAsync(SceneAssetPreset preset, CancellationToken token)
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
                token.ThrowIfCancellationRequested();

                float progress = Mathf.Clamp01(unloadOperation.progress / 0.9f);
                //Debug.Log($"[GameSceneManager] Unload {progress}");
                await Task.Yield();
            }
        }

        private async Task LoadSceneAsync(SceneAssetPreset preset, CancellationToken token)
        {
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(preset.SceneName, LoadSceneMode.Additive);
            if (loadOperation == null) return;

            while (!loadOperation.isDone)
            {
                token.ThrowIfCancellationRequested();

                float progress = Mathf.Clamp01(loadOperation.progress / 0.9f);
                //Debug.Log($"[GameSceneManager] Load {progress}");
                await Task.Yield();
            }
        }

        #endregion
    }
}
