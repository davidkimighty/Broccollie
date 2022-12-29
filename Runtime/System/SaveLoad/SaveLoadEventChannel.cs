using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CollieMollie.System
{
    [CreateAssetMenu(fileName = "EventChannel_SaveLoad", menuName = "CollieMollie/Event Channels/Save & Load")]
    public class SaveLoadEventChannel : ScriptableObject
    {
        #region Events
        public event Action<object, SaveOptions, Action> OnSaveRequest = null;
        public event Action<object, SaveOptions, Action> OnLoadRequest = null;

        #endregion

        #region Publishers
        public void RaiseSaveEvent(object data, SaveOptions saveOptions, Action afterComplete = null)
        {
            OnSaveRequest?.Invoke(data, saveOptions, afterComplete);
        }

        public void RaiseLoadEvent(object data, SaveOptions saveOptions, Action afterComplete = null)
        {
            OnLoadRequest?.Invoke(data, saveOptions, afterComplete);
        }

        #endregion
    }
}
