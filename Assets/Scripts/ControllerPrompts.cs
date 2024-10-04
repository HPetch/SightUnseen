using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ControllerPrompts : MonoBehaviour
{
    //Set up prompts that we can choose
    public enum Prompts { analog,menu,primary,secondary,trigger,grip }
    //Array for each *prompt* (so the highlighted sphere) to show and hide. Must be in order as the above
    [SerializeField] private GameObject[] objects;
    [SerializeField] private Transform handJoint;
    [SerializeField] private Renderer controllerMesh;
    [SerializeField] private AudioSource controllerSource;
    
    //Set up an array or something to hold which billboard messages should be shown?


    // Start is called before the first frame update
    void Start()
    {
        
    }

    //For adding a prompt to the controller. Does not remove any other prompts.
    public void AddPrompt(string input)
    {
        //Unity is silly and doesn't let you invoke with enum parameters DESPITE THEM BEING STATIC. BASIC UNITY WORKFLOW CHALLENGE: IMPOSSIBLE.
        //Instead, get the prompt as a string input, try to run it in a case. If it fails, run it to the debug.log because there is definitely a typo
        Prompts prompt = (Prompts)Enum.Parse(typeof(Prompts), input);

        switch (prompt)
        {
            case Prompts.analog:
                objects[0].SetActive(true);
                break;
            case Prompts.menu:
                objects[1].SetActive(true);
                break;
            case Prompts.primary:
                objects[2].SetActive(true);
                break;
            case Prompts.secondary:
                objects[3].SetActive(true);
                break;
            case Prompts.trigger:
                objects[4].SetActive(true);
                break;
            case Prompts.grip:
                objects[5].SetActive(true);
                break;
            default:
                Debug.LogError("Cannot add prompt '" + input + "' as it is invalid. Is there a typo?");
                break;
        }
        setControllerActive(true);
        if (!controllerSource.isPlaying) controllerSource.Play();

        //Hide the hand
        setHandActive(false);
    }

    //For removing a single prompt from the controller.
    public void RemovePrompt(string input)
    {
        Prompts prompt = (Prompts)Enum.Parse(typeof(Prompts), input);

        switch (prompt)
        {
            case Prompts.analog:
                objects[0].SetActive(false);
                break;
            case Prompts.menu:
                objects[1].SetActive(false);
                break;
            case Prompts.primary:
                objects[2].SetActive(false);
                break;
            case Prompts.secondary:
                objects[3].SetActive(false);
                break;
            case Prompts.trigger:
                objects[4].SetActive(false);
                break;
            case Prompts.grip:
                objects[5].SetActive(false);
                break;
            default:
                Debug.LogError("Cannot add prompt '" + input + "' as it is invalid. Is there a typo?");
                break;
        }
        
        //Show the hand IF there is nothing else left on screen
        bool somethingIsStillEnabled = false;
        foreach (GameObject obj in objects)
        {
            if (obj.activeInHierarchy) somethingIsStillEnabled = true;
        }
        setHandActive(!somethingIsStillEnabled);
        setControllerActive(somethingIsStillEnabled);
    }

    //For clearing all prompts from the controller immediately. Use sparingly!
    public void ClearPrompts()
    {
        foreach (GameObject item in objects)
        {
            item.SetActive(false);
            setHandActive(true);
        }
    }


    private void setControllerActive(bool isVisible)
    {
        if (isVisible)
        {
            controllerMesh.enabled = true;
        }
        else controllerMesh.enabled = false;
    }

    private void setHandActive(bool isVisible)
    {
        /*
        if (isVisible)
        {
            handJoint.localScale = Vector3.one;
        }
        else
        {
            handJoint.localScale = new Vector3(0.01f,0.01f,0.01f);
        }*/
    }

    //For forcing a prompt to be the only one on the controller. Use carefully!
    public void ForcePrompt(string input)
    {
        ClearPrompts();
        AddPrompt(input);
    }

    public void ShowTooltip(TwoPointsLine tooltip)
    {
        //Change tooltip to be the correct tooltip we are affecting if this is a teleporter conditional
        if (tooltip.hasTeleporterAlternate && !GameManager.Instance.smoothMovement.isOn)
        {
            tooltip = tooltip.teleporterAlternate;
        }

        //Check if tooltip has a Point A that is associated to a relevant button. If so, we should add that button too
        Transform anchor = tooltip.pointA;

        for (int i = 0; i < objects.Length; i++)
        {
            if (anchor.gameObject == objects[i])
            {
                AddPrompt(i.ToString());
            }
        }
        
        tooltip.gameObject.SetActive(true);
    }

    public void HideTooltip(TwoPointsLine tooltip)
    {
        //Change tooltip to be the correct tooltip we are affecting if this is a teleporter conditional
        if (tooltip.hasTeleporterAlternate && !GameManager.Instance.smoothMovement.isOn)
        {
            tooltip = tooltip.teleporterAlternate;
        }

        //Check if tooltip has a Point A that is associated to a relevant button. If so, we should add that button too
        Transform anchor = tooltip.gameObject.GetComponent<TwoPointsLine>().pointA;

        for (int i = 0; i < objects.Length; i++)
        {
            if (anchor.gameObject == objects[i])
            {
                RemovePrompt(i.ToString());
            }
        }

        tooltip.gameObject.SetActive(false);
    }

}
