using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnimateOnboardingStart : MonoBehaviour
{
    [SerializeField] private GameObject activateImmediately;
    [SerializeField] private GameObject eyeballButton;
    // Start is called before the first frame update
    void Start()
    {
        activateImmediately.GetComponentInChildren<AnimateOnboardingUI>().TweenIn();
    }

    private void Update()
    {
        //Deactivate the eyeball from being there if we click a special button on the keyboard
        if (Keyboard.current.backspaceKey.wasPressedThisFrame)
        {
            eyeballButton.SetActive(false);
        }
    }

}
