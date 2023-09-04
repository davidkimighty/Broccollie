using System.Threading.Tasks;
using Broccollie.System;
using UnityEngine;

public class AssetSceneTransition : MonoBehaviour
{
    [SerializeField] private AssetSceneEventChannel _eventChannel = null;
    [SerializeField] private AssetScenePreset _sceneOne = null;
    [SerializeField] private AssetScenePreset _sceneTwo = null;

    [SerializeField] private ScreenFader _fader = null;

    private async void Awake()
    {
        await _eventChannel.RequestSceneLoadAsync(_sceneOne, false);

        await _eventChannel.RequestSceneLoadAsync(_sceneTwo, true);
    }

    private void OnEnable()
    {
        _eventChannel.OnBeforeSceneUnloadAsync += FadeIn;
        _eventChannel.OnAfterLoadingSceneLoadAsync += FadeOut;
        _eventChannel.OnBeforeLoadingSceneUnloadAsync += FadeIn;
        _eventChannel.OnAfterSceneLoadAsync += FadeOut;
    }

    private void OnDisable()
    {
        _eventChannel.OnBeforeSceneUnloadAsync -= FadeIn;
        _eventChannel.OnAfterLoadingSceneLoadAsync -= FadeOut;
        _eventChannel.OnBeforeLoadingSceneUnloadAsync -= FadeIn;
        _eventChannel.OnAfterSceneLoadAsync -= FadeOut;
    }

    private async Task FadeIn(AssetScenePreset preset)
    {
        await _fader.FadeAsync(1);
    }

    private async Task FadeOut(AssetScenePreset preset)
    {
        await _fader.FadeAsync(0);
    }
}