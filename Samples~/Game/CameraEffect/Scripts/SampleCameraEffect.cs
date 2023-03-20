using System.Collections;
using System.Collections.Generic;
using Broccollie.Game;
using UnityEngine;
using UnityEngine.UI;

public class SampleCameraEffect : MonoBehaviour
{
    [SerializeField] private CameraEffectEventChannel _eventChannel = null;
    [SerializeReference] private CameraEffectPreset _shakeEffect = null;

    [SerializeField] private Button _shakeButton = null;

    private void Awake()
    {
        _shakeButton.onClick.AddListener(() => _eventChannel.RaisePlayCameraEffectEvent(_shakeEffect));
    }
}
