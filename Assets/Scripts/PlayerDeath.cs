using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDeath : MonoBehaviour
{
    public Image fadeImage;
    public Transform lastCheckpoint;
    public int deathCounter;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Laser"))
        {
            deathCounter++;
            fadeImage.DOFade(1, 1).OnComplete(MovePlayerToCheckpoint);
        }

        if (other.gameObject.CompareTag("Checkpoint"))
        {
            lastCheckpoint = other.transform;
            deathCounter = 0;
        }
    }

    void MovePlayerToCheckpoint()
    {
        transform.position = lastCheckpoint.position;
        fadeImage.DOFade(0, 1);
    }
}
