using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Level3DestroyTurret : MonoBehaviour
{
    public UnityEvent onDeath;
    //This script only triggers when a destroyed enemy is spawned, so we can assume the enemy is already dead
    private void OnDestroy()
    {
        onDeath.Invoke();
    }

}
