using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityRoomGuard : MonoBehaviour
{
    //Trigger these methods in the Level2SecurityRoomGuard animation
    [SerializeField] private TriggerRestart zoneObject;

    void AwayFromCamera()
    {
        zoneObject.setTriggerActive(false);
    }

    void LookingAtCamera()
    {
        zoneObject.setTriggerActive(true);
    }
}
