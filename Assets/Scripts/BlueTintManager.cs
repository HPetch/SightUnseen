using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using TMPro;

public class BlueTintManager : MonoBehaviour
{
    //Based on the theory of Jeppe Nymand https://electronicepileptic.wordpress.com/2023/06/15/blue-tinting-to-avoid-visual-triggers/ 

    public enum BLUELIGHT_STRENGTH
    {
        Off, Low, Medium, High, Max //0f, 0.25f, 0.5f, 0.75f, 1f - these should all be toggleable options
    }
    private BLUELIGHT_STRENGTH currentStrength;
    private PostProcessVolume volume;
    [SerializeField] private TMP_Dropdown dropdown;

    void Start()
    {
        //Disable post-processing instance until needed
        volume = GetComponent<PostProcessVolume>();
        volume.enabled = false;
    }

    //Trigger this on dropdown value changed
    public void updateStrength()
    {
        switch (dropdown.value)
        {
            case 0:
                setStrength(BLUELIGHT_STRENGTH.Off);
                break;
            case 1:
                setStrength(BLUELIGHT_STRENGTH.Low);
                break;
            case 2:
                setStrength(BLUELIGHT_STRENGTH.Medium);
                break;
            case 3:
                setStrength(BLUELIGHT_STRENGTH.High);
                break;
            case 4:
                setStrength(BLUELIGHT_STRENGTH.Max);
                break;
            default:
                Debug.LogError("Blue-tint dropdown set to invalid value!");
                setStrength(BLUELIGHT_STRENGTH.Off);
                break;
        }
    }

    //Set blue light strength if called
    private void setStrength(BLUELIGHT_STRENGTH newStrength)
    {
        currentStrength = newStrength;

        //Adjust weight of volume based on strength value to be more blue.
        switch (currentStrength)
        {
            case BLUELIGHT_STRENGTH.Off:
                volume.enabled = false;
                volume.weight = 0f;
                break;
            case BLUELIGHT_STRENGTH.Low:
                volume.enabled = true;
                volume.weight = 0.25f;
                break;
            case BLUELIGHT_STRENGTH.Medium:
                volume.enabled = true;
                volume.weight = 0.5f;
                break;
            case BLUELIGHT_STRENGTH.High:
                volume.enabled = true;
                volume.weight = 0.75f;
                break;
            case BLUELIGHT_STRENGTH.Max:
                volume.enabled = true;
                volume.weight = 1f;
                break;
            default:
                Debug.LogWarning("Somehow, the blue light strength is wrong in BlueTintManager");
                break;
        }
    }
}
