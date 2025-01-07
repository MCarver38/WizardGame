using System;
using UnityEngine;

public class FPSCap : MonoBehaviour
{
    private int targetFPS = 200;

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = targetFPS;
    }
}
