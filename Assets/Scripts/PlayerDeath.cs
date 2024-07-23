using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDeath : MonoBehaviour
{
    public Image fadeImage;
    public Transform lastCheckpoint;
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
            fadeImage.DOFade(1, 1).OnComplete(MovePlayerToCheckpoint);
        }

        if (other.gameObject.CompareTag("Checkpoint"))
        {
            lastCheckpoint = other.transform;
        }
    }

    void MovePlayerToCheckpoint()
    {
        transform.position = lastCheckpoint.position;
        fadeImage.DOFade(0, 1);
    }
}
