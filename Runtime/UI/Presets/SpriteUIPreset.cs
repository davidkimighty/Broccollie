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

#if UNITY_EDITOR
        private void OnValidate()
        {
            for (int i = 0; i < Settings.Length; i++)
            {
                if (Settings[i].ExecutionState == null || Settings[i].ExecutionState == string.Empty)
                    Settings[i].ExecutionState = Settings[i].ExecutionStateHelper.ToString();
            }
        }
#endif
    }
}
