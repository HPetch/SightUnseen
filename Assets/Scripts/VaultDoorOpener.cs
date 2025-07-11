using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HurricaneVR.Framework.Core;
using HurricaneVR.Framework.Components;
using UnityEngine.Events;

public class VaultDoorOpener : MonoBehaviour
{

    //When rotation updator is called while rotating step, check if clamped angle = the angle held in desired angle, +/- a few degrees.
    //When in range and grab point is released, play an animation.

    [SerializeField] private float angleGoal = 0f;
    [Range(0f,5f)] [SerializeField] private float marginDegrees = 2f;
    private Collider wheelCollider;
    private Animator animator;
    private float currentAngle;
    private HVRRotationTracker rot;
    private HVRGrabbable grabbable;
    [SerializeField] private UnityEvent onComplete;
    public AudioClip openClip;
    public AudioClip rotateClip;
    public AudioSource audioSource;
    public AudioSource audioSource2;
    public bool hasPlayedRotate = false;
    private bool isDone = false;

    private void Awake()
    {
        //Get the Door Offset animator.
        animator = GetComponentInParent<Animator>();
        grabbable = GetComponent<HVRGrabbable>();
        rot = GetComponent<HVRRotationTracker>();
        wheelCollider = GetComponentInChildren<Collider>();
    }

    private void FixedUpdate()
    {
        if (!isDone) updateRotate();
    }

    public void updateRotate()
    {
        //While the user is grabbing the door, keep checking the current angle of the grabbable.
        currentAngle = rot.ClampedAngle;

        //If the current angle is rotated past the threshold (currently only works going leftwards (negative numbers)), user has completed this challenge.
        if ((currentAngle > (angleGoal - marginDegrees)) && (currentAngle < (angleGoal + marginDegrees)))
        {
            Debug.LogWarning("Ready to open this door");


            //Wait for player to release grip on this angle
            //if (!grabbable.IsHandGrabbed)
            //{
                //Force the player to no longer hold the wheel and turn it off, to prevent any bugs with animation.
                grabbable.ForceRelease();
                grabbable.enabled = false;
                GetComponent<Rigidbody>().isKinematic = true;
                wheelCollider.enabled = false;

                animator.SetBool("unlocked", true); //Animate the door opening
                isDone = true;

                onComplete.Invoke(); //Invoke anything that would happen once completed (e.g. dialogue lines)
                audioSource2.clip = openClip;
                audioSource2.Play();
            //}

        }
    }

    public void playSoundOnRotation()
    {

        if (hasPlayedRotate == false)
        {
            audioSource.clip = rotateClip;
            audioSource.Play();
            hasPlayedRotate = true;
        }
    }
}
