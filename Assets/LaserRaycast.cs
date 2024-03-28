using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LaserRaycast : MonoBehaviour
{
    //Credit to: https://www.youtube.com/watch?v=hUg3UfE186Q

    private LineRenderer line;
    [SerializeField] private LayerMask ignoreLayers;
    [SerializeField] private bool isCybereyeOnly = true;

    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();
        StartCoroutine(SwitchLayerAfterInit());
    }

    private IEnumerator SwitchLayerAfterInit()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        if (isCybereyeOnly) gameObject.layer = 30;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position,transform.forward, out hit, 5000, ~ignoreLayers))
        {
            if (hit.collider)
            {
                line.SetPosition(1, new Vector3(0, 0, hit.distance));
            }
            else
            {
                line.SetPosition(1, new Vector3(0, 0, 5000));
            }
        }
    }
}
