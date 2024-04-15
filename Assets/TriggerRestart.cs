using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class TriggerRestart : MonoBehaviour
{
    [SerializeField] private UnityEvent OnTriggered;
    [SerializeField] private LayerMask layersToCheck;
    [SerializeField] private float timeUntilRestart = 3f;
    public bool isActive = false;

    public void setTriggerActive(bool isOn)
    {
        isActive = isOn;
    }

    private void OnTriggerStay(Collider other)
    {
        //First check if player has entered trigger while it is Active
        if ((((1 << other.gameObject.layer) & layersToCheck) != 0) && (isActive))
        {
            //Trigger the Unity Event, start a timer and on completion, restart the level.
            OnTriggered.Invoke();
            StartCoroutine(resetLevel());
        }
    }

    private IEnumerator resetLevel()
    {
        yield return new WaitForSecondsRealtime(timeUntilRestart);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
