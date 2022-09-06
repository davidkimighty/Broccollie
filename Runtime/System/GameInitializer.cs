using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace CollieMollie.System
{
    public class GameInitializer : MonoBehaviour
    {
        #region Variable Field
        [Header("Initializer")]
        [SerializeField] private ScenePreset persistentScene = null;

        #endregion

        private void Start()
        {
            if (persistentScene.sceneType == SceneType.Persistent)
            {
                persistentScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true).Completed += UnloadInitializer;
            }
        }

        #region Subscribers
        private void UnloadInitializer(AsyncOperationHandle<SceneInstance> obj)
        {
            SceneManager.UnloadSceneAsync(0);
        }
        #endregion
    }
}
