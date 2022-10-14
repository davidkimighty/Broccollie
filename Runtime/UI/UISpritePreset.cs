using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CollieMollie.UI
{
    [CreateAssetMenu(fileName = "UISpritePreset", menuName = "CollieMollie/UI/SpritePreset")]
    public class UISpritePreset : ScriptableObject
    {
        public SpriteState[] SpriteStates = null;

        [System.Serializable]
        public struct SpriteState
        {
            public Sprite TargetSprite;
            public InteractionState ExecutionState;
            public bool IsEnabled;

            public bool IsValid() => ExecutionState != InteractionState.None;
        }
    }
}
