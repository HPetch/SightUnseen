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
    }

    //For removing a prompt from the controller.

    //For clearing all prompts from the controller immediately. Use sparingly!
    public void ClearPrompts()
    {
        foreach (GameObject item in objects)
        {
            item.SetActive(false);
        }
    }

    //For forcing a prompt to be the only one on the controller. Use carefully!

    public void ForcePrompt(string input)
    {
        ClearPrompts();
        AddPrompt(input);
    }
}
