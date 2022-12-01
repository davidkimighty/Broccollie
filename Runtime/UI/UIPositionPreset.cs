using System;
using System.Collections;
using System.Collections.Generic;
using CollieMollie.Core;
using UnityEngine;

namespace CollieMollie.UI
{
    [Serializable]
    public class UIPositionPreset
    {
        public Setting[] Settings;

        [Serializable]
        public struct Setting
        {
            public bool IsEnabled;
            public UIState State;
            public Transform TargetPoint;
            public float Duration;
            public AnimationCurve Curve;

            public bool IsValid() => State != UIState.None;
        }
    }
}
