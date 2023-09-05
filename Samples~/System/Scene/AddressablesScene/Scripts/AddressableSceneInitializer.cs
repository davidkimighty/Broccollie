using UnityEngine;
using UnityEngine.SceneManagement;

namespace Broccollie.System.Scene.Addressables
{
    public class AddressableSceneInitializer : MonoBehaviour
    {
        [Header("Initializer")]
        [SerializeField] private AddressableScenePreset _persistentScene = null;

        private void Start()
        {
            _persistentScene.SceneReference.LoadSceneAsync(LoadSceneMode.Single);
        }
    }
}
