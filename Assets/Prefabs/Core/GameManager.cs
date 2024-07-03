using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using INab.WorldScanFX;

public class GameManager : MonoBehaviour
{
    [Header("Global settings")]
    [SerializeField] private bool isDontDestroyOnLoad = false;
    [SerializeField] private bool CybervisionOn = true;
    //[SerializeField] private GameObject playerObject;
    public bool isRightEye = true;
    public TMP_Dropdown eyeChoice;
    public Toggle isDouble;
    public GameObject glasses;
    public List<Camera> currentCybereyes = new List<Camera>();
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

    [Header("Head Polish")]
    [SerializeField] private bool IgnoreHeadPolish;
    [SerializeField] private GameObject LeftEyeInHead;
    [SerializeField] private GameObject RightEyeInHead;

    [Header("Detached eye throwable")]
    public bool switchCam;
    public GameObject detachedEyePrefab;
    private Canvas eyepatchCanvas;
    [SerializeField] private Color eyepatchColour;
    EyeHolder eyeHolder;

    public static GameManager Instance;

    [Header("Scan Data")]
    public ScanFXBase scanFX;

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
                isRightEye = SettingsManager.Instance.rightEyeSelected;

            if (isRightEye) eyeChoice.value = 0; else eyeChoice.value = 1;
            //Left handed gun should be swapped in instead and be tracked as a variable here.
        }

        if (isDontDestroyOnLoad) DontDestroyOnLoad(this.gameObject);

        //playerObject = GameObject.FindGameObjectWithTag("HVR Player");

        //Auto bind cybervision immediately
        refreshCybereye();

        //Get eyepatch canvas for detached eye.
        eyepatchCanvas = detachedEyePrefab.GetComponentInChildren<Canvas>();
        eyeHolder = detachedEyePrefab.GetComponent<EyeHolder>();
    }

    public void ToggleDoubleCybereyes()
    {
        //Clear cybereyes array
        currentCybereyes.Clear();

        //If this should be double, add both cameras to be targetted by currentCybereyes.
        //Also forces view to be from the head perspective
        if (isDouble.isOn)
        {
            glasses.SetActive(true);
            currentCybereyes.Add(rightEye);
            currentCybereyes.Add(leftEye);
            if (isRightEye) rightEye.enabled = true; else leftEye.enabled = true;
            detachedEyePrefab.GetComponentInChildren<Camera>().enabled = false;
            Debug.Log("1");
            
        }
        else
        {
            glasses.SetActive(false);
            if (isRightEye) rightEye.enabled = false; else leftEye.enabled = false;
            detachedEyePrefab.GetComponentInChildren<Camera>().enabled = true;
            refreshCybereye();
            Debug.Log("2");
        }
        //Forces the switch cam to be false to stop any weird edge cases
        switchCam = false;

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

        //if eye is out of head then enable the other eye that should be in the players head
        if (eyeHolder.eyeIsSpawned)
        {
            if (isRightEye)
            {
                rightEye.enabled = false;
                leftEye.enabled = true;
            }
            else
            {
                rightEye.enabled = true;
                leftEye.enabled = false;
            }
        }
        

        UpdateDetachedEyeTarget();
    }

    private void refreshCybereye()
    {
        //Make sure whenever cybervision is changed by setting, its disabled immediately
        //SetCybervisionState(false);

        //Instead of CLEARING the eye references, just attempt to remove both instances of the HMD eyes for now.
        if (currentCybereyes.Contains(rightEye)) currentCybereyes.Remove(rightEye);
        if (currentCybereyes.Contains(leftEye)) currentCybereyes.Remove(leftEye);

        //If right eye is currently the cybereye, make sure it's reset 
        if (isRightEye) currentCybereyes.Add(rightEye); else currentCybereyes.Add(leftEye);

        //Correctly put eyes in head (when looking at yourself in detached eye view
        if (isRightEye) RightEyeInHead.SetActive(false); else LeftEyeInHead.SetActive(false);

        if (isDouble.isOn || IgnoreHeadPolish)
        {
            LeftEyeInHead.SetActive(true);
            RightEyeInHead.SetActive(true);

            //if (switchCam == true)
            //{
            //    switchCam = false;
            //    if (isRightEye) rightEye.enabled = false; else leftEye.enabled = false;
            //}
        }

        //Add detached eye prefab, too.
        currentCybereyes.Add(detachedEyePrefab.GetComponentInChildren<Camera>());

        UpdateDetachedEyeTarget();
        
    }

    public void ToggleCybervision()
    {
        //player can only toggle cybervision when the eye is not detached from face
        if(eyeHolder.eyeIsSpawned == false)
        {
            if (CybervisionOn)
            {
                scanFX.scansLeft = 0;
                scanFX.timeLeft = 0f;
                scanFX.timePassed = 0f;
                scanFX.PassScanOriginProperties();
                scanFX.StartScan(1);
                SetCybervisionState(false);
            }
            else
            {
                scanFX.scansLeft = 0;
                scanFX.timeLeft = 0f;
                scanFX.timePassed = 0f;
                scanFX.PassScanOriginProperties();
                scanFX.StartScan(1);
                SetCybervisionState(true);
            }
        }
        else
        {
            if (isDouble.isOn)
            {
                if (switchCam == false)
                {
                    switchCam = true;
                    detachedEyePrefab.GetComponentInChildren<Camera>().enabled = true;
                }
                else
                {
                    switchCam = false;
                    if (isRightEye) rightEye.enabled = true; else leftEye.enabled = true;
                    detachedEyePrefab.GetComponentInChildren<Camera>().enabled = false;
                }
            }
            
        }
        //If Cybervision is already on, turn it off from whichever eye(s) have cybervision, and vice versa if off.
        
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
        if (!isDouble.isOn)
        {
            foreach (Camera eye in currentCybereyes)
            {
                eye.enabled = false;
            }
            //Re-enable the detached eye camera
            if (currentCybereyes.Contains(detachedEyePrefab.GetComponentInChildren<Camera>()))
                detachedEyePrefab.GetComponentInChildren<Camera>().enabled = true;
        }
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
            //NOTE: CHANGE THIS! there should be a gadget or extra camera or something on the players hands that should just see this camera in this mode.
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

        if (doHide)
        {
            SetCybervisionState(false); //Turn off cybervision is camera is hidden
        }
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