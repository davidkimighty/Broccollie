using System.Collections;
using System.Collections.Generic;
using CollieMollie.Core;
using SharedLibrary;
using UnityEngine;

public class SampleNetworkManager : MonoBehaviour
{
    private async void Start()
    {
        NetworkData data = await Helper.Get<NetworkData>("https://localhost:7219/Network/?id=13");
    }
}
