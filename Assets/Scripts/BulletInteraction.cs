using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletInteraction : MonoBehaviour
{
    public List<Sprite> bulletHoles;
    public List<GameObject> spawnedBullets;
    public GameObject bulletHole;
    public GameObject hitVFX;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
        Vector3 pos = contact.point;
        rot.y -= 90;
        GameObject hole = Instantiate(bulletHole, pos, rot);
        Instantiate(hitVFX, pos, rot);
        spawnedBullets.Add(hole);
        hole.transform.parent = collision.transform;

    }
}
