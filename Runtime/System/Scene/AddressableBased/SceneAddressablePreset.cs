using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CollieMollie.System
{
    [CreateAssetMenu(fileName = "SceneAddressablePreset", menuName = "CollieMollie/System/SceneAddressablePreset")]
    public class SceneAddressablePreset : ScriptableObject
    {
        public SceneType SceneType;
        public AssetReference SceneReference = null;
    }

    public enum SceneType
    {
        Persistent,
        Title,
        Loading,
        Gameplay
    }
}
