using System.Threading.Tasks;
using CollieMollie.System;
using UnityEngine;

public class SampleTransistion : MonoBehaviour
{
    [SerializeField] private SceneAddressableLoader _sceneAddressableLoader = null;
    [SerializeField] private SceneAddressablePreset _sceneOne = null;
    [SerializeField] private SceneAddressablePreset _sceneTwo = null;

    [SerializeField] private ScreenFader _screenFader = null;

    private void OnEnable()
    {
        _sceneAddressableLoader.OnBeforeSceneUnload += FadeIn;
        _sceneAddressableLoader.OnAfterSceneLoad += FadeOut;
    }

    private void OnDisable()
    {
        _sceneAddressableLoader.OnBeforeSceneUnload -= FadeIn;
        _sceneAddressableLoader.OnAfterSceneLoad -= FadeOut;
    }

    private async void Start()
    {
        await _sceneAddressableLoader.LoadNewSceneAsync(_sceneOne);

        await Task.Delay(3 * 1000);

        await _sceneAddressableLoader.UnloadActiveSceneAsync(true);

        await Task.Delay(3 * 1000);

        await _sceneAddressableLoader.LoadNewSceneAsync(_sceneTwo);
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