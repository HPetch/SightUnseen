using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HurricaneVR;
using UnityEngine.SpatialTracking;
using HurricaneVR.Framework.ControllerInput;
using HurricaneVR.Framework.Core.Player;
using UnityEngine.Windows;
using Unity.VisualScripting;

public class EyeHolder : MonoBehaviour
{
    private bool eyeIsSpawned = false;
    private GameManager gameManager;
    private Camera thisCam;
    private Rigidbody rb;

    private Renderer[] renderers;
    private TrackedPoseDriver hmdTracker;
    private bool inSpecialZone;

    //Eye camera rotating on joystick button press
    [SerializeField] private HVRPlayerController playerController;
    private bool useDetachedEye = false;
    [SerializeField] private GameObject eyeballRotTargetAnim;
    [SerializeField] private GameObject bodyRotTargetAnim;


    [SerializeField] private float stillMotionTimer = 1f;
    private float stillMotionMax;
    private bool eyeSetDown;

    [SerializeField] private GameObject directionIndicator;

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
        if (doDetached)
        {
            GameManager.Instance.UpdateDetachedEyeTarget();
            GameManager.Instance.doDetachedVision();
            //Tell gamemanager to disable Cybereye Camera and enable Detached Camera and edit its target eye based on isRightEye.
            eyeIsSpawned = true;
            GameManager.Instance.hideViewport("cyber", true);
        } else
        {
            //If not, we want to return vision to the Right Camera
            GameManager.Instance.UpdateDetachedEyeTarget();
            GameManager.Instance.doAttachedVision();
            eyeIsSpawned = false;
            GameManager.Instance.hideViewport("cyber", false);

            //Also, ensure that eyeball is no longer the rotation target and playerController has it back.
            playerController.rotatingByEye = false;
            //playerController.RotationEnabled = true;
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
            if ((stillMotionTimer < 0f) && !eyeSetDown)
            {
                eyeSetDown = true;
                hmdTracker.enabled = true; //Start tracking headset
                ResetOrientation();
                GameManager.Instance.hideViewport("cyber", false);
                

            }

            //If eye goes back to being moved around again, hide the viewport
            if (!eyeSetDown)
            {
                GameManager.Instance.hideViewport("cyber", true);
                hmdTracker.enabled = false; //Disable this so eye doesnt move about in hand
            }

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
            transform.position = playerController.transform.position + Vector3.forward;

            //Ensure velocity is reset, to avoid collision momentum issues.
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}
