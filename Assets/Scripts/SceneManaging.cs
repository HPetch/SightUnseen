using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManaging : MonoBehaviour
{
    public void LoadScene(int buildIndex)
    {
        SceneManager.LoadScene(buildIndex);
    }

    public void LoadNextScene()
    {
        LoadSceneDelayed(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadSceneDelayed(int buildIndex)
    {
        StartCoroutine(DelayBeforeSwitch(1f, buildIndex));
    }

    private IEnumerator DelayBeforeSwitch(float timeBeforeNextLevel, int buildIndex)
    {
        yield return new WaitForSeconds(timeBeforeNextLevel);
        SceneManager.LoadScene(buildIndex);

    }
}
