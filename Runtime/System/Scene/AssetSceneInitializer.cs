using UnityEngine;
using UnityEngine.SceneManagement;

namespace Broccollie.System
{
    public class AssetSceneInitializer : MonoBehaviour
    {
        [SerializeField] private AssetScenePreset _persistentScene = null;

        private void Start()
        {
            SceneManager.LoadScene(_persistentScene.SceneName);
        }
    }
}
