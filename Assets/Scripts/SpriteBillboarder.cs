using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteBillboarder : MonoBehaviour
{
    [SerializeField] bool freezeXZAxis = true;
    [SerializeField] private GameObject objectToFace;

    // Update is called once per frame
    void Update()
    {
        if (freezeXZAxis)
        {
            transform.rotation = Quaternion.Euler(0f, objectToFace.transform.rotation.eulerAngles.y, 0f);
        }
        else
        {
            transform.rotation = objectToFace.transform.rotation;
        }
    }
}
