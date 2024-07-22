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
    private Animator animator;
    private float currentAngle;
    private HVRGrabbable grabbable;
    private UnityEvent onComplete;

    private void Awake()
    {
        //Get the Door Offset animator.
        animator = GetComponentInParent<Animator>();
    }

    public void updateRotate()
    {
        //While the user is grabbing the door, keep checking the current angle of the grabbable.
        currentAngle = transform.rotation.eulerAngles.y;

        //If the current angle is rotated past the threshold (currently only works going leftwards (negative numbers)), user has completed this challenge.
        if (currentAngle < angleGoal)
        {
            //Force the player to no longer hold the wheel and turn it off, to prevent any bugs with animation.
            grabbable.ForceRelease();
            grabbable.enabled = false;
            
            animator.SetBool("unlocked", true); //Animate the door opening

            onComplete.Invoke(); //Invoke anything that would happen once completed (e.g. dialogue lines)
        }
    }
}
