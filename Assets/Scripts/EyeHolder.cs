using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HurricaneVR;

public class EyeHolder : MonoBehaviour
{
    private bool eyeIsSpawned = false;
    private GameManager gameManager;
    private Camera thisCam;
    private Rigidbody rb;

    private Renderer[] renderers;


    [SerializeField] private float stillMotionTimer = 1f;
    private float stillMotionMax;
    private bool eyeSetDown;

    private void Awake()
    {
        stillMotionMax = stillMotionTimer; //Remember the still motion timer variable as we'll be counting it down per frames
        rb = GetComponent<Rigidbody>();

        //Get all renderer components to be hidden when in socket and out of socket
        renderers = GetComponentsInChildren<Renderer>();
        ToggleRenderers(false);
    }

    public void ToggleRenderers(bool enabled)
    {
        foreach (Renderer item in renderers)
        {
            item.enabled = enabled;
        }
    }

    public void SwitchEyeToDetached(bool doDetached)
    {
        if (doDetached)
        {
            GameManager.Instance.UpdateDetachedEyeTarget();
            GameManager.Instance.doDetachedVision();
            //Tell gamemanager to disable Cybereye Camera and enable Detached Camera and edit its target eye based on isRightEye.
            eyeIsSpawned = true;
            GameManager.Instance.hideViewport("cyber", true);
        } else
        {
            //If not, we want to return vision to the Right Camera
            GameManager.Instance.UpdateDetachedEyeTarget();
            GameManager.Instance.doAttachedVision();
            eyeIsSpawned = false;
            GameManager.Instance.hideViewport("cyber", false);
        }
    }

    public void Test(string test)
    {
        Debug.Log("TEST " + test);
    }

    private void Update()
    {
        //While eye is spawned, keep checking for the velocity to be 0 for multiple seconds uninterrupted, then unhide viewport.
        if (eyeIsSpawned)
        {
            Debug.Log(rb.velocity);
            stillMotionTimer -= Time.deltaTime; //Subtract from change in time, so once it hits 0 after X seconds

            if (rb.velocity != Vector3.zero)
            {
                stillMotionTimer = stillMotionMax; //Reset timer
                eyeSetDown = false;
            }

            //If the timer hits 0, we have been still for a second, so can unveil the eye prefab.
            if ((stillMotionTimer < 0f) && !eyeSetDown)
            {
                eyeSetDown = true;
                ResetOrientation();
                GameManager.Instance.hideViewport("cyber", false);

            }

            //If eye goes back to being moved around again, hide the viewport
            if (!eyeSetDown)
            {
                GameManager.Instance.hideViewport("cyber", true);
            }

        }
    }

    private void ResetOrientation()
    {
        //Reset velocity and rotation immediately
        rb.velocity = Vector3.zero;
        transform.rotation = Quaternion.identity;
    }
}
