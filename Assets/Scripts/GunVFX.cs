using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunVFX : MonoBehaviour
{
    public GameObject vfxParent;
    public GameObject muzzleFlash;
    //// Start is called before the first frame update
    //void Start()
    //{
        
    //}

    //// Update is called once per frame
    //void Update()
    //{
        
    //}

    public void MuzzleFlash()
    {
        Instantiate(muzzleFlash, vfxParent.transform.position, vfxParent.transform.rotation);
    }
}
