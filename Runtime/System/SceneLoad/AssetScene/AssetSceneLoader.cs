using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Broccollie.System
{
    public class AssetSceneLoader : MonoBehaviour
    {
        [SerializeField] private AssetSceneEventChannel _eventChannel;

        private void OnEnable()
        {
            _eventChannel.OnSceneLoadAsync += LoadSceneAsync;
        }

        private void OnDisable()
        {
            _eventChannel.OnSceneLoadAsync -= LoadSceneAsync;
        }

        #region Subscribers
        public async Task LoadSceneAsync(AssetScenePreset scene)
        {
            try
            {
                AsyncOperation loadOperation = SceneManager.LoadSceneAsync(scene.SceneName, LoadSceneMode.Single);
                if (loadOperation == null) return;

                while (!loadOperation.isDone)
                {
                    float progress = Mathf.Clamp01(loadOperation.progress / 0.9f);
                    _eventChannel.SceneLoadPrgress(progress);
                    //Debug.Log($"[ SceneLoader ] Load progress: {progress}");
                    await Task.Yield();
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        #endregion
    }
}
