using System.Threading.Tasks;
using Broccollie.System;
using Broccollie.System.Scene.Addressables;
using UnityEngine;

public class AddressableSceneTransition : MonoBehaviour
{
    [SerializeField] private AddressableSceneEventChannel _sceneEventChannel = null;
    [SerializeField] private AddressableScenePreset _sceneOne = null;
    [SerializeField] private AddressableScenePreset _sceneTwo = null;

    [SerializeField] private ScreenFader _screenFader = null;

    private async void Awake()
    {
        _screenFader.Fade(0);
        await _sceneEventChannel.RequestSceneLoadAsync(_sceneTwo, false);
    }

    //private async void Start()
    //{
    //    await _sceneEventChannel.RequestSceneLoadAsync(_sceneOne, false);

    //    await Task.Delay(3 * 1000);

    //    await _sceneEventChannel.RequestSceneLoadAsync(_sceneTwo, false);
    //}

    private void OnEnable()
    {
        _sceneEventChannel.OnBeforeSceneUnloadAsync += FadeIn;
        _sceneEventChannel.OnAfterLoadingSceneLoadAsync += FadeOut;
        _sceneEventChannel.OnBeforeLoadingSceneUnloadAsync += FadeIn;
        _sceneEventChannel.OnAfterSceneLoadAsync += FadeOut;
    }

    private void OnDisable()
    {
        _sceneEventChannel.OnBeforeSceneUnloadAsync -= FadeIn;
        _sceneEventChannel.OnAfterLoadingSceneLoadAsync -= FadeOut;
        _sceneEventChannel.OnBeforeLoadingSceneUnloadAsync -= FadeIn;
        _sceneEventChannel.OnAfterSceneLoadAsync -= FadeOut;
    }

    private async Task FadeIn(AddressableScenePreset preset)
    {
        await _screenFader.FadeAsync(1);
    }

    private async Task FadeOut(AddressableScenePreset preset)
    {
        await _screenFader.FadeAsync(0);
    }
}