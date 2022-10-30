using System.Collections;
using System.Collections.Generic;
using CollieMollie.System;
using UnityEngine;

public class SampleSceneAssetTransition : MonoBehaviour
{
    [SerializeField] private SceneAssetEventChannel sceneEventChannel = null;
    [SerializeField] private SceneAssetPreset targetScene = null;

    private void Start()
    {
        sceneEventChannel.RaiseSceneLoadEvent(targetScene, true);
    }
}
