using UnityEditor;
using UnityEngine;

namespace Broccollie.System
{
    [CreateAssetMenu(fileName = "AssetScene", menuName = "Broccollie/System/Preset/AssetScene")]
    public class AssetScenePreset : ScriptableObject
    {
        private string _sceneName = null;
        public string SceneName
        {
            get => _sceneName;
        }

#if UNITY_EDITOR
        public SceneAsset Scene = null;

        private void OnValidate()
        {
            if (Scene != null)
                _sceneName = Scene.name;
        }
#endif
    }
}
