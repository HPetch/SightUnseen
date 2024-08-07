using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleDestroy : MonoBehaviour
{
    [SerializeField] private GameObject objectToDestroy;
    public void doDestroy()
    {
        Destroy(objectToDestroy);
    }
}
