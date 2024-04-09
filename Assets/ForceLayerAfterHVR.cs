using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceLayerAfterHVR : MonoBehaviour
{
    public int layerToBe;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DoReassign());
    }

    private IEnumerator DoReassign()
    {
        yield return new WaitForSeconds(0.05f);
        gameObject.layer = layerToBe;
    }

    private void FixedUpdate()
    {
        //HVR likes to update the layer constantly. Stop doing that! Bad HVR! Keep checking and uncheck it if it changes. This is unoptimal code, but its the foundations we have.
        if (gameObject.layer != layerToBe)
        {
            gameObject.layer = layerToBe;
        }

    }
}
