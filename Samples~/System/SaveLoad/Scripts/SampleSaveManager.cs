using System.Collections;
using System.IO;
using CollieMollie.System;
using TMPro;
using UnityEngine;

public class SampleSaveManager : MonoBehaviour
{
    private const string AESKEY = "KEY";
    private const string SAVEFOLDER = "Saved";
    private const string SAVEFILENAME = "TestData.json";
    private static string s_savePath = null;

    [SerializeField] private SaveLoadController _saveController = null;
    [SerializeField] private SamplePlayerData _playerData = null;
    [SerializeField] private TextMeshProUGUI _nameText = null;

    private IEnumerator _saveAction = null;

    private void Awake()
    {
        s_savePath = Path.Combine(Application.persistentDataPath, SAVEFOLDER);
    }

    public void Save()
    {
        if (_saveAction != null)
            StopCoroutine(_saveAction);
        _saveAction = _saveController.SaveDataEncrypt(s_savePath, SAVEFILENAME, AESKEY, _playerData);
        StartCoroutine(_saveAction);
        Debug.Log($"[SampleSaveManager] Saved path: {s_savePath}");
    }

    public void Load()
    {
        if (_saveAction != null)
            StopCoroutine(_saveAction);
        _saveAction = _saveController.LoadDataDecrypt(s_savePath, SAVEFILENAME, AESKEY, _playerData, () =>
        {
            _nameText.text = _playerData.Name;
        });
        StartCoroutine(_saveAction);
    }
}
