using System.Collections;
using System.Collections.Generic;
using Broccollie.System;
using UnityEngine;

public class SampleSavePosition : MonoBehaviour, ISaveable
{
    [SerializeField] private LocalSaveableEntity _saveableEntity = null;

    public void LoadState(object state)
    {
        if (state == null) return;

        SampleGameData gameData = state as SampleGameData;
        if (gameData.CubesPosition.TryGetValue(_saveableEntity.UniqueId, out Vector3 position))
        {
            transform.position = position;
        }
    }

    public void SaveState(object state)
    {
        if (state == null) return;

        SampleGameData gameData = state as SampleGameData;
        if (gameData.CubesPosition.ContainsKey(_saveableEntity.UniqueId))
        {
            gameData.CubesPosition[_saveableEntity.UniqueId] = transform.position;
        }
        else
        {
            gameData.CubesPosition.Add(_saveableEntity.UniqueId, transform.position);
        }
    }
}
