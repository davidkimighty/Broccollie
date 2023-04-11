using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Broccollie.System
{
    [CreateAssetMenu(fileName = "EventChannel_LocalSave", menuName = "Broccollie/Event Channels/Local Save")]
    public class LocalSaveEventChannel : ScriptableObject
    {
        #region Events
        public event Func<Task> OnRequestSaveAsync = null;
        public event Func<Task> OnRequestLoadAsync = null;

        #endregion

        #region Publishers
        public async Task RequestSaveAsync() => await OnRequestSaveAsync?.Invoke();

        public async Task RequestLoadAsync() => await OnRequestLoadAsync?.Invoke();

        #endregion
    }
}
