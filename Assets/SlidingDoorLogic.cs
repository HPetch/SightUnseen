using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingDoorLogic : MonoBehaviour
{
    [SerializeField] private BoxCollider doorTrigger;
    private Animator animator;
    [SerializeField] private bool isOpenable;
    [SerializeField] private LayerMask layersToCheck;
    public bool isActive = true;
    private bool opening = false;

    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (doorTrigger != null && isOpenable)
        {
            if ((((1 << other.gameObject.layer) & layersToCheck) != 0) && (isActive))
            {
                openDoor();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (doorTrigger != null && isOpenable)
        {
            if ((((1 << other.gameObject.layer) & layersToCheck) != 0) && (isActive))
            {
                closeDoor();
            }
        }
    }

    public void openDoor()
    {
        if (!opening)
        {
            animator.SetBool("isOpening", true);
            opening = true;
        }
    }

    public void openDoorButtonTimer(bool Permanent)
    {
        if (!opening)
        {
            animator.SetBool("isOpening", true);
            opening = true;

            if (!Permanent)
            StartCoroutine(countdownToClose());

        }
    }

    public void closeDoor()
    {
        if (opening)
        {
            animator.SetBool("isOpening", false);
            opening = false;
        }
    }

    IEnumerator countdownToClose()
    {
        yield return new WaitForSeconds(5f);
        closeDoor();
    }
}
