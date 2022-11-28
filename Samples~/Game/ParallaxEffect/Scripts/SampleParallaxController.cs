using System.Collections;
using System.Collections.Generic;
using CollieMollie.Game;
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

    private void Awake()
    {
        _buttonLeft.onClick.AddListener(() => _parallaxController.StartParallax(-1, _duration));
        _buttonRight.onClick.AddListener(() => _parallaxController.StartParallax(+1, _duration));
        _buttonStart.onClick.AddListener(() => _parallaxController.StartParallaxLoop(+1));
        _buttonStop.onClick.AddListener(() => _parallaxController.StopParallax());

    }
}
