using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Broccollie.System
{
    [CreateAssetMenu(fileName = "EventChannel_LocalSave", menuName = "Broccollie/EventChannels/LocalSave")]
    public class LocalSaveEventChannel : ScriptableObject
    {
        public event Func<Task> OnSaveAsync;
        public event Func<Task> OnLoadAsync;

        #region Publishers
        public async Task RequestSaveAsync() => await OnSaveAsync?.Invoke();

        public async Task RequestLoadAsync() => await OnLoadAsync?.Invoke();

        #endregion
    }
}
