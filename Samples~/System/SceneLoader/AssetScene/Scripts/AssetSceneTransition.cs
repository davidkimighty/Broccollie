using System.Threading.Tasks;
using Broccollie.System;
using UnityEngine;

public class AssetSceneTransition : MonoBehaviour
{
    [SerializeField] private AssetSceneEventChannel _eventChannel = null;
    [SerializeField] private AssetScenePreset _sceneOne = null;
    [SerializeField] private AssetScenePreset _sceneTwo = null;
    [SerializeField] private AssetScenePreset _sceneLoading = null;

    [SerializeField] private ScreenFader _fader = null;

    private async void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        await Task.Delay(3000);

        await _fader.FadeAsync(1);
        await _eventChannel.RequestSceneLoadAsync(_sceneLoading);
        await _fader.FadeAsync(0);

        await Task.Delay(3000);

        await _fader.FadeAsync(1);
        await _eventChannel.RequestSceneLoadAsync(_sceneTwo);
        await _fader.FadeAsync(0);
    }

}