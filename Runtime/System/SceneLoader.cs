using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using CollieMollie.Rendering;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

namespace CollieMollie.System
{
    public class SceneLoader : MonoBehaviour
    {
        #region Variable Field
        [Header("Scene Loader")]
        [SerializeField] private SceneEventChannel _sceneEventChannel = null;
        [SerializeField] private ScenePreset _loadingScene = null;

        [SerializeField] private FadeController _fadeController = null;

        private bool _loading = false;
        private ScenePreset _currentlyLoadedScene = null;
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
        private void LoadNewScene(ScenePreset scene, bool showLoadingScreen)
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
        private IEnumerator SceneLoadProcess(ScenePreset targetScene, bool showLoadingScreen, Action transitionLogic = null)
        {
            yield return _fadeController.FadeIn();

            if (_currentlyLoadedScene != null)
                SceneUnload(_currentlyLoadedScene);

            if (showLoadingScreen)
            {
                yield return SceneLoad(_loadingScene, true);
                yield return _fadeController.FadeOut();
            }

            transitionLogic?.Invoke();
            yield return new WaitForSeconds(2f);

            if (showLoadingScreen)
            {
                yield return _fadeController.FadeIn();
                SceneUnload(_loadingScene);
            }

            yield return SceneLoad(targetScene, true);
            _currentlyLoadedScene = targetScene;

            yield return _fadeController.FadeOut();
            _loading = false;
        }

        private void SceneUnload(ScenePreset scene)
        {
            scene.SceneReference.UnLoadScene();
        }

        private IEnumerator SceneLoad(ScenePreset scene, bool activate)
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
