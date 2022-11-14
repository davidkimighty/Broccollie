using System.Collections;
using System.Collections.Generic;
using CollieMollie.Core;
using UnityEngine;

namespace CollieMollie.UI
{
    [CreateAssetMenu(fileName = "UIScalePreset", menuName = "CollieMollie/UI/ScalePreset")]
    public class UIScalePreset : ScriptableObject
    {
        public ScaleState[] ScaleStates = null;

        [System.Serializable]
        public struct ScaleState
        {
            public float TargetScale;
            public InteractionState ExecutionState;
            public bool IsEnabled;
            public float Duration;
            public AnimationCurve Curve;

            public bool IsValid() => ExecutionState != InteractionState.None;
        }
    }
}

