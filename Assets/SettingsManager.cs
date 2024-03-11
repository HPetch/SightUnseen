using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public bool rightHanded { get; private set; }
    public bool rightEyeSelected { get; private set; }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
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
