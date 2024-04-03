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
}
