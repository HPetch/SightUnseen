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
using DG.Tweening;

public class Recaller : MonoBehaviour
{
    [Header("General Settings")]
    public bool alterHand;
    public bool gunEnteredHolster;

    [Header("Hand Settings")]
    public HVRHandGrabber leftHand;
    public HVRHandGrabber rightHand;
    public HVRHandGrabber Grabber;
    public HVRGrabTrigger GrabTrigger;
    public HVRPosableGrabPoint GrabPoint;

    [Header("Prefabs")]
    public HVRGrabbable gunPrefab;
    public HVRGrabbable eyePrefab;
    public GameObject fadeOnlyGun;
    public GameObject fadeOnlyEye;
    GameObject fadeGun;
    public GameObject gunLine;
    public GameObject recallEffect;
    public GameObject eyeRecall;
    public GameObject gunRecall;

    [Header("Holsters")]
    public DemoHolster leftHolster;
    public DemoHolster rightHolster;

    [Header("Audio Stuff")]
    public AudioClip recallEye;

    Rigidbody rightRb;
    Rigidbody leftRb;

    public bool canRecallEye;
    public bool canRecallGun;
    public void Start()
    {
        //Grabber = GameObject.FindObjectsOfType<HVRHandGrabber>().FirstOrDefault(e => e.gameObject.activeInHierarchy);
        rightRb = rightHand.gameObject.GetComponent<Rigidbody>();
        leftRb = leftHand.gameObject.GetComponent<Rigidbody>();
    }

    public void GrabGun()
    {
        if (canRecallGun)
        {
            if (gunPrefab && Grabber)
            {
                MaterialiseGun(fadeOnlyGun, gunPrefab.gameObject, gunRecall);

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
                var mfxActivator = gunPrefab.gameObject.GetComponent<MfxController>();
                if (mfxActivator != null)
                    mfxActivator.ActivateBackward();
            }
        }
    }

    //same as above but for the eye instead
    public void GrabEye()
    {
        if (canRecallEye)
        {
            HVRRotationTracker tracker = GameManager.Instance.glasses.GetComponent<HVRRotationTracker>();
            tracker.StepChanged.RemoveAllListeners();
            if (eyePrefab && Grabber)
            {
                if (eyePrefab.GetComponent<EyeHolder>().eyeIsSpawned)
                {
                    MaterialiseGun(fadeOnlyEye, eyePrefab.gameObject, eyeRecall);
                }

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
        if(gunEnteredHolster == false)
        {
            MaterialiseGun(fadeOnlyGun, gunPrefab.gameObject, gunRecall);
            gunLine.SetActive(false);
            
            //var fadeAwayGun = gun.gameObject.GetComponent<MfxController>();
            //if (fadeAwayGun != null)
            //    fadeAwayGun.ActivateBackward();

            if (gunPrefab.GetComponent<ConfigurableJoint>() != null)
            {
                if (gunPrefab.GetComponent<ConfigurableJoint>().connectedBody == leftRb)
                {
                    leftHolster.StartCoroutine(leftHolster.TryGrabSpecificGrabbable(gunPrefab));
                }
                if (gunPrefab.GetComponent<ConfigurableJoint>().connectedBody == rightRb)
                {
                    rightHolster.StartCoroutine(rightHolster.TryGrabSpecificGrabbable(gunPrefab));
                }
            }

            if (eyePrefab.GetComponent<EyeHolder>().onGun)
            {
                GrabEye();
            }

            var mfxActivator = gunPrefab.gameObject.GetComponent<MfxController>();
            if (mfxActivator != null)
                mfxActivator.ActivateBackward();
        }
    }

    public void MaterialiseGun(GameObject obj, GameObject prefab, GameObject recallPrefab)
    {
        GameObject recallVFX = Instantiate(recallPrefab, prefab.transform.position, prefab.transform.rotation);
        recallVFX.GetComponent<TrackingVFX>().objToFollow = prefab;
        fadeGun = Instantiate(obj, prefab.transform.position, prefab.transform.rotation);
        fadeGun.GetComponent<MfxController>().ActivateForward();
    }

    void DestroyOBJ(GameObject obj)
    {
        Destroy(obj);
    }

    public void EnterHolster(bool didIDoThat)
    {
        if (didIDoThat)
        {
            gunEnteredHolster = true;
        }
        else
        {
            gunEnteredHolster = false;
        }
    }

    public void EnableGunLine()
    {
        gunLine.SetActive(true);
        //if(gunPrefab.GetComponent<ConfigurableJoint>().connectedBody != rightRb && gunPrefab.GetComponent<ConfigurableJoint>().connectedBody != leftRb)
        //{
        //    gunLine.SetActive(false);
        //}
        //else
        //{
        //    gunLine.SetActive(true);
        //}

    }

    public void EnableGunRecall(bool enabler)
    {
        if (enabler)
        {
            canRecallGun = true;
        }
        else
        {
            canRecallGun = false;
        }
        
    }

    public void EnableEyeRecall(bool enabler)
    {
        if (enabler)
        {
            canRecallEye = true;
        }
        else
        {
            canRecallEye = false;
        }
    }
}
