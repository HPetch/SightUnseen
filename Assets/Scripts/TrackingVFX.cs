using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingVFX : MonoBehaviour
{
    public float speed;
    bool canMove;
    bool isActive = false;
    public GameObject objToFollow;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(moveDelay());
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            transform.position = Vector3.MoveTowards(transform.position, Offset(), speed * Time.deltaTime);
        }
        if (isActive == true && Vector3.Distance(transform.position, objToFollow.transform.position) < 1)
        {
            isActive = false;
            this.GetComponent<ParticleSystem>().Stop();
            StartCoroutine(DestoryMe());
        }
    }

    public Vector3 Offset()
    {
        Vector3 target = objToFollow.transform.position;
        //target.y += 1;
        return target;
    }

    IEnumerator moveDelay()
    {
        yield return new WaitForSeconds(0.2f);
        canMove = true;
        isActive = true;
    }

    IEnumerator DestoryMe()
    {
        yield return new WaitForSeconds(3);
        Destroy(this.gameObject);
    }
}
