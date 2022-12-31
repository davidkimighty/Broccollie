using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
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

        [SerializeField] private FadeController _fadeController = null;
        [SerializeField] private float _fadeDuration = 1f;

        private bool _loading = false;
        private SceneAddressablePreset _currentlyLoadedScene = null;
        private IEnumerator _sceneLoadAction = null;
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

            if (_sceneLoadAction != null)
                StopCoroutine(_sceneLoadAction);

            _sceneLoadAction = SceneLoadProcess(scene, showLoadingScreen);
            StartCoroutine(_sceneLoadAction);
        }
        #endregion

        #region Scene Load Features
        private IEnumerator SceneLoadProcess(SceneAddressablePreset targetScene, bool showLoadingScreen)
        {
            yield return _fadeController.FadeAmount(1, _fadeDuration);

            if (_currentlyLoadedScene != null)
                SceneUnload(_currentlyLoadedScene);

            if (showLoadingScreen)
            {
                yield return SceneLoad(_loadingScene, true);
                yield return _fadeController.FadeAmount(0, _fadeDuration);

                yield return new WaitForSeconds(2f);
            }

            if (showLoadingScreen)
            {
                yield return _fadeController.FadeAmount(1, _fadeDuration);
                SceneUnload(_loadingScene);
            }

            yield return SceneLoad(targetScene, true);
            _currentlyLoadedScene = targetScene;

            yield return _fadeController.FadeAmount(0, _fadeDuration);
            _loading = false;
        }

        private void SceneUnload(SceneAddressablePreset scene)
        {
            scene.SceneReference.UnLoadScene();
        }

        private IEnumerator SceneLoad(SceneAddressablePreset scene, bool activate)
        {
            AsyncOperationHandle loadOperation = scene.SceneReference.LoadSceneAsync(LoadSceneMode.Additive, activate);
            if (!loadOperation.IsValid()) yield break;
            
            while (!loadOperation.IsDone)
            {
                float progress = loadOperation.PercentComplete;
                //Debug.Log($"[SceneLoader] Load {progress}");
                yield return null;
            }
        }
        #endregion
    }
}
