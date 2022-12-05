using System;
using System.Collections;
using System.Collections.Generic;
using CollieMollie.Core;
using UnityEngine;

namespace CollieMollie.UI
{
    [CreateAssetMenu(fileName = "UISpritePreset", menuName = "CollieMollie/UI/SpritePreset")]
    public class UISpritePreset : ScriptableObject, IUIPreset
    {
        public Setting[] States = null;

        public float GetDuration(string state)
        {
            Setting setting = Array.Find(States, x => x.ExecutionState.ToString() == state);
            if (IsValid(setting.ExecutionState))
                return setting.DelayTime;
            return 0;
        }

        public bool IsValid(UIAllState state)
        {
            return state != UIAllState.None;
        }

        [Serializable]
        public struct Setting
        {
            public bool IsEnabled;
            public Sprite TargetSprite;
            public float DelayTime;
            public UIAllState ExecutionState;
        }
    }
}
