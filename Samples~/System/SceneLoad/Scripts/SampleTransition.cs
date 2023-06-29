using System.Threading.Tasks;
using Broccollie.System;
using UnityEngine;

public class SampleTransition : MonoBehaviour
{
    [SerializeField] private SceneAddressableEventChannel _sceneEventChannel = null;
    [SerializeField] private SceneAddressablePreset _sceneOne = null;
    [SerializeField] private SceneAddressablePreset _sceneTwo = null;

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

    private async Task FadeIn()
    {
        await _screenFader.FadeAsync(1);
    }

    private async Task FadeOut()
    {
        await _screenFader.FadeAsync(0);
    }
}