using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Energy : MonoBehaviour
{
    public Asset asset;
    void Start()
    {
        asset.RestoreEnergyFromOffline();
    }

        void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus) // Ứng dụng mất tiêu điểm
        {
            asset.SaveLastPlayedTime();
        }
    }

    void OnApplicationPause(bool isPaused)
    {
        if (isPaused) // Ứng dụng bị tạm dừng
        {
            asset.SaveLastPlayedTime();
        }
    }

    void OnApplicationQuit()
    {
        asset.SaveLastPlayedTime();
    }
}
