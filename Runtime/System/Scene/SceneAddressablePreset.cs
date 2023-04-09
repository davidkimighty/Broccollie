using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Broccollie.System
{
    [CreateAssetMenu(fileName = "SceneAddressablePreset", menuName = "Broccollie/System/Scene Addressable Preset")]
    public class SceneAddressablePreset : ScriptableObject
    {
        public int SceneId = 0;
        public AssetReference SceneReference = null;
    }
}
