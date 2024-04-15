using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using HurricaneVR.Framework.Components;

public class LeverUnlockDoor : MonoBehaviour
{
    [SerializeField] private int step = 3;
    private HVRRotationTracker tracker;

    [SerializeField] private UnityEvent triggerOnThreshold;

    private void Start()
    {
        tracker = GetComponent<HVRRotationTracker>();
    }

    //Check HVR Step stage (percentage lifted up, basically). If we reach that point then execute the Unity Event, which could be to end the level, or open a door, etc.
    public void CheckStep()
    {
        if (step >= tracker.Step)
        {
            triggerOnThreshold.Invoke();
        }
    }
}
