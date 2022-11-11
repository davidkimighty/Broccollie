using System;
using System.Collections;
using System.Collections.Generic;
using CollieMollie.Audio;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine;

public class SamplePlayAudio : MonoBehaviour
{
    private const string SFX = "SFXVolume";
    private const string AMBIENT = "AmbientVolume";

    [SerializeField] private AudioMixer _masterAudioMixer = null;

    [SerializeField] private Slider _ambientSlider = null;

    [SerializeField] private AudioEventChannel _eventChannel = null;
    [SerializeField] private AudioPreset _backgroundAudioPreset = null;
    [SerializeField] private AudioPreset _buttonAudioPreset = null;

    private void Start()
    {
        _eventChannel.RaisePlayAudioEvent(_backgroundAudioPreset);
    }

    public void PlayButtonAudio()
    {
        _eventChannel.RaisePlayAudioEvent(_buttonAudioPreset);
    }

    public void ChangeVolume(float value)
    {
        _ambientSlider.value = value;
        _masterAudioMixer.SetFloat(AMBIENT, Mathf.Log10(value) * 20);
    }
}
