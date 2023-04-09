using UnityEngine;
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
            _persistentScene.SceneReference.LoadSceneAsync(LoadSceneMode.Single, true);
        }
    }
}
