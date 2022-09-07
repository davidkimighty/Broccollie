using System.Collections;
using System.Collections.Generic;
using CollieMollie.System;
using UnityEngine;

public class SampleTransistion : MonoBehaviour
{
    [SerializeField] private SceneEventChannel sceneEventChannel = null;
    [SerializeField] private ScenePreset sceneOne = null;

    private void Start()
    {
        sceneEventChannel.RaiseSceneLoadEvent(sceneOne, true);
    }
}