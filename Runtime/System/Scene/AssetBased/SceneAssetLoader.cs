using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CollieMollie.Shaders;
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
        [SerializeField] private float _loadingSceneDuration = 1f;

        [SerializeField] private FadeController _fadeController = null;
        [SerializeField] private float _fadeDuration = 1f;

        private bool _loading = false;
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
        private void LoadNewScene(SceneAssetPreset scene, bool showLoadingScreen)
        {
            if (_loading) return;
            _loading = true;

            _cts.Cancel();
            _cts = new CancellationTokenSource();

            Task sceneLoadTask = LoadSceneAsync(scene, showLoadingScreen, _cts.Token);
        }
        #endregion

        private async Task LoadSceneAsync(SceneAssetPreset newScenePreset, bool showLoadingScreen, CancellationToken token)
        {
            if (_fadeController != null)
                await _fadeController.ChangeFadeAmountAsync(1, _fadeDuration);

            if (_currentActiveScene != null)
                await UnloadSceneAsync(_currentActiveScene);

            if (showLoadingScreen)
            {
                await LoadSceneAsync(_loadingScene);
                if (_fadeController != null)
                    await _fadeController.ChangeFadeAmountAsync(0, _fadeDuration);

                await Task.Delay((int)_loadingSceneDuration * 1000, token);
            }

            if (showLoadingScreen)
            {
                if (_fadeController != null)
                    await _fadeController.ChangeFadeAmountAsync(1, _fadeDuration);
                await UnloadSceneAsync(_loadingScene);
            }

            await LoadSceneAsync(newScenePreset);
            _currentActiveScene = newScenePreset;

            if (_fadeController != null)
                await _fadeController.ChangeFadeAmountAsync(0, _fadeDuration);
            _loading = false;

            async Task UnloadSceneAsync(SceneAssetPreset preset)
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

            async Task LoadSceneAsync(SceneAssetPreset preset)
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
        }
    }
}
