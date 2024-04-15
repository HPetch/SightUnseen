using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathCounterIterator : MonoBehaviour
{
    private TrackDoor trackDoor;
    
    //This script only triggers when a destroyed enemy is spawned, so we can assume the enemy is already dead
    private void Start()
    {
        //Very inefficient way of doing this, but sometimes you just need to duct tape.
        trackDoor = GameObject.Find("DoorOpener").GetComponent<TrackDoor>();

        trackDoor.addScore();
    }

}
