using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GenericTriggerEvents : MonoBehaviour
{
    [SerializeField] private UnityEvent OnTriggered;
    [SerializeField] private LayerMask layersToCheck;
    public bool isActive = true;
    [SerializeField] private GameObject[] objectsToDisable;

    public void disableAllObjectsInArray()
    {
        foreach (GameObject item in objectsToDisable)
        {
            item.SetActive(false);
        }
    }

    public void setTriggerActive(bool isOn)
    {
        isActive = isOn;
    }

    private void OnTriggerStay(Collider other)
    {
        //First check if player has entered trigger while it is Active
        if ((((1 << other.gameObject.layer) & layersToCheck) != 0) && (isActive))
        {
            //Trigger the Unity Event, start a timer and on completion, restart the level.
            OnTriggered.Invoke();
        }
    }
}
