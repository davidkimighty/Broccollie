using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
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
        private void LoadNewScene(SceneAddressablePreset scene, bool showLoadingScreen, float loadingScreenDuration)
        {
            _cts.Cancel();
            _cts = new CancellationTokenSource();

            Task sceneLoadTask = LoadSceneAsync(scene, showLoadingScreen, loadingScreenDuration, _cts.Token);
        }

        #endregion

        #region Scene Load Features
        private async Task LoadSceneAsync(SceneAddressablePreset targetScene, bool showLoading, float loadingScreenDuration, CancellationToken token)
        {
            _sceneEventChannel.InvokeBeforeSceneUnload();

            if (_currentlyLoadedScene != null)
                SceneUnload(_currentlyLoadedScene);

            if (showLoading)
            {
                await SceneLoadAsync(_loadingScene, true, token);
                _sceneEventChannel.InvokeAfterSceneLoad();

                await Task.Delay((int)loadingScreenDuration * 1000, token);
            }

            if (showLoading)
            {
                _sceneEventChannel.InvokeBeforeSceneUnload();
                SceneUnload(_loadingScene);
            }

            await SceneLoadAsync(targetScene, true, token);
            _currentlyLoadedScene = targetScene;

            _sceneEventChannel.InvokeAfterSceneLoad();
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
