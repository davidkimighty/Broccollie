using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CollieMollie.System
{
    [CreateAssetMenu(fileName = "SceneAssetPreset", menuName = "CollieMollie/System/SceneAssetPreset")]
    public class SceneAssetPreset : ScriptableObject
    {
        public SceneType SceneType;
#if UNITY_EDITOR
        public SceneAsset Scene;
#endif
        public string SceneName;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (Scene != null)
                SceneName = Scene.name;
        }
#endif
    }
}
