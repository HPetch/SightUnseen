using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HurricaneVR.Framework.Core.Player;
using DG.Tweening;
using UnityEngine.SceneManagement;
using TMPro;
public class DemoTimer : MonoBehaviour
{
    public float timerToStartup;
    public HVRCanvasFade fader;
    public float baseTimer = 600f;
    public int extraTimers;

    [SerializeField] private CanvasGroup canvasGroup;
    public TextMeshProUGUI endText;

    void Start()
    {
        canvasGroup.alpha = 0f;
    }

    public void StartTimer()
    {
        StartCoroutine(BaseTimer());
    }

    public void AddExtraTime()
    {
        extraTimers++;
    }

    IEnumerator BaseTimer()
    {
        yield return new WaitForSecondsRealtime(baseTimer);
        if(extraTimers > 0)
        {
            StartCoroutine(ExtraTime());
        }
        else
        {
            EndDemo();
        }
    }

    IEnumerator ExtraTime()
    {
        yield return new WaitForSecondsRealtime(60);
        extraTimers--;
        if (extraTimers > 0)
        {
            StartCoroutine(ExtraTime());
        }
        else
        {
            EndDemo();
        }
    }

    public void EndDemo()
    {
        fader.Fade(1, 1);
    }

    public void ShowEndText()
    {
        //canvasGroup.alpha = 0f;
        endText.DOFade(1, 1).SetUpdate(UpdateType.Normal, true).OnComplete(ResetTimer);
    }

    void ResetTimer()
    {
        StartCoroutine(BackToSetup());
    }

    IEnumerator BackToSetup()
    {
        yield return new WaitForSecondsRealtime(timerToStartup);
        SceneManager.LoadScene(0);
    }
}
