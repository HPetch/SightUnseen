using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public bool rightHanded { get; private set; }
    public bool rightEyeSelected { get; private set; }

    public static SettingsManager Instance;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        isRightEye(true); //Default settings for debug purposes - this should not override as awake won't trigger multiple times on DDOL object.
        isRightHanded(true);
    }

    public void isRightHanded(bool val)
    {
        rightHanded = val;
    }

    public void isRightEye(bool val)
    {
        rightEyeSelected = val;
    }
}
