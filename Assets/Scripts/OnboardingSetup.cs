using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnboardingSetup : MonoBehaviour
{
    public SettingsSO settings;
    //// Start is called before the first frame update
    //void Start()
    //{
        
    //}

    //// Update is called once per frame
    //void Update()
    //{
        
    //}

    public void HandChoice(bool rightHand)
    {
        if (rightHand)
        {
            settings.rightHanded = true;
        }
        else
        {
            settings.rightHanded = false;
        }
    }
    public void EyeChoice(bool rightEye)
    {
        if (rightEye)
        {
            settings.rightEye = true;
        }
        else
        {
            settings.rightEye = false;
        }
    }
    public void GlassesChoice(bool enabled)
    {
        if (enabled)
        {
            settings.glassesOn = true;
        }
        else
        {
            settings.glassesOn = false;
        }
    }
    public void SmoothChoice(bool enabled)
    {
        if (enabled)
        {
            settings.smoothMovement = true;
        }
        else
        {
            settings.smoothMovement = false;
        }
    }
}
