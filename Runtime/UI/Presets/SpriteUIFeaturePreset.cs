using System;
using UnityEngine;

namespace Broccollie.UI
{
    [CreateAssetMenu(fileName = "SpritePreset", menuName = "Broccollie/UI/Preset/Sprite")]
    public class SpriteUIFeaturePreset : ScriptableObject
    {
        public Setting[] Settings;

        [Serializable]
        public struct Setting
        {
            public bool IsEnabled;
            public UIStates ExecutionState;
            public Sprite Sprite;
            public float Delay;
        }
    }
}
