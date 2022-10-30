using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CollieMollie.System
{
    [CreateAssetMenu(fileName = "SceneAssetPreset", menuName = "CollieMollie/System/SceneAssetPreset")]
    public class SceneAssetPreset : ScriptableObject
    {
        public SceneType SceneType;
#if UNITY_EDITOR
        public SceneAsset scene;
#endif
    }
}
