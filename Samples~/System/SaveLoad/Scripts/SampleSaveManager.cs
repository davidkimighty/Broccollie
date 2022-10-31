using System;
using System.Collections;
using System.Collections.Generic;
using CollieMollie.Helper;
using TMPro;
using UnityEngine;

public class SampleSaveManager : MonoBehaviour
{
    private const string AESKEY = "KEY";
    private const string SAVEFOLDER = "/Saved/";
    private const string SAVEFILENAME = "TestData.json";
    private static string s_savePath = null;

    [SerializeField] private SamplePlayerData _playerData = null;
    [SerializeField] private TextMeshProUGUI _nameText = null;

    private IEnumerator _saveAction = null;

    private void Awake()
    {
        s_savePath = Application.persistentDataPath + SAVEFOLDER + SAVEFILENAME;
    }

    public void Save()
    {
        Helper.SaveDataEncrypt(s_savePath, AESKEY, _playerData);
        Debug.Log($"[SampleSaveManager] Saved path: {s_savePath}");
    }

    public void Load()
    {
        Helper.LoadDataDecrypt(Application.persistentDataPath + SAVEFOLDER, SAVEFILENAME, AESKEY, _playerData);
        _nameText.text = _playerData.Name;
    }
}
