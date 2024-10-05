using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnityEventOnAnimEvent : MonoBehaviour
{
    public UnityEvent myEvent;
    public void TriggerEvent()
    {
        myEvent.Invoke();
    }
}
