using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Broccollie.System
{
    [CreateAssetMenu(fileName = "ScenePreset", menuName = "Broccollie/System/Scene Preset")]
    public class SceneAddressablePreset : ScriptableObject
    {
        public int SceneId = 0;
        public string SceneName = null;
        public AssetReference SceneReference = null;

#if UNITY_EDITOR
        private void OnValidate()
        {
            SceneName = SceneReference.editorAsset.name;
        }
#endif
    }
}
