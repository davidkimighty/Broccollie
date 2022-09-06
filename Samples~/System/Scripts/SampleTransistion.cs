using System.Collections;
using System.Collections.Generic;
using CollieMollie.System;
using UnityEngine;

namespace CollieMollie.Sample
{
    public class SampleTransistion : MonoBehaviour
    {
        [SerializeField] private EventChannel sceneEventChannel = null;
        [SerializeField] private ScenePreset sceneOne = null;

        private void Start()
        {
            sceneEventChannel.RaiseEvent(sceneOne, true);
        }
    }
}
