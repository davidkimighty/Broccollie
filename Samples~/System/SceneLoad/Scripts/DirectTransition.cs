using System.Threading.Tasks;
using Broccollie.System;
using UnityEngine;

public class DirectTransition : MonoBehaviour
{
    [SerializeField] private SceneAddressableEventChannel _sceneEventChannel = null;
    [SerializeField] private SceneAddressablePreset _scene = null;

    private async void Awake()
    {
        await Task.Delay(3000);

        await _sceneEventChannel.RequestSceneLoadAsync(_scene, false);
    }
}
