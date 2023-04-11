using Broccollie.System;
using UnityEngine;

public class SampleSaveManager : MonoBehaviour
{
    [SerializeField] private LocalSaveEventChannel _saveEventChannel = null;

    [ContextMenu("Execute Save")]
    public async void Save()
    {
        await _saveEventChannel.RequestSaveAsync();
    }

    [ContextMenu("Execute Load")]
    public async void Load()
    {
        await _saveEventChannel.RequestLoadAsync();
    }
}
