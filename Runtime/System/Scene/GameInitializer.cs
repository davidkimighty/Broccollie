using UnityEngine;
using UnityEngine.SceneManagement;

namespace Broccollie.System
{
    public class GameInitializer : MonoBehaviour
    {
        [Header("Initializer")]
        [SerializeField] private SceneAddressablePreset _persistentScene = null;

        private void Start()
        {
            _persistentScene.SceneReference.LoadSceneAsync(LoadSceneMode.Single, true);
        }
    }
}
