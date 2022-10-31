using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CollieMollie.System
{
    [CreateAssetMenu(fileName = "SceneAddressablePreset", menuName = "CollieMollie/System/SceneAddressablePreset")]
    public class SceneAddressablePreset : ScriptableObject
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
        Loading,
        Gameplay
    }
}
