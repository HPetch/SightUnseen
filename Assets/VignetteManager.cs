using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class VignetteManager : MonoBehaviour
{
    [Range(0f,1f)] [SerializeField] private float animationRate = 0.25f;
    private PostProcessVolume volume;
    [SerializeField] private CharacterController playerController;
    [SerializeField] private Toggle settingsToggle;

    private void Start()
    {
        volume = GetComponent<PostProcessVolume>();

    }

    private void Update()
    {
        if (playerController != null)
        {
            isMoving(playerController.velocity);
        }
    }

    public void ToggleActive()
    {
        volume.enabled = settingsToggle.isOn;
    }

    public void isMoving(Vector3 velocity)
    {
        //If velocity = 0, subtract from volume weight until it gets to 0
        //If velocity != 0, add to volume weight until it gets to 1

        if (velocity != Vector3.zero)
            volume.weight = Mathf.Min(volume.weight + (animationRate * Time.deltaTime), 1f);
        else
          volume.weight = Mathf.Max(volume.weight - (animationRate * Time.deltaTime), 0f);

    }
}
