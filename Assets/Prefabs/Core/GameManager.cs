using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using INab.WorldScanFX;
using HurricaneVR.Framework.Components;
using UnityEngine.Events;
using DG.Tweening;
using HurricaneVR.Framework.Core.Player;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    //bool for forcing glasses mode for the little babies
    public bool forceBabyMode;
    public OnboardingSettingsSO settings;
    public bool isInSetup;

    [Header("Global settings")]
    [SerializeField] private bool isDontDestroyOnLoad = false;
    [SerializeField] public bool CybervisionOn = true;
    //[SerializeField] private GameObject playerObject;
    public bool isRightEye = true;
    public TMP_Dropdown eyeChoice;
    public Toggle isDouble;
    public Toggle displaySubtitles;
    public List<Camera> currentCybereyes = new List<Camera>();
    [SerializeField] private Color EMPColour;
    [SerializeField] private Color flashColour;
    SceneManaging sceneManager;
    public Toggle rightHandRecall;

    //Set up Default and Cybereye culling masks correctly in Inspector
    //You can't add mask layers in code in a more user-friendly way.
    //Each eye has an Override to display stuff on the HMD no matter what (i.e. flashbang)
    [Header("Left Eye")]
    public Camera leftEye;
    //[SerializeField] private GameObject LeftEyepatchCanvas;
    [SerializeField] public LayerMask leftDefaultMask;
    [SerializeField] public LayerMask leftCybereyeMask;

    [Header("Right Eye")]
    public Camera rightEye;
    //[SerializeField] private GameObject RightEyepatchCanvas;
    [SerializeField] public LayerMask rightDefaultMask;
    [SerializeField] public LayerMask rightCybereyeMask;

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

    [Header("Glasses Mode Stuff")]
    public GameObject glasses;
    public bool glassesLeverPreventer;
    public static GameManager Instance;
    public AudioClip glassesOn;
    public AudioClip glassesOff;

    [Header("Scan Data")]
    public ScanFXBase scanFX;
    public GameObject scanMask;
    Coroutine scanRoutine;
    bool isScanning = false;
    float currentScanTime;
    public float maxScanTimer = 7;
    public AudioClip enableCyberVision;
    public AudioClip disableCyberVision;
    public AudioSource audioSource;
    public Toggle showAimTrajectory;

    [Header("Movement Data")]
    public Toggle smoothMovement;
    HVRTeleportCollisonHandler teleportCol;
    HVRTeleporter teleporter;
    CharacterController characterController;
    Recaller recall;
    public float fallDelayTimer = 1;
    HVRPlayerController hvrPlayerController;

    private void Awake()
    {
        sceneManager = GetComponent<SceneManaging>();
        GameObject player = GameObject.Find("PlayerController");
        teleportCol = player.GetComponent<HVRTeleportCollisonHandler>();
        teleporter = player.GetComponent<HVRTeleporter>();
        characterController = player.GetComponent<CharacterController>();
        recall = player.GetComponent<Recaller>();
        hvrPlayerController = player.GetComponent<HVRPlayerController>();

        displaySubtitles.isOn = false;
        displaySubtitles.isOn = true;

        // If there is an instance, and it's not me, delete myself.
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        //if ((leftEye != null) && (rightEye != null))
        //{
        //        isRightEye = SettingsManager.Instance.rightEyeSelected;

        //    if (isRightEye) eyeChoice.value = 0; else eyeChoice.value = 1;
        //    //Left handed gun should be swapped in instead and be tracked as a variable here.
        //}

        if (isDontDestroyOnLoad) DontDestroyOnLoad(this.gameObject);

        //playerObject = GameObject.FindGameObjectWithTag("HVR Player");

        //Auto bind cybervision immediately
        refreshCybereye();

        //Get eyepatch canvas for detached eye.
        eyepatchCanvas = detachedEyePrefab.GetComponentInChildren<Canvas>();
        eyeHolder = detachedEyePrefab.GetComponent<EyeHolder>();

        if (forceBabyMode)
        {
            isDouble.isOn = true;
            ToggleDoubleCybereyes();
        }

        if(isInSetup == false)
        {
            ApplyStartupSettings();
        }

        if (!isDouble.isOn)
        {
            glasses.SetActive(false);
        }
        
    }

    private void Update()
    {
        if (Keyboard.current[Key.R].wasPressedThisFrame)
        {
            sceneManager.LoadOnboardingScene();
        }
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
            if(forceBabyMode == false)
            {
                glasses.SetActive(false);
                if (eyeHolder.eyeIsSpawned)
                {
                    if (isRightEye) rightEye.enabled = false; else leftEye.enabled = false;
                    detachedEyePrefab.GetComponentInChildren<Camera>().enabled = true;
                }
                else
                {
                    if (isRightEye) rightEye.enabled = true; else leftEye.enabled = true;
                    detachedEyePrefab.GetComponentInChildren<Camera>().enabled = false;
                }
                
                refreshCybereye();
                Debug.Log("2");
            }
        }
        //Forces the switch cam to be false to stop any weird edge cases
        switchCam = false;
        //reset cybervision to be correct
        if (eyeHolder.eyeIsSpawned == true)
        {
            SetCybervisionState(false);
            if (!isDouble.isOn)
            {
                SetCybervisionState(true);
            }
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
        if (eyeHolder.eyeIsSpawned == false)
        {
            //find the mesh to use later
            MeshRenderer glassesMesh = glasses.GetComponentInChildren<MeshRenderer>();

            //disable cybervision
            if (CybervisionOn)
            {
                //stop any scanners
                ScanEffect(false);

                //if in double mode then move the glasses to be on the players forehead
                if (isDouble.isOn)
                {
                    MoveGlasses(false);
                    glassesMesh.enabled = true;
                    PlayAudio(glassesOff);
                    PlayAudio(disableCyberVision);
                }
            }
            //enable cybervision
            else
            {
                ScanEffect(true);

                //if in double eye mode then move the glasses to be in front of the player
                if (isDouble.isOn)
                {
                    MoveGlasses(true);
                    glassesMesh.enabled = false;
                    PlayAudio(glassesOn);
                    PlayAudio(enableCyberVision);
                }
            }
        }
        //switching perspective when in double mode
        else
        {
            if (isDouble.isOn)
            {
                //if in normal perspective
                if (switchCam == false)
                {
                    //run the scan effect, move glasses to players face and enable the eye camera
                    switchCam = true;
                    ScanEffect(true);
                    MoveGlasses(true);
                    detachedEyePrefab.GetComponentInChildren<Camera>().enabled = true;
                    rightEye.enabled = false;
                    leftEye.enabled = false;
                    PlayAudio(glassesOn);
                }
                else
                {
                    //otherwise do the opposite
                    switchCam = false;
                    ScanEffect(false);
                    MoveGlasses(false);
                    //if (isRightEye) rightEye.enabled = true; else leftEye.enabled = true;
                    rightEye.enabled = true;
                    leftEye.enabled = true;
                    detachedEyePrefab.GetComponentInChildren<Camera>().enabled = false;
                    PlayAudio(glassesOff);
                }
            }
            

        }
        //If Cybervision is already on, turn it off from whichever eye(s) have cybervision, and vice versa if off.
        
    }

    void MoveGlasses(bool onFace)
    {
        HVRRotationTracker tracker = glasses.GetComponent<HVRRotationTracker>();
        HVRPhysicsLever lever = glasses.GetComponent<HVRPhysicsLever>();
        //move glasses to be on the players face
        if (onFace)
        {
            glassesLeverPreventer = true;
            tracker.Steps = 0;
            tracker.MaximumAngle = 1000;
            tracker.enabled = false;
            lever.enabled = false;
            glasses.transform.Rotate(50, 0, 0);
            tracker.MaximumAngle = 93;
            tracker.Steps = 2;
            tracker.enabled = true;
            lever.enabled = true;
        }
        //move glasses to be on the players forehead
        else
        {
            glassesLeverPreventer = true;
            tracker.MaximumAngle = 1000;
            tracker.Steps = 0;
            tracker.enabled = false;
            lever.enabled = false;
            Vector3 pos = new Vector3(68, 0, 0);
            glasses.transform.Rotate(-50, 0, 0);

            tracker.MaximumAngle = 93;
            tracker.Steps = 2;
            tracker.enabled = true;
            lever.enabled = true;
        }
        //run routine to make the glassses act normal again
        StartCoroutine(GlassesTimer());
    }

    public void ScanEffect(bool shouldRun)
    {
        if (shouldRun)
        {
            //reset all scanner data in case the player spams the button
            if (scanRoutine != null)
            {
                StopCoroutine(scanRoutine);
            }
            scanMask.transform.localScale = Vector3.zero;
            scanMask.transform.position = detachedEyePrefab.transform.position;
            scanFX.scansLeft = 0;
            scanFX.timeLeft = 0f;
            scanFX.timePassed = 0f;

            //run the scan
            scanFX.PassScanOriginProperties();
            scanFX.StartScan(1);
            SetCybervisionState(true);
            currentScanTime = 0;
            isScanning = true;
            scanRoutine = StartCoroutine(ScanMaskExpand());
            PlayAudio(enableCyberVision);
        }
        else
        {
            //stop any currently running scanners and reset mask 
            if (scanRoutine != null)
            {
                StopCoroutine(scanRoutine);
            }
            scanMask.transform.localScale = Vector3.zero;
            SetCybervisionState(false);
            PlayAudio(disableCyberVision);
        }
    }

    //we need this coroutine becasuse the glasses are little babies and want to constantly override literally anything that we do 
    IEnumerator GlassesTimer()
    {
        yield return new WaitForSecondsRealtime(.01f);
        glassesLeverPreventer = false;
    }

    public void GlassesGrab()
    {
        //we need the glassesLeverPreventer bool to be false here so that things actually act normal when pressing the A button
        if (isDouble.isOn && glassesLeverPreventer == false)
        {
            MeshRenderer glassesMesh = glasses.GetComponentInChildren<MeshRenderer>();
            if (eyeHolder.eyeIsSpawned)
            {
                glassesMesh.enabled = true;
                if (switchCam == false)
                {
                    switchCam = true;

                    ScanEffect(true);

                    detachedEyePrefab.GetComponentInChildren<Camera>().enabled = true;
                    rightEye.enabled = false;
                    leftEye.enabled = false;
                }
                else
                {
                    switchCam = false;

                    ScanEffect(false);

                    //if (isRightEye) rightEye.enabled = true; else leftEye.enabled = true;
                    rightEye.enabled = true;
                    leftEye.enabled = true;
                    detachedEyePrefab.GetComponentInChildren<Camera>().enabled = false;
                }
            }
            else
            {
                if (switchCam == false)
                {
                    switchCam = true;
                    ScanEffect(true);
                    glassesMesh.enabled = false;
                }
                else
                {
                    switchCam = false;
                    ScanEffect(false);
                    glassesMesh.enabled = true;
                }
            }
            
        }
    }

    void ChangeView()
    {

    }

    IEnumerator ScanMaskExpand()
    {
        //run a timer that expands the mask sphere over time
        while (isScanning == true)
        {
            currentScanTime += Time.deltaTime;
            yield return new WaitForSeconds(0.001f);

            if (currentScanTime >= maxScanTimer)
            {
                isScanning = false;
                scanRoutine = null;
            }

            if (isScanning == true)
            {
                scanMask.transform.localScale += new Vector3(0.2f, 0.2f, 0.2f);
            }
        }
    }

    public void SetCybervisionState(bool value)
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
            if (isDouble.isOn && eyeHolder.eyeIsSpawned)
            {
                Camera eyeCam = detachedEyePrefab.GetComponentInChildren<Camera>();
                if (isRightEye) eyeCam.cullingMask = rightCybereyeMask; else eyeCam.cullingMask = leftCybereyeMask;
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
            if (isDouble.isOn && eyeHolder.eyeIsSpawned)
            {
                Camera eyeCam = detachedEyePrefab.GetComponentInChildren<Camera>();
                if (isRightEye) eyeCam.cullingMask = rightCybereyeMask; else eyeCam.cullingMask = leftCybereyeMask;
            }
            CybervisionOn = false;
        }
    }

    public void doDetachedVision()
    {
        //Disable all cameras
        if (!isDouble.isOn)
        {
            //foreach (Camera eye in currentCybereyes)
            //{
            //    eye.enabled = false;
            //}
            ////Re-enable the detached eye camera
            //if (currentCybereyes.Contains(detachedEyePrefab.GetComponentInChildren<Camera>()))
            //    detachedEyePrefab.GetComponentInChildren<Camera>().enabled = true;

            if (isRightEye) rightEye.enabled = false; else leftEye.enabled = false;
            detachedEyePrefab.GetComponentInChildren<Camera>().enabled = true;
        }
    }

    public void doAttachedVision()
    {
        ////Disable all cameras
        //foreach (Camera eye in currentCybereyes)
        //{
        //    eye.enabled = true;
        //}
        ////Disable the detached eye camera
        //if (currentCybereyes.Contains(detachedEyePrefab.GetComponentInChildren<Camera>()))
        //    detachedEyePrefab.GetComponentInChildren<Camera>().enabled = false;

        if (isRightEye) rightEye.enabled = true; else leftEye.enabled = true;
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

        //if (doHide)
        //{
        //    SetCybervisionState(false); //Turn off cybervision is camera is hidden
        //}
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

    public void SmoothMovementToggle()
    {
        if (smoothMovement.isOn)
        {
            hvrPlayerController.teleportModeOn = false;
            //characterController.enabled = true;
            teleportCol.enabled = false;
            teleporter.enabled = false;
        }
        else
        {
            hvrPlayerController.teleportModeOn = true;
            //characterController.enabled = false;
            teleportCol.enabled = true;
            teleporter.enabled = true;
        }
    }

    public void MoveGlassesBack()
    {
        if (isDouble.isOn && !switchCam)
        {
            MoveGlasses(false);
        }
    }
    public void ToggleRecallHand()
    {
        if (rightHandRecall.isOn)
        {
            recall.Grabber = recall.rightHand;
        }
        else
        {
            recall.Grabber = recall.leftHand;
        }
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

    public void PlayAudio(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }
    void ApplyStartupSettings()
    {
        if (settings.rightEye)
        {
            eyeChoice.value = 0;
        }
        else
        {
            eyeChoice.value = 1;
        }

        if (settings.glassesOn)
        {
            isDouble.isOn = true;
        }
        else
        {
            isDouble.isOn = false;
        }

        if (settings.rightHanded)
        {
            rightHandRecall.isOn = true;
        }
        else
        {
            rightHandRecall.isOn = false;
        }

        if (settings.smoothMovement)
        {
            smoothMovement.isOn = true;
        }
        else
        {
            smoothMovement.isOn = false;
        }

        eyeChoiceToggle();
        ToggleDoubleCybereyes();
        ToggleRecallHand();
        StartCoroutine(FallDelay());
    }

    IEnumerator FallDelay()
    {
        yield return new WaitForSeconds(fallDelayTimer);
        SmoothMovementToggle();
    }
}