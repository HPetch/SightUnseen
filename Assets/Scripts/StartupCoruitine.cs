using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartupCoruitine : MonoBehaviour
{
    Collider col;
    // Start is called before the first frame update
    void Start()
    {
        col = this.GetComponent<Collider>();
        StartCoroutine(AliveMe());
    }

    IEnumerator AliveMe()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        col.enabled = true;
    }
}
