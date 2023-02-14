using System.IO;
using System.Threading;
using System.Threading.Tasks;
using CollieMollie.System;
using UnityEngine;

public class SampleSaveManager : MonoBehaviour
{
    private const string Aeskey = "SAMPLEKEY";
    private const string SaveFolder = "Saved";
    private const string SaveFileName = "TestData.json";
    private static string s_savePath = null;

    [SerializeField] private SaveLoadController _saveLoadController = null;
    [SerializeField] private SampleGameData _playerData = null;

    private SaveOptions _saveOptions;
    private CancellationTokenSource _cts = null;

    private void Awake()
    {
        s_savePath = Path.Combine(Application.persistentDataPath, SaveFolder);
        _saveOptions = new SaveOptions()
        {
            SaveDirectory = s_savePath,
            SaveFileName = SaveFileName,
            AesKey = Aeskey
        };
        _cts = new CancellationTokenSource();
    }

    [ContextMenu("Execute Save")]
    public void Save()
    {
        _cts.Cancel();
        _cts = new CancellationTokenSource();

        Task save = _saveLoadController.SaveAsync(_playerData, _saveOptions, _cts.Token, () =>
        {
            Debug.Log($"[SampleSaveManager] Saved path: {s_savePath}");
        });
    }

    [ContextMenu("Execute Load")]
    public void Load()
    {
        _cts.Cancel();
        _cts = new CancellationTokenSource();

        Task load = _saveLoadController.LoadAsync(_playerData, _saveOptions, _cts.Token, () =>
        {
            Debug.Log($"[SampleSaveManager] Data loaded.");
        });
    }
}
