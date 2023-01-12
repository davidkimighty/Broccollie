using System.Collections;
using System.Collections.Generic;
using CollieMollie.System;
using UnityEngine;

public class SampleSceneAssetTransition : MonoBehaviour
{
    [SerializeField] private SceneAssetEventChannel sceneEventChannel = null;
    [SerializeField] private SceneAssetPreset targetSceneOne = null;
    [SerializeField] private SceneAssetPreset targetSceneTwo = null;

    private IEnumerator Start()
    {
        sceneEventChannel.RaiseSceneLoadEvent(targetSceneOne, false);

        yield return new WaitForSeconds(3f);

        sceneEventChannel.RaiseSceneLoadEvent(targetSceneTwo, true);
    }
}
