using System.Threading.Tasks;
using Broccollie.System;
using UnityEngine;

public class GamePlayController : MonoBehaviour
{
    [SerializeField] private SceneAddressableEventChannel _sceneEventChannel = null;
    [SerializeField] private GameObject _object = null;

    private void OnEnable()
    {
        _sceneEventChannel.OnAfterSceneLoadAsync += Init;
    }

    private void OnDisable()
    {
        _sceneEventChannel.OnAfterSceneLoadAsync -= Init;
    }

    private async Task Init(SceneAddressablePreset preset)
    {
        await Task.Delay(2 * 1000);
        _object.SetActive(true);
    }
}
