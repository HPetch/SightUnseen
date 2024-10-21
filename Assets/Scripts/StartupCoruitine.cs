using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartupCoruitine : MonoBehaviour
{
    Collider col;
    public bool notVeryStart;
    // Start is called before the first frame update
    void Start()
    {
        if (notVeryStart == false)
        {
            col = this.GetComponent<Collider>();
            StartCoroutine(AliveMe());
        }
        
    }

    public void Activate()
    {
        col = this.GetComponent<Collider>();
        StartCoroutine(AliveMe());
    }

    IEnumerator AliveMe()
    {
        yield return new WaitForSecondsRealtime(2f);
        col.enabled = true;
    }
}
