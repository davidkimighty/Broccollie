using System;
using System.Collections;
using System.Collections.Generic;
using Broccollie.System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "SampleGameData", menuName = "Sample/SampleGameData")]
public class SampleGameData : ScriptableObject
{
    public string Name = null;
    public SerializableDictionary<string, Vector3> CubesPosition = new SerializableDictionary<string, Vector3>();
    public SerializableDictionary<string, Color> CubesColor = new SerializableDictionary<string, Color>();
}
