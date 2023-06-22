using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Broccollie.System
{
    [CreateAssetMenu(fileName = "ScenePreset", menuName = "Broccollie/System/Scene Preset")]
    public class SceneAddressablePreset : ScriptableObject
    {
        public int SceneId = 0;
        public AssetReference SceneReference = null;
    }
}
