using DG.Tweening;
using HurricaneVR.Framework.Core.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDeath : MonoBehaviour
{
    //public Image fadeImage;
    public Transform lastCheckpoint;
    public int deathCounter;
    public HVRCanvasFade fader;
    bool respawning;
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
        //if hit a laser
        if (other.gameObject.CompareTag("Laser"))
        {
            //up the death counter
            deathCounter++;
            //fadeImage.DOFade(1, 1).OnComplete(MovePlayerToCheckpoint);
            //start fading and set the respawn bool to true
            fader.Fade(1, 1);
            respawning = true;
        }

        //overwrite the players current checkpoint when hitting a checkpoint
        if (other.gameObject.CompareTag("Checkpoint"))
        {
            lastCheckpoint = other.transform;
            deathCounter = 0;
        }
    }

    public void MovePlayerToCheckpoint()
    {
        //this bool is used because an overflow happens if we dont
        if (respawning)
        {
            //move the player to the last checkpoint pos and fade the black screen out
            respawning = false;
            transform.position = lastCheckpoint.position;
            fader.Fade(0, 1);
        }
        
        //fadeImage.DOFade(0, 1);
    }
}
