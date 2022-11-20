using System.Collections;
using System.Collections.Generic;
using System.IO;
using CollieMollie.System;
using TMPro;
using UnityEngine;

public class SampleSaveManager : MonoBehaviour
{
    private const string AESKEY = "SAMPLEKEY";
    private const string SAVEFOLDER = "Saved";
    private const string SAVEFILENAME = "TestData.json";
    private static string s_savePath = null;

    [SerializeField] private SaveLoadController _saveController = null;
    [SerializeField] private SampleGameData _playerData = null;

    private IEnumerator _saveAction = null;

    private void Awake()
    {
        s_savePath = Path.Combine(Application.persistentDataPath, SAVEFOLDER);
    }

    [ContextMenu("Execute Save")]
    public void Save()
    {
        if (_saveAction != null)
            StopCoroutine(_saveAction);

        foreach (SaveableEntity saveable in FindObjectsOfType<SaveableEntity>())
        {
            saveable.SaveStates();
        }

        _saveAction = _saveController.SaveData(s_savePath, SAVEFILENAME, _playerData, AESKEY);
        StartCoroutine(_saveAction);
        Debug.Log($"[SampleSaveManager] Saved path: {s_savePath}");
    }

    [ContextMenu("Execute Load")]
    public void Load()
    {
        if (_saveAction != null)
            StopCoroutine(_saveAction);

        _saveAction = _saveController.LoadData(s_savePath, SAVEFILENAME, _playerData, AESKEY, () =>
        {
            foreach (SaveableEntity saveable in FindObjectsOfType<SaveableEntity>())
            {
                saveable.LoadStates();
            }
        });
        StartCoroutine(_saveAction);
    }
}
