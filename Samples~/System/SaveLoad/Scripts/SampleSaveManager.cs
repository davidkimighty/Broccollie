using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using CollieMollie.System;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class SampleSaveManager : MonoBehaviour
{
    private const string Aeskey = "SAMPLEKEY";
    private const string SaveFolder = "Saved";
    private const string SaveFileName = "TestData.json";
    private static string s_savePath = null;

    [SerializeField] private SaveLoadEventChannel _saveEventChannel = null;
    [SerializeField] private SampleGameData _playerData = null;

    private SaveOptions _saveOptions;
    
    private void Awake()
    {
        s_savePath = Path.Combine(Application.persistentDataPath, SaveFolder);
        _saveOptions = new SaveOptions()
        {
            SaveDirectory = s_savePath,
            SaveFileName = SaveFileName,
            AesKey = Aeskey
        };
    }

    [ContextMenu("Execute Save")]
    public void Save()
    {
        _saveEventChannel.RaiseSaveEvent(_playerData, _saveOptions, () =>
        {
            Debug.Log($"[SampleSaveManager] Saved path: {s_savePath}");
        });
    }

    [ContextMenu("Execute Load")]
    public void Load()
    {
        _saveEventChannel.RaiseLoadEvent(_playerData, _saveOptions, () =>
        {
            Debug.Log($"[SampleSaveManager] Data loaded.");
        });
    }
}
