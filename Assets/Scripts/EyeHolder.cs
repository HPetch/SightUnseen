using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HurricaneVR;
using UnityEngine.SpatialTracking;
using HurricaneVR.Framework.ControllerInput;
using HurricaneVR.Framework.Core.Player;
using UnityEngine.Windows;
using Unity.VisualScripting;
using DG.Tweening;
using HurricaneVR.Framework.Core.Utils;
using HurricaneVR.Framework.Core.Grabbers;
using static System.Security.Cryptography.ECCurve;

public class EyeHolder : MonoBehaviour
{
    public bool eyeIsSpawned = false;
    private GameManager gameManager;
    private Camera thisCam;
    private Rigidbody rb;

    private Renderer[] renderers;
    private TrackedPoseDriver hmdTracker;
    private bool inSpecialZone;

    //Eye camera rotating on joystick button press
    [SerializeField] private HVRPlayerController playerController;
    [SerializeField] private Transform RecallLocation;
    private bool useDetachedEye = false;
    [SerializeField] private GameObject eyeballRotTargetAnim;
    [SerializeField] private GameObject bodyRotTargetAnim;


    [SerializeField] private float stillMotionTimer = 1f;
    private float stillMotionMax;
    public bool eyeSetDown;

    [SerializeField] private GameObject directionIndicator;

    public bool watchMode;
    public GameObject watchCam;

    public AudioSource audioSource;
    public AudioClip turnOnSound;

    public bool canGoOnGun = true;
    public GameObject onGunPos;
    public bool onGun;
    public HVRSocket gunSocket;

    public bool isGravityReversed;
    public float eyeReverseOffset;

    private void Awake()
    {
        playerController = FindObjectOfType<HVRPlayerController>();
        stillMotionMax = stillMotionTimer; //Remember the still motion timer variable as we'll be counting it down per frames
        rb = GetComponent<Rigidbody>();

        //Get HMD tracker
        hmdTracker = GetComponentInChildren<TrackedPoseDriver>();

        //Get all renderer components to be hidden when in socket and out of socket
        renderers = GetComponentsInChildren<Renderer>();
        ToggleRenderers(false);
    }

    public void ToggleRenderers(bool enabled)
    {
        foreach (Renderer item in renderers)
        {
            item.enabled = enabled;
        }

        directionIndicator.SetActive(enabled);

    }

    public void SwitchEyeToDetached(bool doDetached)
    {
        Debug.Log("s");
        if (doDetached)
        {
            //GameManager.Instance.SetCybervisionState(true);
            GameManager.Instance.UpdateDetachedEyeTarget();
            GameManager.Instance.doDetachedVision();
            //Tell gamemanager to disable Cybereye Camera and enable Detached Camera and edit its target eye based on isRightEye.
            GameManager.Instance.CybervisionOn = false;
            if (GameManager.Instance.isDouble.isOn && GameManager.Instance.switchCam || !GameManager.Instance.isDouble.isOn && GameManager.Instance.CybervisionOn)
            {
                GameManager.Instance.ToggleCybervision();
            }
            eyeIsSpawned = true;
            GameManager.Instance.hideViewport("cyber", true);
            if (watchMode)
            {
                //the unused vector is a test to make a more fluid pop up if we want it
                Vector3 firstScale = new Vector3( 0.013761f, 0.013761f, 0f);
                Vector3 secondScale = new Vector3(0.013761f, 0.013761f, 0.013761f);
                watchCam.transform.DOScale(secondScale, 0.3f);
            }
            Camera eyeCam = GameManager.Instance.detachedEyePrefab.GetComponentInChildren<Camera>();
            if (GameManager.Instance.isRightEye) eyeCam.cullingMask = GameManager.Instance.rightCybereyeMask; else eyeCam.cullingMask = GameManager.Instance.leftCybereyeMask;
        } else
        {
            //If not, we want to return vision to the Right Camera
            GameManager.Instance.UpdateDetachedEyeTarget();
            GameManager.Instance.doAttachedVision();
            GameManager.Instance.CybervisionOn = true;
            if(GameManager.Instance.isDouble.isOn && GameManager.Instance.switchCam || !GameManager.Instance.isDouble.isOn && GameManager.Instance.CybervisionOn)
            {
                GameManager.Instance.ToggleCybervision();
            }
            
            eyeIsSpawned = false;
            GameManager.Instance.hideViewport("cyber", false);

            //Also, ensure that eyeball is no longer the rotation target and playerController has it back.
            playerController.rotatingByEye = false;
            //playerController.RotationEnabled = true;
            if (watchMode)
            {
                //if we want the more fluid pop up then copy paste the vectors here too
                Vector3 firstScale = new Vector3(0, 0.013761f, 0f);
                watchCam.transform.DOScale(firstScale, 0.3f);
            }
            GameManager.Instance.SetCybervisionState(false);
        }
    }

    public void Test(string test)
    {
        Debug.Log("TEST " + test);
    }

