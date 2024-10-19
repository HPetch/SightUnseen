using HurricaneVR.Framework.ControllerInput;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class KeyboardHotkeys : MonoBehaviour
{
    public UnityEvent Respawn = new UnityEvent();
    public UnityEvent EndDemo = new UnityEvent();
    //public UnityEvent HotswitchEyeToggle = new UnityEvent();
    //public UnityEvent HotswitchGlassesToggle = new UnityEvent();
    //public UnityEvent HotswitchSubtitlesToggle = new UnityEvent();
    //public UnityEvent HotswitchDomHandToggle = new UnityEvent();
    public UnityEvent HotswitchCalibrateHeightToggle = new UnityEvent();

    public HVRInputActions _globalInputs;
    private bool _hasInputs;

    private void Awake()
    {
        _globalInputs = new HVRInputActions();
    }

    // Start is called before the first frame update
    void Start()
    {
        //_globalInputs = new HVRInputActions();
    }

    // Update is called once per frame
    void Update()
    {

    }



    private void OnEnable()
    {
        //debug
        _globalInputs.KeyboardHotkeys.Respawn.started += DoRespawn;
        _globalInputs.KeyboardHotkeys.EndDemo.started += DoEndDemo;
        _globalInputs.KeyboardHotkeys.BackToSetup.started += DoBackToSetup;
        _globalInputs.KeyboardHotkeys.EyeToggle.started += DoEyeToggle;
        _globalInputs.KeyboardHotkeys.GlassesToggle.started += DoGlassesToggle;
        _globalInputs.KeyboardHotkeys.CalibrateHeight.started += DoHeightToggle;
        _globalInputs.KeyboardHotkeys.HandToggle.started += DoHandToggle;
        _globalInputs.KeyboardHotkeys.SubtitlesToggle.started += DoSubtitlesToggle;

        _globalInputs.KeyboardHotkeys.Enable();
    }

    private void OnDisable()
    {
        //playerControls.PlayerMainControls.SpecialActivation.started -= DoSpecialActivation;
    }

    public void DoRespawn(InputAction.CallbackContext obj)
    {
        Respawn.Invoke();
    }

    public void DoEndDemo(InputAction.CallbackContext obj)
    {
        EndDemo.Invoke();
    }

    public void DoBackToSetup(InputAction.CallbackContext obj)
    {
        SceneManager.LoadScene(0);
    }

    public void DoEyeToggle(InputAction.CallbackContext obj)
    {
        if (GameManager.Instance.isDouble.isOn == false)
        {
            if (GameManager.Instance.isRightEye)
            {
                GameManager.Instance.eyeChoice.value = 1;
            }
            else
            {
                GameManager.Instance.eyeChoice.value = 0;
            }
            GameManager.Instance.eyeChoiceToggle();
        }
    }

    public void DoGlassesToggle(InputAction.CallbackContext obj)
    {
        if (GameManager.Instance.isDouble.isOn)
        {
            GameManager.Instance.isDouble.isOn = false;
        }
        else
        {
            GameManager.Instance.isDouble.isOn = true;
        }
        GameManager.Instance.ToggleDoubleCybereyes();
    }

    public void DoHandToggle(InputAction.CallbackContext obj)
    {
        if (GameManager.Instance.rightHandRecall.isOn)
        {
            GameManager.Instance.rightHandRecall.isOn = false;
        }
        else
        {
            GameManager.Instance.rightHandRecall.isOn = true;
        }
        GameManager.Instance.ToggleRecallHand();
    }

    public void DoHeightToggle(InputAction.CallbackContext obj)
    {
        HotswitchCalibrateHeightToggle.Invoke();
    }


    //ignore me for now
    public void DoSubtitlesToggle(InputAction.CallbackContext obj)
    {
        //GameManager.Instance.SubtitlesToggle();
    }
}
