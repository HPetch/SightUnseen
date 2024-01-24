using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private Movement movement;
    private MouseLook mouseLook;

    DummyPlayer controls;
    DummyPlayer.DummyActionsActions groundActions;

    Vector2 horizontalInput;
    Vector2 mouseInput;

    //Initialise input actions
    void Awake()
    {
        movement = GetComponent<Movement>();
        mouseLook = GetComponent<MouseLook>();
        controls = new DummyPlayer();
        groundActions = controls.DummyActions;

        // groundActions.[action].performed += context => do something
        groundActions.HorizontalMovement.performed += context => horizontalInput = context.ReadValue<Vector2>();
        groundActions.Jump.performed += _ => movement.onJumpPressed();
        groundActions.MouseX.performed += context => mouseInput.x = context.ReadValue<float>();
        groundActions.MouseY.performed += context => mouseInput.y = context.ReadValue<float>();
    }


    void Update()
    {
        movement.ReceiveInput(horizontalInput);
        mouseLook.ReceiveInput(mouseInput);
    }


    //Enable and disable control actions when you go in and out
    private void OnEnable()
    {
        controls.Enable();
    }
    private void OnDisable()
    {
        controls.Disable();
    }
}
