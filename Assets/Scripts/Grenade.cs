using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Grenade : MonoBehaviour
{
    // Thanks to https://youtu.be/rQG9aUWarwE for implementation
    public float timeBeforeExplosion = 3f;
    public bool isEMP = false;
    public Color imageColour = Color.black;
    public GameObject canvasToSpawn;
    private ParticleSystem particles;
    private GameObject flashbang;
    private bool successfulHit = false; //Enable to true if both it sees us and the player sees it.
   
    public float viewRadius;
    [Range(0,360)] public float viewAngle;

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    public List<Transform> visibleTargets = new List<Transform>();

    private void Start()
    {
        particles = GetComponentInChildren<ParticleSystem>();
        StartCoroutine(FindTargetsWithDelay(timeBeforeExplosion));
    }
    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
            Explode();
        }
    }

    void FindVisibleTargets()
    {
        visibleTargets.Clear();
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward,dirToTarget) < viewAngle / 2)
            {
                float distToTarget = Vector3.Distance(transform.position, target.position);
                if (!Physics.Raycast(transform.position, dirToTarget, distToTarget, obstacleMask))
                {
                    visibleTargets.Add(target);
                }
            }
        }

        //If it exists, trigger the grenade receiver script to see if it too can see the grenade. If so, enable a bool
        foreach (Transform target in visibleTargets)
        {
            if (target.GetComponent<GrenadeReceiver>() != null)
            {
                List<Transform> playerVisibleGrenades = target.GetComponent<GrenadeReceiver>().FindVisibleTargets();

                //Find this grenade in the list, if it is, then we have hit it.
                foreach (Transform grenade in playerVisibleGrenades)
                {
                    if (grenade == transform)
                    {
                        successfulHit = true;
                    }
                }
            }
        }

    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    private void Explode()
    {
        particles.Play(); //Play particle system
        StartCoroutine(timeToDestroy(1.2f));
        string layerText = ""; //What's the name of the layer mask for this to be in

        //Check if we actually hit in the FindVisibleTargets
        if (successfulHit)
        {

            //If an EMP, only affect the cyber eye currently active.
            if (isEMP)
            {
                if (GameManager.Instance.isRightEye)
                    layerText = "RightEyeHMD";
                else
                    layerText = "LeftEyeHMD";

                //Get camera
                Camera thisCam = GameManager.Instance.getActiveCyberCamera();

                flashbang = Instantiate(canvasToSpawn, thisCam.transform);
                flashbang.GetComponentInChildren<Image>().color = imageColour;

                flashbang.GetComponent<Canvas>().worldCamera = thisCam;

                //Update canvas and children to use correct layer mask so visible in 1 eye only
                flashbang.layer = LayerMask.NameToLayer(layerText);
                foreach (Transform child in flashbang.GetComponentsInChildren<Transform>())
                {
                    child.gameObject.layer = LayerMask.NameToLayer(layerText);
                }

            }
            else
            {
                //Define which eye is the normal eye
                if (!GameManager.Instance.isRightEye)
                    layerText = "RightEyeHMD";
                else
                    layerText = "LeftEyeHMD";

                Camera thisCam = GameManager.Instance.getActiveNormalCamera();

                flashbang = Instantiate(canvasToSpawn, thisCam.transform);
                flashbang.GetComponentInChildren<Image>().color = imageColour;

                flashbang.GetComponent<Canvas>().worldCamera = thisCam;

                //Update canvas and children to use correct layer mask so visible in 1 eye only
                flashbang.layer = LayerMask.NameToLayer(layerText);
                foreach (Transform child in flashbang.GetComponentsInChildren<Transform>())
                {
                    child.gameObject.layer = LayerMask.NameToLayer(layerText);
                }
            }
        }
    }

    private IEnumerator timeToDestroy(float seconds)
    {
        //Wait for seconds, then destroy object
        yield return new WaitForSeconds(seconds);
        Destroy(flashbang);
        Destroy(gameObject);
    }
}
