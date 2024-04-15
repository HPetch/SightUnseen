using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HurricaneVR.Framework.Components;

public class TrackDoor : MonoBehaviour
{
    private int currentScore;
    [SerializeField] private HVRPhysicsDoor Door1;
    [SerializeField] private int ScoreThreshold1;
    [SerializeField] private HVRPhysicsDoor Door2;
    [SerializeField] private int ScoreThreshold2;


    //Call from enemies, add to score then check if it's over one of the thresholds, if so, open that door.
    public void addScore()
    {
        currentScore++;

        if (currentScore > ScoreThreshold1)
        {
            Door1.Unlock();
        }
        if (currentScore > ScoreThreshold2)
        {
            Door2.Unlock();
        }
    }
}
