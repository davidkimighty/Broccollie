using System.Threading.Tasks;
using Broccollie.System.Scene.Addressables;
using UnityEngine;

public class GamePlayController : MonoBehaviour
{
    [SerializeField] private AddressableSceneEventChannel _sceneEventChannel = null;
    [SerializeField] private GameObject _object = null;

    private void OnEnable()
    {
        _sceneEventChannel.OnAfterSceneLoadAsync += Init;
    }

    private void OnDisable()
    {
        _sceneEventChannel.OnAfterSceneLoadAsync -= Init;
    }

    private async Task Init(AddressableScenePreset preset)
    {
        await Task.Delay(2 * 1000);
        _object.SetActive(true);
    }
}
