using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CheckpointExtraTime : MonoBehaviour
{
    public bool hasBeenActivatedBefore = false;
    public UnityEvent myEvent;

    private void OnTriggerEnter(Collider other)
    {
        //if hit a laser
        if (other.gameObject.CompareTag("Player") && hasBeenActivatedBefore == false)
        {
            hasBeenActivatedBefore = true;
            myEvent.Invoke();
        }
    }
}
