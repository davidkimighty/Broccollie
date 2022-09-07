using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CollieMollie.System
{
    [CreateAssetMenu(fileName = "ScenePreset", menuName = "CollieMollie/System/ScenePreset")]
    public class ScenePreset : ScriptableObject
    {
        #region Variable Field
        public SceneType SceneType;
        public AssetReference SceneReference = null;
        #endregion
    }

    public enum SceneType
    {
        Persistent,
        Title,
        Gameplay
    }
}
