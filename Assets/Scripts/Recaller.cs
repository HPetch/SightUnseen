using HurricaneVR.Framework.Core.Grabbers;
using HurricaneVR.Framework.Core.HandPoser;
using HurricaneVR.Framework.Core;
using HurricaneVR.Framework.Shared;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using INab.WorldScanFX;
using QFX.MFX;
using UnityEngine.InputSystem.HID;
using HurricaneVR.Framework.Components;

public class Recaller : MonoBehaviour
{
    public bool alterHand;

    public HVRHandGrabber leftHand;
    public HVRHandGrabber rightHand;
    public HVRHandGrabber Grabber;
    public HVRGrabbable gunPrefab;
    public HVRGrabbable eyePrefab;
    public HVRGrabTrigger GrabTrigger;
    public HVRPosableGrabPoint GrabPoint;

    public void Start()
    {
        Grabber = GameObject.FindObjectsOfType<HVRHandGrabber>().FirstOrDefault(e => e.gameObject.activeInHierarchy);
    }

    public void GrabGun()
    {
        if (gunPrefab && Grabber)
        {
            //pick which hand to send the gun to based on the toggle use some UI menu or something to change this
            if (alterHand)
            {
                Grabber = rightHand;
            }
            else
            {
                Grabber = leftHand;
            }
            //just in case we want the button to drop the object if the player is holding it
            if (GrabTrigger == HVRGrabTrigger.ManualRelease && Grabber.GrabbedTarget == gunPrefab)
            {
                Grabber.ForceRelease();
                return;
            }

            //grabber needs to have it's release sequence completed if it's holding something
            if (Grabber.IsGrabbing)
                Grabber.ForceRelease();
            Grabber.Grab(gunPrefab, GrabTrigger, GrabPoint);
        }
    }

    //same as above but for the eye instead
    public void GrabEye()
    {
        HVRRotationTracker tracker = GameManager.Instance.glasses.GetComponent<HVRRotationTracker>();
        tracker.StepChanged.RemoveAllListeners();
        if (eyePrefab && Grabber)
        {
            if (alterHand)
            {
                Grabber = rightHand;
            }
            else
            {
                Grabber = leftHand;
            }
            if (GrabTrigger == HVRGrabTrigger.ManualRelease && Grabber.GrabbedTarget == eyePrefab)
            {
                Grabber.ForceRelease();
                return;
            }

            //grabber needs to have it's release sequence completed if it's holding something
            if (Grabber.IsGrabbing)
                Grabber.ForceRelease();
            Grabber.Grab(eyePrefab, GrabTrigger, GrabPoint);
            //materialise eye
            var mfxActivator = eyePrefab.gameObject.GetComponent<MfxController>();
            if (mfxActivator != null)
                mfxActivator.ActivateBackward();

            //force player view if in glasses mode
            if (GameManager.Instance.isDouble.isOn)
            {
                if (GameManager.Instance.switchCam)
                {
                    GameManager.Instance.ToggleCybervision();
                }
                GameManager.Instance.detachedEyePrefab.GetComponentInChildren<Camera>().enabled = false;
            }
        }
    }

    public void ScanEffect()
    {

    }
}
