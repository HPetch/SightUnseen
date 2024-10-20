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
            if (other.GetComponent<EyeHolder>().eyeIsSpawned)
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
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Eyeball")
        {
            if (other.GetComponent<EyeHolder>().eyeIsSpawned)
            {
                drivingObjectGravity = false;
                eye.attachedRigidbody.useGravity = true;
                if (shouldReverseEyeRotation)
                {
                    other.GetComponent<EyeHolder>().isGravityReversed = false;
                }
            }
        }
            
    }

    private void FixedUpdate()
    {
        if (drivingObjectGravity && !eye.attachedRigidbody.isKinematic)
        {
            eye.attachedRigidbody.AddForce(Physics.gravity * -1.5f, ForceMode.Acceleration);

            //Attempt to fix all instances of eyeball being weird on the ceiling by forcing velocity to be 0 if, when rounded, it is 0
            Vector3 roundedVelocity = new Vector3();
            roundedVelocity.x = Mathf.Round(eye.attachedRigidbody.velocity.x * 100) / 100;
            roundedVelocity.y = Mathf.Round(eye.attachedRigidbody.velocity.y * 100) / 100;
            roundedVelocity.z = Mathf.Round(eye.attachedRigidbody.velocity.z * 100) / 100;

            if (roundedVelocity == Vector3.zero)
            {
                eye.attachedRigidbody.velocity = Vector3.zero;
            }
        }
    }
}
