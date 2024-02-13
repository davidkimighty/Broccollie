using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Broccollie.System
{
    [CreateAssetMenu(fileName = "EventChannel_AssetScene", menuName = "Broccollie/EventChannels/AssetScene")]
    public class AssetSceneEventChannel : ScriptableObject
    {
        public event Func<AssetScenePreset, Task> OnSceneLoadAsync;
        public event Action<float> OnLoadProgress;

        #region Publishers
        public async Task RequestSceneLoadAsync(AssetScenePreset scene)
        {
            if (scene == null || OnSceneLoadAsync == null) return;
            await OnSceneLoadAsync.Invoke(scene);
        }

        public void SceneLoadPrgress(float progress) => OnLoadProgress?.Invoke(progress);

        #endregion
    }
}
