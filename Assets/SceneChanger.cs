using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public int buildIndexTarget;
    public float timeBeforeNextLevel = 1f;
    [SerializeField] private bool doOnEnable = false;

    private void OnEnable()
    {
        if (doOnEnable) StartCoroutine(DelayBeforeAct());
    }

    public void ButtonPushed()
    {
        StartCoroutine(DelayBeforeAct());
    }

    private IEnumerator DelayBeforeAct()
    {
        yield return new WaitForSeconds(timeBeforeNextLevel);
       AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(buildIndexTarget);

        while (!asyncOperation.isDone)
        {
            Debug.Log("Progress = " + asyncOperation.progress);
            yield return null;
        }

    }

}
