using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using CollieMollie.System;
using UnityEngine;

public class SampleSceneAssetTransition : MonoBehaviour
{
    [SerializeField] private SceneAssetLoader _sceneAssetLoader = null;
    [SerializeField] private SceneAssetPreset targetSceneOne = null;
    [SerializeField] private SceneAssetPreset targetSceneTwo = null;

    private async void Start()
    {
        await _sceneAssetLoader.LoadNewScene(targetSceneOne);

        await Task.Delay(3 * 1000);

        await _sceneAssetLoader.UnloadActiveScene(true);

        await Task.Delay(3 * 1000);

        await _sceneAssetLoader.LoadNewScene(targetSceneTwo);
    }
}
