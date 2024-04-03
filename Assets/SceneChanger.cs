using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public int buildIndexTarget;
    public float timeBeforeNextLevel = 1f;

    public void ButtonPushed()
    {
        StartCoroutine(DelayBeforeAct());
    }

    private IEnumerator DelayBeforeAct()
    {
        yield return new WaitForSeconds(timeBeforeNextLevel);
        SceneManager.LoadScene(buildIndexTarget);

    }

}
