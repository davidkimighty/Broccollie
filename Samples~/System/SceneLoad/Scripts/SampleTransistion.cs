using System.Threading.Tasks;
using Broccollie.System;
using UnityEngine;

public class SampleTransistion : MonoBehaviour
{
    [SerializeField] private SceneAddressableEventChannel _sceneEventChannel = null;
    [SerializeField] private SceneAddressablePreset _sceneOne = null;
    [SerializeField] private SceneAddressablePreset _sceneTwo = null;

    [SerializeField] private ScreenFader _screenFader = null;

    private void Awake()
    {
        _screenFader.Fade(0);
    }

    private async void Start()
    {
        await _sceneEventChannel.RequestSceneLoadAsync(_sceneOne, false);

        await Task.Delay(3 * 1000);

        await _sceneEventChannel.RequestSceneLoadAsync(_sceneTwo, true);
    }

    private void OnEnable()
    {
        _sceneEventChannel.OnBeforeTransitionAsync += FadeIn;
        _sceneEventChannel.OnAfterTransitionAsync += FadeOut;
    }

    private void OnDisable()
    {
        _sceneEventChannel.OnBeforeTransitionAsync -= FadeIn;
        _sceneEventChannel.OnAfterTransitionAsync -= FadeOut;
    }

    private async Task FadeIn()
    {
        await _screenFader.FadeAsync(1);
    }

    private async Task FadeOut()
    {
        await _screenFader.FadeAsync(0);
    }
}