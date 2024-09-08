using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HurricaneVR.Framework.Core;

public class VelocityTrackingDisplay : MonoBehaviour
{
    private EyeHolder eyeball;
    private HVRGrabbable eyeGrabbable;
    private Rigidbody eyeRb;
    private Slider velocitySlider;
    [SerializeField] private Text velocityText;

    // Start is called before the first frame update
    void Start()
    {
        eyeball = GetComponentInParent<EyeHolder>();
        eyeGrabbable = eyeball.GetComponent<HVRGrabbable>();
        eyeRb = eyeball.GetComponent<Rigidbody>();
        velocitySlider = GetComponentInChildren<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (eyeball.eyeIsSpawned && !eyeGrabbable.IsBeingHeld)
        {
            float val = eyeRb.velocity.magnitude;//Mathf.Abs(Vector3.Distance(eyeRb.velocity, Vector3.zero)); //Display a positive (Absolute) number of the velocity
            velocitySlider.value = val;
            velocityText.text = val.ToString("F2") + " m/s";
        } else
        {
            velocitySlider.value = 0;
            velocityText.text = "0.00 m/s";
        }
    }
}