    private void Update()
    {
        //While eye is spawned, keep checking for the velocity to be 0 for multiple seconds uninterrupted, then unhide viewport.
        if (eyeIsSpawned)
        {
            //Debug.Log(rb.velocity);
            stillMotionTimer -= Time.deltaTime; //Subtract from change in time, so once it hits 0 after X seconds

            if (float.Parse(Vector3.Distance(rb.velocity, Vector3.zero).ToString("F3")) != 0f)
            {
                stillMotionTimer = stillMotionMax; //Reset timer
                eyeSetDown = false;
            }

            //If the timer hits 0, we have been still for a second, so can unveil the eye prefab.
            if ((stillMotionTimer < 0f) && !eyeSetDown && !onGun)
            {
                eyeSetDown = true;
                hmdTracker.enabled = true; //Start tracking headset
                ResetOrientation();
                GameManager.Instance.hideViewport("cyber", false);

                if (!GameManager.Instance.isDouble.isOn)
                {
                    GameManager.Instance.CybervisionOn = false;

                    //play scan effect
                    //GameManager.Instance.ScanEffect(true);

                    //scan may cause epilepsy? so it just sets the state and makes the mask huge for now
                    GameManager.Instance.SetCybervisionState(true);
                    GameManager.Instance.scanMask.transform.localScale = new Vector3(100, 100, 100);
                    //GameManager.Instance.ToggleCybervision();
                }

                audioSource.clip = turnOnSound;
                audioSource.Play();
                
            }

            //If eye goes back to being moved around again, hide the viewport
            if (!eyeSetDown)
            {
                GameManager.Instance.hideViewport("cyber", true);
                hmdTracker.enabled = false; //Disable this so eye doesnt move about in hand
            }
            //transform.LookAt(TargetOffset());
        }
    }

    private void ResetOrientation()
    {
        //Reset velocity and rotation immediately
        rb.velocity = Vector3.zero;
        transform.rotation = Quaternion.identity;

        Vector3 targetPos; //Position of player in X and Z axis, on this object's Y axis so it does not get angled upwards.
        targetPos.y = transform.position.y;
        targetPos.x = playerController.transform.position.x;
        targetPos.z = playerController.transform.position.z;
        transform.LookAt(targetPos, Vector3.up);

        //offset the eye rotation based on which direction the player is looking
        Quaternion newRotation = transform.rotation;
        if (playerController.transform.rotation.y >= 0)
        {
            if (isGravityReversed)
            {
                newRotation.y -= eyeReverseOffset; 
            }
            else
            {
                newRotation.y -= 90;
                //newRotation.y -= playerController.snapVar;
            }
        }
        else
        {
            if (isGravityReversed)
            {
                newRotation.y += eyeReverseOffset;
            }
            else
            {
                newRotation.y += 90;
                //newRotation.y += playerController.snapVar;
            }
        }

        transform.rotation = newRotation.normalized;

        //if used snap rotation we just move it again lol
        if(playerController.snapVar != 0)
        {
            transform.Rotate(0, playerController.snapVar, 0);
        }
        //Debug.Log(targetPos);
    }


    //Controls whether rotation targets the detached eye if it is out of socket, allowing for finer control with the player.
    public void SwitchRotationTarget()
    {

        if (eyeIsSpawned)
        {
            //Because this toggles between the modes each time the method is called, switch what this value currently is then execute branch
            useDetachedEye = !useDetachedEye;
            if (useDetachedEye)
            {
                //playerController.RotationEnabled = false;
                playerController.rotatingByEye = true;

                //Play animation indicating the targetting got changed
                //Enable game object - animation will play automatically then get disabled at the end automatically.
                eyeballRotTargetAnim.SetActive(true);

            }
            else
            {
                //playerController.RotationEnabled = true;
                playerController.rotatingByEye = false;
                bodyRotTargetAnim.SetActive(true);

            }
        }
    }
    public void TeleportEyeBackToPlayer()
    {
        if (eyeIsSpawned)
        {
            transform.position = RecallLocation.position;

            //Ensure velocity is reset, to avoid collision momentum issues.
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    protected virtual void PlaySFX(AudioClip clip)
    {
        if (SFXPlayer.Instance) SFXPlayer.Instance.PlaySFX(clip, transform.position);
    }

    public void AttachToGun(bool attached)
    {
        if (attached /*&& canGoOnGun == true*/)
        {
            GameManager.Instance.hideViewport("cyber", true);
            eyeSetDown = false;
            onGun = true;
            
            //transform.parent = onGunPos.transform;
            transform.localPosition = Vector3.zero;
        }
        else
        {
            GameManager.Instance.hideViewport("cyber", true);
            EyeOnGunAimer aimer = gunSocket.GetComponentInParent<EyeOnGunAimer>();
            aimer.ForceVisibleOff();
            eyeSetDown = false;
            onGun = false;
            //transform.parent = null;
        }
    }

    public void FireEye()
    {
        if (onGun)
        {
            SphereCollider collider = gameObject.GetComponent<SphereCollider>();
            //EyeOnGunAimer aimer = gunSocket.GetComponentInParent<EyeOnGunAimer>();
            //aimer.SetTrajectoryVisible(false);
            //transform.parent = null;
            gunSocket.ForceRelease();
            stillMotionTimer = stillMotionMax;
            eyeSetDown = false;
            collider.isTrigger = false;
            rb.isKinematic = false;
            rb.AddForce(transform.forward * 3, ForceMode.Impulse);
            onGun = false;
        }
    }

    public void CanEyeGoOnGun(bool canI)
    {
        if (canI)
        {
            canGoOnGun = true;
        }
        else
        {
            canGoOnGun = false;
        }
    }

    public void BackToNormalGravity()
    {
        isGravityReversed = false;
    }
}
