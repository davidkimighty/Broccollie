using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "SamplePlayerData", menuName = "Sample/SamplePlayerData")]
public class SamplePlayerData : ScriptableObject
{
    public string Name = null;
}
