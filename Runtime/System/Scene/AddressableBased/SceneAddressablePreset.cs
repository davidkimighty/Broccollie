using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CollieMollie.System
{
    [CreateAssetMenu(fileName = "SceneAddressablePreset", menuName = "CollieMollie/System/SceneAddressablePreset")]
    public class SceneAddressablePreset : ScriptableObject
    {
        public SceneType SceneType = SceneType.Persistent;
        public int SceneId = 0;
        public AssetReference SceneReference = null;
    }

    public enum SceneType
    {
        Persistent,
        Loading,
        Entrance,
        InGame
    }
}
