using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitySwitch : MonoBehaviour
{
    private Vector3 gravity;
    private bool drivingObjectGravity = false;
    private Collider eye;
    public AudioSource audioSource;
    public AudioClip eyeStick;
    public bool shouldReverseEyeRotation;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Eyeball")
        {
            drivingObjectGravity = true;
            eye = other;
            eye.attachedRigidbody.useGravity = false;
            audioSource.clip = eyeStick;
            audioSource.Play();
            if (shouldReverseEyeRotation)
            {
                other.GetComponent<EyeHolder>().isGravityReversed = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Eyeball")
        {
            drivingObjectGravity = false;
            eye.attachedRigidbody.useGravity = true;
            if (shouldReverseEyeRotation)
            {
                other.GetComponent<EyeHolder>().isGravityReversed = false;
            }
        }
            
    }

    private void FixedUpdate()
    {
        if (drivingObjectGravity)
        {
            eye.attachedRigidbody.AddForce(Physics.gravity * -1.5f, ForceMode.Acceleration);
        }
    }
}
