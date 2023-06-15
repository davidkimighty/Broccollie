using System;
using UnityEngine;

namespace Broccollie.UI
{
    [CreateAssetMenu(fileName = "SpritePreset", menuName = "Broccollie/UI/Preset/Sprite")]
    public class SpriteUIPreset : BaseUIPreset
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
