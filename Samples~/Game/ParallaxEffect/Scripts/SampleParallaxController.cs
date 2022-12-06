using System.Collections;
using System.Collections.Generic;
using CollieMollie.Core;
using CollieMollie.Game;
using CollieMollie.UI;
using UnityEngine;
using UnityEngine.UI;

public class SampleParallaxController : MonoBehaviour
{
    [SerializeField] private ParallaxEffectController _parallaxController = null;
    [SerializeField] private Button _buttonLeft = null;
    [SerializeField] private Button _buttonRight = null;
    [SerializeField] private Button _buttonStart = null;
    [SerializeField] private Button _buttonStop = null;
    [SerializeField] private float _duration = 3f;

    [SerializeField] private UIPanel _panel = null;

    [SerializeField] private CameraEffect _shakeEffect = null;
    [SerializeField] private CameraEffectEventChannel _eventChannel = null;

    private void Awake()
    {

        _buttonLeft.onClick.AddListener(() => _parallaxController.StartParallax(-1, _duration));
        _buttonRight.onClick.AddListener(() => _parallaxController.StartParallax(+1, _duration));
        _buttonStart.onClick.AddListener(() => _parallaxController.StartParallaxLoop(+1));
        _buttonStop.onClick.AddListener(() => _parallaxController.StopParallax());

        _buttonLeft.onClick.AddListener(() => _panel.ChangeInteractionState(UIInteractionState.Selected));
        _buttonRight.onClick.AddListener(() => _panel.ChangeInteractionState(UIInteractionState.Selected));
        _parallaxController.OnEndParallax += () => _panel.ChangeState(UIState.Default);

        _buttonLeft.onClick.AddListener(() => _eventChannel.RaisePlayCameraEffectEvent(_shakeEffect));
        _buttonRight.onClick.AddListener(() => _eventChannel.RaisePlayCameraEffectEvent(_shakeEffect));
    }

    private void Start()
    {
        _panel.ChangeState(UIState.Default);
        
    }
}
