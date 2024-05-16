using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityRoomGuard : MonoBehaviour
{
    //Trigger these methods in the Level2SecurityRoomGuard animation
    [SerializeField] private TriggerRestart zoneObject;
    [SerializeField] private Renderer[] screens;

    void AwayFromCamera()
    {
        zoneObject.setTriggerActive(false);
        foreach (Renderer i in screens)
        {
            i.GetComponent<Renderer>().enabled = false;
        }
    }

    void LookingAtCamera()
    {
        zoneObject.setTriggerActive(true);
        foreach (Renderer i in screens)
        {
            i.GetComponent<Renderer>().enabled = true;
        }
    }
}
