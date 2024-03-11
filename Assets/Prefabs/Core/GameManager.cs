using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Global settings")]
    [SerializeField] private bool isDontDestroyOnLoad = false;
    [SerializeField] private bool CybervisionOn = true;
    public bool isRightEye = true;
    public TMP_Dropdown eyeChoice;
    public Toggle isDouble;
    private List<Camera> currentCybereyes = new List<Camera>();
    [SerializeField] private Color EMPColour;
    [SerializeField] private Color flashColour;

    //Set up Default and Cybereye culling masks correctly in Inspector
    //You can't add mask layers in code in a more user-friendly way.
    //Each eye has an Override to display stuff on the HMD no matter what (i.e. flashbang)
    [Header("Left Eye")]
    public Camera leftEye;
    //[SerializeField] private GameObject LeftEyepatchCanvas;
    [SerializeField] private LayerMask leftDefaultMask;
    [SerializeField] private LayerMask leftCybereyeMask;

    [Header("Right Eye")]
    public Camera rightEye;
    //[SerializeField] private GameObject RightEyepatchCanvas;
    [SerializeField] private LayerMask rightDefaultMask;
    [SerializeField] private LayerMask rightCybereyeMask;


    [Header("Detached eye throwable")]
    public GameObject detachedEyePrefab;
    private Canvas eyepatchCanvas;
    [SerializeField] private Color eyepatchColour;

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

        if ((leftEye != null) && (rightEye != null))
        {
            SettingsManager initialSettings = GameObject.FindGameObjectWithTag("Settings").GetComponent<SettingsManager>();
            isRightEye = initialSettings.rightEyeSelected;

            if (isRightEye) eyeChoice.value = 0; else eyeChoice.value = 1;
            //Left handed gun should be swapped in instead and be tracked as a variable here.
        }

        if (isDontDestroyOnLoad) DontDestroyOnLoad(this.gameObject);

        //Auto bind cybervision immediately
        refreshCybereye();

        //Get eyepatch canvas for detached eye.
        eyepatchCanvas = detachedEyePrefab.GetComponentInChildren<Canvas>();
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
        UpdateDetachedEyeTarget();
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
        UpdateDetachedEyeTarget();
    }

    private void refreshCybereye()
    {
        //Instead of CLEARING the eye references, just attempt to remove both instances of the HMD eyes for now.
        if (currentCybereyes.Contains(rightEye)) currentCybereyes.Remove(rightEye);
        if (currentCybereyes.Contains(leftEye)) currentCybereyes.Remove(leftEye);

        //If right eye is currently the cybereye, make sure it's reset 
        if (isRightEye) currentCybereyes.Add(rightEye); else currentCybereyes.Add(leftEye);

        //Add detached eye prefab, too.
        currentCybereyes.Add(detachedEyePrefab.GetComponentInChildren<Camera>());

        UpdateDetachedEyeTarget();
        
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
                //We should know which eye this is affecting, and thus which culling masks to set for the current eyes
                if (isRightEye) cam.cullingMask = rightCybereyeMask; else cam.cullingMask = leftCybereyeMask;
            }
            CybervisionOn = true;
        }
        else
        {
            Debug.Log("5");
            //If we want cybervision off, turn it off.
            foreach (Camera cam in currentCybereyes)
            {
                //We should know which eye this is affecting, and thus which culling masks to set for the current eyes
                if (isRightEye) cam.cullingMask = rightDefaultMask; else cam.cullingMask = leftDefaultMask;
            }
            CybervisionOn = false;
        }
    }

    public void doDetachedVision()
    {
        //Disable all cameras
        foreach (Camera eye in currentCybereyes)
        {
            eye.enabled = false;
        }
        //Re-enable the detached eye camera
        if (currentCybereyes.Contains(detachedEyePrefab.GetComponentInChildren<Camera>()))
            detachedEyePrefab.GetComponentInChildren<Camera>().enabled = true;
    }

    public void doAttachedVision()
    {
        //Disable all cameras
        foreach (Camera eye in currentCybereyes)
        {
            eye.enabled = true;
        }
        //Disable the detached eye camera
        if (currentCybereyes.Contains(detachedEyePrefab.GetComponentInChildren<Camera>()))
            detachedEyePrefab.GetComponentInChildren<Camera>().enabled = false;
    }

    public void UpdateDetachedEyeTarget()
    {
        if (!isDouble.isOn) {
            //If right eye then the target eye must be right eye, if left eye, left eye.
            if (isRightEye)
                detachedEyePrefab.GetComponentInChildren<Camera>().stereoTargetEye = StereoTargetEyeMask.Right;
            else detachedEyePrefab.GetComponentInChildren<Camera>().stereoTargetEye = StereoTargetEyeMask.Left;
        }
        else
        {
            //Otherwise, it'll be in both eyes.
            detachedEyePrefab.GetComponentInChildren<Camera>().stereoTargetEye = StereoTargetEyeMask.Both;
        }
        foreach (Camera cam in currentCybereyes)
        {
            Debug.LogWarning(cam);
        }
        
    }

    public void hideViewport(string eye, bool doHide)
    {
        eyepatchCanvas.GetComponent<Animator>().SetBool("isHiding", doHide);
        /*
        if (eye.ToLower() == "right")
        {
            //Hide or show the right eye
            RightEyepatchCanvas.GetComponent<Animator>().SetBool("isHiding", doHide);
        }
        else if (eye.ToLower() == "left")
        {
            LeftEyepatchCanvas.GetComponent<Animator>().SetBool("isHiding", doHide);
        }
        else if (eye.ToLower() == "cyber")
        {
            if (isRightEye) RightEyepatchCanvas.GetComponent<Animator>().SetBool("isHiding", doHide);
            else LeftEyepatchCanvas.GetComponent<Animator>().SetBool("isHiding", doHide);
        } else 
            Debug.LogError("Hide viewport eye is invalid, is '" + eye + "' a typo?");
        */
    }

    public Camera getActiveCyberCamera()
    {
        Camera returnCam = null;
        foreach (Camera cam in currentCybereyes)
        {
            if ((cam.enabled == true) && (returnCam == null))
            {
                returnCam = cam;
                Debug.Log(cam);
            }

            if (returnCam == null)
            {
                Debug.LogWarning("Could not find a cybereye camera that is active? Does your viewport currently work?");
            }
        }
        return returnCam;
    }

    public Camera getActiveNormalCamera()
    {
        if (!isDouble.isOn)
        {
            if (isRightEye)
            {
                return leftEye;
            }
            else
            {
                return rightEye;
            }
        }
        else
        {
            Debug.LogError("Trying to return a normal camera when isDouble is on.");
            return null;
        }
    }
}