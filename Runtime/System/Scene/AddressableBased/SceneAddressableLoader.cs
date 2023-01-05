using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using CollieMollie.Shaders;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

namespace CollieMollie.System
{
    [DefaultExecutionOrder(-100)]
    public class SceneAddressableLoader : MonoBehaviour
    {
        #region Variable Field
        [Header("Scene Loader")]
        [SerializeField] private SceneAddressableEventChannel _sceneEventChannel = null;
        [SerializeField] private SceneAddressablePreset _loadingScene = null;
        [SerializeField] private float _loadingSceneDuration = 1f;

        [SerializeField] private FadeController _fadeController = null;
        [SerializeField] private float _fadeDuration = 1f;

        private bool _loading = false;
        private SceneAddressablePreset _currentlyLoadedScene = null;
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
        private void LoadNewScene(SceneAddressablePreset scene, bool showLoadingScreen)
        {
            if (_loading) return;
            _loading = true;

            _cts.Cancel();
            _cts = new CancellationTokenSource();

            Task sceneLoadTask = LoadSceneAsync(scene, showLoadingScreen, _cts.Token);
        }

        #endregion

        #region Scene Load Features
        private async Task LoadSceneAsync(SceneAddressablePreset targetScene, bool showLoading, CancellationToken token)
        {
            if (_fadeController != null)
                await _fadeController.ChangeFadeAmountAsync(1, _fadeDuration);

            if (_currentlyLoadedScene != null)
                SceneUnload(_currentlyLoadedScene);

            if (showLoading)
            {
                await SceneLoadAsync(_loadingScene, true, token);
                if (_fadeController != null)
                    await _fadeController.ChangeFadeAmountAsync(0, _fadeDuration);

                await Task.Delay((int)_loadingSceneDuration * 1000, token);
            }

            if (showLoading)
            {
                if (_fadeController != null)
                    await _fadeController.ChangeFadeAmountAsync(1, _fadeDuration);
                SceneUnload(_loadingScene);
            }

            await SceneLoadAsync(targetScene, true, token);
            _currentlyLoadedScene = targetScene;

            if (_fadeController != null)
                await _fadeController.ChangeFadeAmountAsync(0, _fadeDuration);
            _loading = false;
        }

        private void SceneUnload(SceneAddressablePreset scene)
        {
            scene.SceneReference.UnLoadScene();
        }

        private async Task SceneLoadAsync(SceneAddressablePreset scene, bool activate, CancellationToken token)
        {
            AsyncOperationHandle loadOperation = scene.SceneReference.LoadSceneAsync(LoadSceneMode.Additive, activate);
            if (!loadOperation.IsValid()) return;
            
            while (!loadOperation.IsDone)
            {
                token.ThrowIfCancellationRequested();

                float progress = loadOperation.PercentComplete;
                //Debug.Log($"[SceneLoader] Load {progress}");
                await Task.Yield();
            }
        }
        #endregion
    }
}
