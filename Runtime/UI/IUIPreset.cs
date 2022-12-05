using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CollieMollie.UI
{
    public interface IUIPreset
    {
        public bool IsValid(UIAllState state);

        public float GetDuration(string state);
    }

    public enum UIAllState
    {
        None = -1,
        Default,
        Interactive,
        NonInteractive,
        Show,
        Hide,
        Hovered,
        Pressed,
        Selected,
        Drag,
    }
}
