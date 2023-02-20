using System;
using System.Threading.Tasks;
using UnityEngine;

namespace CollieMollie.System
{
    [CreateAssetMenu(fileName = "EventChannel_SaveLoad", menuName = "CollieMollie/EventChannels/SaveLoad")]
    public class SaveLoadEventChannel : ScriptableObject
    {
        #region Events
        public Func<Task> OnSaveRequestAsync = null;
        public Func<Task> OnLoadRequestAsync = null;

        public Action OnSaveRequest = null;
        public Action OnLoadRequest = null;

        #endregion

        #region Publishers
        public async Task RaiseSaveEventAsync() => await OnSaveRequestAsync?.Invoke();

        public async Task RaiseLoadEventAsync() => await OnLoadRequestAsync?.Invoke();

        public void RaiseSaveEvent() => OnSaveRequest?.Invoke();

        public void RaiseLoadEvent() => OnLoadRequest?.Invoke();

        #endregion
    }
}
