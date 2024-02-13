using System.Collections;
using System.Collections.Generic;
using Broccollie.Core;
using UnityEngine;

public class SampleLogger : MonoBehaviour
{
    private void Start()
    {
        Helper.Log("This is Log", Helper.Broccollie, this);
        Helper.LogWarning("This is LogWarning", Helper.Broccollie, this);
        Helper.LogError("This is LogError", Helper.Broccollie, this);
    }
}
