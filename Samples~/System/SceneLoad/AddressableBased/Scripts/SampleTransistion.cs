using System.Collections;
using System.Collections.Generic;
using CollieMollie.System;
using UnityEngine;

public class SampleTransistion : MonoBehaviour
{
    [SerializeField] private SceneAddressableEventChannel sceneEventChannel = null;
    [SerializeField] private SceneAddressablePreset sceneOne = null;

    private void OnEnable()
    {
        sceneEventChannel.OnBeforeSceneUnload += FadeIn;
        sceneEventChannel.OnAfterSceneLoad += FadeOut;
    }

    private void OnDisable()
    {
        sceneEventChannel.OnBeforeSceneUnload -= FadeIn;
        sceneEventChannel.OnAfterSceneLoad -= FadeOut;
    }

    private void Start()
    {
        sceneEventChannel.RaiseSceneLoadEvent(sceneOne, true);
    }

    private void FadeIn()
    {
        // Screen fade in
    }

    private void FadeOut()
    {
        // Screen fade out
    }
}