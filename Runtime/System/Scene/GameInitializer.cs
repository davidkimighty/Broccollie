using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Broccollie.System
{
    public class GameInitializer : MonoBehaviour
    {
        #region Variable Field
        [Header("Initializer")]
        [SerializeField] private SceneAddressablePreset _persistentScene = null;

        #endregion

        private void Start()
        {
            if (_persistentScene.SceneType == SceneType.Persistent)
            {
                _persistentScene.SceneReference.LoadSceneAsync(LoadSceneMode.Single, true);
            }
        }
    }
}
