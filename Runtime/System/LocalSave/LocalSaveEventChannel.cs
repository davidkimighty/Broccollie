using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Broccollie.System
{
    [CreateAssetMenu(fileName = "EventChannel_LocalSave", menuName = "Broccollie/Event Channels/Local Save")]
    public class LocalSaveEventChannel : ScriptableObject
    {
        #region Events
        public event Func<Task> OnSaveRequestAsync = null;
        public event Func<Task> OnLoadRequestAsync = null;

        public event Action OnSaveRequest = null;
        public event Action OnLoadRequest = null;

        #endregion

        #region Publishers
        public async Task RaiseSaveEventAsync() => await OnSaveRequestAsync?.Invoke();

        public async Task RaiseLoadEventAsync() => await OnLoadRequestAsync?.Invoke();

        public void RaiseSaveEvent() => OnSaveRequest?.Invoke();

        public void RaiseLoadEvent() => OnLoadRequest?.Invoke();

        #endregion
    }
}
