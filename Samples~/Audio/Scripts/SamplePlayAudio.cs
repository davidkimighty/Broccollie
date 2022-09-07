using System.Collections;
using System.Collections.Generic;
using CollieMollie.Audio;
using UnityEngine;

public class SamplePlayAudio : MonoBehaviour
{
    [SerializeField] private AudioEventChannel eventChannel = null;
    [SerializeField] private AudioPreset preset = null;

    public void PlaySampleAudio()
    {
        eventChannel.RaisePlayAudioEvent(preset);
    }
}
