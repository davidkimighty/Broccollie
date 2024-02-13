using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Broccollie.System.Scene.Addressables
{
    [CreateAssetMenu(fileName = "AddressableScene", menuName = "Broccollie/System/AddressableScene")]
    public class AddressableScenePreset : ScriptableObject
    {
        public AssetReference SceneReference = null;

        [SerializeField] private string _sceneName = null;
        public string SceneName
        {
            get => _sceneName;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (SceneReference != null)
                _sceneName = SceneReference.editorAsset.name;
        }
#endif
    }
}
