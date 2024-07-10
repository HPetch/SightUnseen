using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitySwitch : MonoBehaviour
{
    private Vector3 gravity;
    private bool drivingObjectGravity = false;
    private Collider eye;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Eyeball")
        {
            drivingObjectGravity = true;
            eye = other;
            eye.attachedRigidbody.useGravity = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Eyeball")
        {
            drivingObjectGravity = false;
            eye.attachedRigidbody.useGravity = true;
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
