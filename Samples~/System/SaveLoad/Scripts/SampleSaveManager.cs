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

    [SerializeField] private LocalSaveController _saveLoadController = null;
    [SerializeField] private LocalSaveOptionsPreset _saveOptions = null;

    private void Awake()
    {
        s_savePath = Path.Combine(Application.persistentDataPath, SaveFolder);
    }

    [ContextMenu("Execute Save")]
    public void Save()
    {
        Task save = _saveLoadController.SaveAsync(() =>
        {
            Debug.Log($"[SampleSaveManager] Saved path: {s_savePath}");
        });
    }

    [ContextMenu("Execute Load")]
    public void Load()
    {
        Task load = _saveLoadController.LoadAsync(() =>
        {
            Debug.Log($"[SampleSaveManager] Data loaded.");
        });
    }
}
