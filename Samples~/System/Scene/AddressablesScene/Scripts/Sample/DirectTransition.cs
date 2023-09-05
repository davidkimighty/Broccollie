using System.Threading.Tasks;
using Broccollie.System.Scene.Addressables;
using UnityEngine;

public class DirectTransition : MonoBehaviour
{
    [SerializeField] private AddressableSceneEventChannel _sceneEventChannel = null;
    [SerializeField] private AddressableScenePreset _scene = null;

    private async void Awake()
    {
        await Task.Delay(3000);

        await _sceneEventChannel.RequestSceneLoadAsync(_scene, false);
    }
}
