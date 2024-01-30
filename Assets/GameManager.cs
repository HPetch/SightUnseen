using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private bool CybervisionOn = true;
    public bool isRightEye = true;
    public Camera leftEye;
    public Camera rightEye;
    private List<Camera> currentCybereyes = new List<Camera>();
    public LayerMask cyberMask;
    public LayerMask normalMask;
    public TMP_Dropdown eyeChoice;
    public Toggle isDouble;

    public static GameManager Instance;

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(this.gameObject);

        //Auto bind cybervision immediately
        refreshCybereye();
    }

    public void ToggleDoubleCybereyes()
    {
        //Clear cybereyes array
        currentCybereyes.Clear();

        //If this should be double, add both cameras to be targetted by currentCybereyes.
        if (isDouble.isOn)
        {
            currentCybereyes.Add(rightEye);
            currentCybereyes.Add(leftEye);
            Debug.Log("1");
        }
        else
        {
            refreshCybereye();
            Debug.Log("2");
        }
    }

    //Activates whenever the option is changed in the settings for the cybervision eye, updates it immediately
    public void eyeChoiceToggle()
    {
        switch (eyeChoice.value)
        {
            //0 = Right, 1 = Left eyes
            case 0:
                isRightEye = true;
                break;
            case 1:
                isRightEye = false;
                break;
            default:
                break;
        }
        refreshCybereye();
    }

    private void refreshCybereye()
    {
        currentCybereyes.Clear();
        //If right eye is currently the cybereye, make sure it's reset 
        if (isRightEye) currentCybereyes.Add(rightEye); else currentCybereyes.Add(leftEye);
    }

    public void ToggleCybervision()
    {
        //If Cybervision is already on, turn it off from whichever eye(s) have cybervision, and vice versa if off.
        if (CybervisionOn)
        {
            SetCybervisionState(false);
        }
        else
        {
            SetCybervisionState(true);
        }
    }

    private void SetCybervisionState(bool value)
    {
        Debug.Log("3");
        //If we want cybervision on, turn on cybervision
        if (value == true)
        {
            Debug.Log("4");
            foreach (Camera cam in currentCybereyes)
            {
                cam.cullingMask = cyberMask;
            }
            CybervisionOn = true;
        }
        else
        {
            Debug.Log("5");
            //If we want cybervision off, turn it off.
            foreach (Camera cam in currentCybereyes)
            {
                cam.cullingMask = normalMask;
            }
            CybervisionOn = false;
        }
    }
}