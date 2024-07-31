using HurricaneVR.Framework.Core;
using HurricaneVR.Framework.Core.Grabbers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceEyeToGun : MonoBehaviour
{
    HVRSocket mySocket;
    // Start is called before the first frame update
    void Start()
    {
        mySocket = GetComponent<HVRSocket>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Eyeball"))
    //    {
    //        HVRHandGrabber hand = other.gameObject.GetComponent<ConfigurableJoint>().connectedBody.gameObject.GetComponent<HVRHandGrabber>();
    //        hand.ForceRelease();
    //    }
    //}
}
