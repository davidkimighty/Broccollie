using UnityEngine;

namespace Broccollie.UI
{
    public abstract class BaseUIPreset : ScriptableObject
    {
        public abstract class Setting
        {
#if UNITY_EDITOR
            public UIStates ExecutionStateHelper = UIStates.Default;
#endif
            public string ExecutionState = null;
            public bool IsEnabled = true;
        }
    }
}
