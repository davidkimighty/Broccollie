using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CollieMollie.System
{
    [CreateAssetMenu(fileName = "ScenePreset", menuName = "CollieMollie/System/ScenePreset")]
    public class ScenePreset : ScriptableObject
    {
        public SceneType sceneType;
        public AssetReference sceneReference = null;
    }

    public enum SceneType
    {
        Persistent,
        Title,
        Gameplay
    }
}
