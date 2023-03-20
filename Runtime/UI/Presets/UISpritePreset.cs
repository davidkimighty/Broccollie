using System;
using UnityEngine;

namespace Broccollie.UI
{
    [CreateAssetMenu(fileName = "SpritePreset", menuName = "Broccollie/UI/Preset/Sprite")]
    public class UISpritePreset : UIBasePreset
    {
        public SpriteSetting[] Settings = null;

        [Serializable]
        public class SpriteSetting : Setting
        {
            public Sprite Sprite;
            public float Delay;
        }
    }
}
