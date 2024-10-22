using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SetupHotkeys : MonoBehaviour
{
    public HVRInputActions _globalInputs;

    public AnimateOnboardingUI _animator;

    public GameObject babyModeObj;
    public GameObject normalModeObj;

    public OnboardingSettingsSO _settingsSO;

    // Start is called before the first frame update
    void Awake()
    {
        _globalInputs = new HVRInputActions();
        _settingsSO.babyMode = false;
    }

    private void OnEnable()
    {
        //debug
        _globalInputs.KeyboardHotkeys.SetupBabyMode.started += DoInput;

        _globalInputs.KeyboardHotkeys.Enable();
    }

    private void OnDisable()
    {
        //playerControls.PlayerMainControls.SpecialActivation.started -= DoSpecialActivation;
    }

    void DoInput(InputAction.CallbackContext obj)
    {
        if(_settingsSO.babyMode == false)
        {
            _settingsSO.babyMode = true;
            _animator.nextSlide = babyModeObj;
        }
        else
        {
            _settingsSO.babyMode = false;
            _animator.nextSlide = normalModeObj;
        }
    }
}
