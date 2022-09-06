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
        [SerializeField] private EventChannel sceneEventChannel = null;
        [SerializeField] private ScenePreset loadingScene = null;

        [SerializeField] private FadeController fadeController = null;

        private bool loading = false;
        private ScenePreset currentlyLoadedScene = null;
        private IEnumerator sceneLoadAction = null;
        #endregion

        private void OnEnable()
        {
            sceneEventChannel.OnSceneLoadRequest += LoadNewScene;
        }

        private void OnDisable()
        {
            sceneEventChannel.OnSceneLoadRequest -= LoadNewScene;
        }

        #region Subscribers
        private void LoadNewScene(ScenePreset scene, bool showLoadingScreen)
        {
            if (loading) return;
            loading = true;

            if (sceneLoadAction != null)
                StopCoroutine(sceneLoadAction);

            sceneLoadAction = SceneLoadProcess(scene, showLoadingScreen);
            StartCoroutine(sceneLoadAction);
        }
        #endregion

        #region Scene Load Features
        private IEnumerator SceneLoadProcess(ScenePreset targetScene, bool showLoadingScreen, Action transitionLogic = null)
        {
            yield return fadeController.FadeIn();

            if (currentlyLoadedScene != null)
            {
                yield return SceneUnload(currentlyLoadedScene);
            }

            if (showLoadingScreen)
            {
                yield return SceneLoad(loadingScene, true);
                yield return fadeController.FadeOut();
            }

            transitionLogic?.Invoke();
            yield return new WaitForSeconds(2f);

            if (showLoadingScreen)
            {
                yield return fadeController.FadeIn();
                yield return SceneUnload(loadingScene);
            }

            yield return SceneLoad(targetScene, true);
            currentlyLoadedScene = targetScene;

            yield return fadeController.FadeOut();
            loading = false;
        }

        private IEnumerator SceneUnload(ScenePreset scene)
        {
            string name = scene.sceneReference.editorAsset.name;
            AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(name);
            if (unloadOperation == null) yield break;

            while (!unloadOperation.isDone)
            {
                float progress = Mathf.Clamp01(unloadOperation.progress / 0.9f);
                //Debug.Log($"[SceneLoader] Unload {progress}");
                yield return null;
            }
        }

        private IEnumerator SceneLoad(ScenePreset scene, bool activate)
        {
            AsyncOperationHandle loadOperation = scene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, activate);
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
