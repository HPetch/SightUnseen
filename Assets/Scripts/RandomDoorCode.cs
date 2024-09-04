using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using HurricaneVR.TechDemo.Scripts;

public class RandomDoorCode : MonoBehaviour
{
    [SerializeField] private bool hardCodedDoor = false;
    [SerializeField] [Tooltip("If hardCodedDoor is true, this code will be used - otherwise, it will be randomised on game start")] private string code;
    [SerializeField] private TMP_Text passwordText;
    [SerializeField] private DemoKeypad keypad;

    // Start is called before the first frame update
    void Start()
    {
        if (!hardCodedDoor)
        {
            generateCode();
            keypad.Code = code;
            passwordText.text = code;
            
        }
    }
    private void generateCode()
    {
        int intCode = Random.Range(1000, 10000);

        //Refresh if we generate an easy to guess code
        switch (intCode)
        {
            case 1000:
                generateCode();
                break;
            case 2000:
                generateCode();
                break;
            case 3000:
                generateCode();
                break;
            case 4000:
                generateCode();
                break;
            case 5000:
                generateCode();
                break;
            case 6000:
                generateCode();
                break;
            case 7000:
                generateCode();
                break;
            case 8000:
                generateCode();
                break;
            case 9000:
                generateCode();
                break;
            case 1234:
                generateCode();
                break;
            case 2345:
                generateCode();
                break;
            case 4567:
                generateCode();
                break;
            case 5678:
                generateCode();
                break;
            case 6789:
                generateCode();
                break;
            case 9876:
                generateCode();
                break;
            case 8765:
                generateCode();
                break;
            case 7654:
                generateCode();
                break;
            case 6543:
                generateCode();
                break;
            case 5432:
                generateCode();
                break;
            case 4321:
                generateCode();
                break;
            case 1488:
                //No dogwhistles allowed
                generateCode();
                break;
            case 8814:
                generateCode();
                break;
            default: //None of the above cases happen - good! Carry on
                break;
        }
        code = intCode.ToString();
    }
}
