using System.Collections;
using System.Collections.Generic;
using Broccollie.System;
using UnityEngine;

public class SampleSaveColor : MonoBehaviour, ISaveable
{
    [SerializeField] private LocalSaveableEntity _saveableEntity = null;
    [SerializeField] private Renderer _renderer = null;

    public void LoadState(object state)
    {
        SampleGameData gameData = state as SampleGameData;
        if (gameData.CubesColor.TryGetValue(_saveableEntity.UniqueId, out Color color))
        {
            _renderer.material.color = color;
        }
    }

    public void SaveState(object state)
    {
        SampleGameData gameData = state as SampleGameData;
        if (gameData.CubesColor.ContainsKey(_saveableEntity.UniqueId))
        {
            gameData.CubesColor[_saveableEntity.UniqueId] = _renderer.material.color;
        }
        else
        {
            gameData.CubesColor.Add(_saveableEntity.UniqueId, _renderer.material.color);
        }
    }
}
