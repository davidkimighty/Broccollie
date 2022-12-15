using System.Collections;
using System.Collections.Generic;
using CollieMollie.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CollieMollie.System
{
    public class SceneAssetLoader : MonoBehaviour
    {
        #region Variable Field
        [Header("Scene Loader")]
        [SerializeField] private SceneAssetEventChannel _sceneEventChannel = null;
        [SerializeField] private SceneAssetPreset _loadingScene = null;

        [SerializeField] private FadeController _fadeController = null;

        private bool _loading = false;
        private SceneAssetPreset _currentActiveScene = null;
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
        private void LoadNewScene(SceneAssetPreset scene, bool showLoadingScreen, float loadingDuration)
        {
            if (_loading) return;
            _loading = true;

            if (_sceneLoadAction != null)
                StopCoroutine(_sceneLoadAction);

            _sceneLoadAction = LoadScene(scene, showLoadingScreen, loadingDuration);
            StartCoroutine(_sceneLoadAction);
        }
        #endregion

        private IEnumerator LoadScene(SceneAssetPreset newScenePreset, bool showLoadingScreen, float duration)
        {
            yield return _fadeController.FadeIn();
            yield return UnloadScene(_currentActiveScene);

            if (showLoadingScreen)
            {
                yield return LoadScene(_loadingScene);
                yield return _fadeController.FadeOut();

                yield return new WaitForSeconds(duration);
            }

            if (showLoadingScreen)
            {
                yield return _fadeController.FadeIn();
                yield return UnloadScene(_loadingScene);
            }

            yield return LoadScene(newScenePreset);
            _currentActiveScene = newScenePreset;

            yield return _fadeController.FadeOut();
            _loading = false;

            IEnumerator UnloadScene(SceneAssetPreset preset)
            {
                string sceneName = null;
                if (preset == null)
                {
                    Scene scene = SceneManager.GetActiveScene();
                    if (!scene.IsValid() || scene.buildIndex == 0) yield break;
                    sceneName = scene.name;
                }
                else
                {
                    if (preset.SceneType == SceneType.Persistent) yield break;
                    sceneName = preset.SceneName;
                }

                AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(sceneName);
                if (unloadOperation == null) yield break;

                while (!unloadOperation.isDone)
                {
                    float progress = Mathf.Clamp01(unloadOperation.progress / 0.9f);
                    //Debug.Log($"[GameSceneManager] Unload {progress}");
                    yield return null;
                }
            }

            IEnumerator LoadScene(SceneAssetPreset preset)
            {
                AsyncOperation loadOperation = SceneManager.LoadSceneAsync(preset.SceneName, LoadSceneMode.Additive);
                if (loadOperation == null) yield break;

                while (!loadOperation.isDone)
                {
                    float progress = Mathf.Clamp01(loadOperation.progress / 0.9f);
                    //Debug.Log($"[GameSceneManager] Load {progress}");
                    yield return null;
                }
            }
        }
    }
}
