using System;
using System.Collections;
using System.Collections.Generic;
using CollieMollie.Core;
using UnityEngine;

namespace CollieMollie.UI
{
    [CreateAssetMenu(fileName = "UISpritePreset", menuName = "CollieMollie/UI/SpritePreset")]
    public class UISpritePreset : ScriptableObject
    {
        public Setting[] States = null;

        [Serializable]
        public struct Setting
        {
            public bool IsEnabled;
            public Sprite TargetSprite;
            public float DelayTime;
            public BaseUI.State ExecutionState;
        }
    }
}
