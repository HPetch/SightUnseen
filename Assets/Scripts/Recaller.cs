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
using HurricaneVR.TechDemo.Scripts;

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
    Rigidbody rightRb;
    Rigidbody leftRb;

    public DemoHolster leftHolster;
    public DemoHolster rightHolster;

    public AudioClip recallEye;

    public void Start()
    {
        //Grabber = GameObject.FindObjectsOfType<HVRHandGrabber>().FirstOrDefault(e => e.gameObject.activeInHierarchy);
        rightRb = rightHand.gameObject.GetComponent<Rigidbody>();
        leftRb = leftHand.gameObject.GetComponent<Rigidbody>();
    }

    public void GrabGun()
    {
        if (gunPrefab && Grabber)
        {
            //pick which hand to send the gun to based on the toggle use some UI menu or something to change this
            //AlterHandCheck();
            //just in case we want the button to drop the object if the player is holding it
            if (GrabTrigger == HVRGrabTrigger.ManualRelease && Grabber.GrabbedTarget == gunPrefab)
            {
                Grabber.ForceRelease();
                return;
            }

            //grabber needs to have it's release sequence completed if it's holding something
            HVRHandGrabber recaller = Grabber;

            if (eyePrefab.GetComponent<ConfigurableJoint>() != null)
            {
                if (eyePrefab.GetComponent<ConfigurableJoint>().connectedBody == leftRb)
                {
                    recaller = rightHand;
                }
                if (eyePrefab.GetComponent<ConfigurableJoint>().connectedBody == rightRb)
                {
                    recaller = leftHand;
                }
            }
            //if (Grabber.IsGrabbing)
            //    Grabber.ForceRelease();
           
            recaller.Grab(gunPrefab, GrabTrigger, GrabPoint);
        }
    }

    //same as above but for the eye instead
    public void GrabEye()
    {
        HVRRotationTracker tracker = GameManager.Instance.glasses.GetComponent<HVRRotationTracker>();
        tracker.StepChanged.RemoveAllListeners();
        if (eyePrefab && Grabber)
        {
            //AlterHandCheck();
            if (GrabTrigger == HVRGrabTrigger.ManualRelease && Grabber.GrabbedTarget == eyePrefab)
            {
                Grabber.ForceRelease();
                return;
            }

            //grabber needs to have it's release sequence completed if it's holding something
            HVRHandGrabber recaller = Grabber;
            if (gunPrefab.GetComponent<ConfigurableJoint>() != null)
            {
                if (gunPrefab.GetComponent<ConfigurableJoint>().connectedBody == leftRb)
                {
                    recaller = rightHand;
                }
                if (gunPrefab.GetComponent<ConfigurableJoint>().connectedBody == rightRb)
                {
                    recaller = leftHand;
                }
            }
            else
            {
                recaller = Grabber;
            }
            //if (Grabber.IsGrabbing)
            //    Grabber.ForceRelease();
            
            
            recaller.Grab(eyePrefab, GrabTrigger, GrabPoint);
            //materialise eye
            var mfxActivator = eyePrefab.gameObject.GetComponent<MfxController>();
            if (mfxActivator != null)
                mfxActivator.ActivateBackward();

            AudioSource source = eyePrefab.GetComponent<AudioSource>();
            source.clip = recallEye;
            source.Play();

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

    void AlterHandCheck()
    {
        if (GameManager.Instance.rightHandRecall.isOn)
        {
            Grabber = rightHand;
        }
        else { Grabber = leftHand; }
    }

    public void HolsterGun()
    {
        leftHolster.StartCoroutine(leftHolster.TryGrabSpecificGrabbable(gunPrefab));
    }
}
