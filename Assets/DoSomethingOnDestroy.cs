using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DoSomethingOnDestroy : MonoBehaviour
{
    public UnityEvent myEvent;

    private void OnDestroy()
    {
        myEvent.Invoke();
    }
}
