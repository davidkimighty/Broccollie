using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine;
using Broccollie.Audio;

public class SamplePlayAudio : MonoBehaviour
{
    private const string SFX = "SFXVolume";
    private const string AMBIENT = "AmbientVolume";

    [SerializeField] private AudioMixer _masterAudioMixer = null;

    [SerializeField] private Slider _ambientSlider = null;

    [SerializeField] private AudioEventChannel _eventChannel = null;
    [SerializeField] private AudioPreset _loopAudioPreset = null;
    [SerializeField] private AudioPreset _buttonAudioPreset = null;

    private void Start()
    {
        _eventChannel.RequestPlayAudio(_loopAudioPreset);
    }

    public void PlayButtonAudio()
    {
        _eventChannel.RequestPlayAudio(_buttonAudioPreset);
    }

    public void PlayLoopAudio()
    {
        _eventChannel.RequestPlayAudio(_loopAudioPreset);
    }

    public void PauseLoopAudio()
    {
        _eventChannel.RequestPauseAudio(_loopAudioPreset);
    }

    public void StopLoopAudio()
    {
        _eventChannel.RequestStopAudio(_loopAudioPreset);
    }

    public void ChangeVolume(float value)
    {
        _ambientSlider.value = value;
        _masterAudioMixer.SetFloat(AMBIENT, Mathf.Log10(value) * 20);
    }
}
