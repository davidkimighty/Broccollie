using System.Collections;
using System.Collections.Generic;
using Broccollie.System;
using UnityEngine;
using UnityEngine.UI;

public class SampleSaveText : MonoBehaviour, ISaveable
{
    [SerializeField] private Text _nameText = null;

    public void LoadState(object state)
    {
        if (state == null) return;
        _nameText.text = (state as SampleGameData).Name;
    }

    public void SaveState(object state)
    {
        if (state == null) return;
        (state as SampleGameData).Name = _nameText.text;
    }
}
