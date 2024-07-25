using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class AnimateOnboardingUIBranch : MonoBehaviour
{
    private GameObject parent;
    private RectTransform rect;
    private CanvasGroup canvasGroup;
    [SerializeField] private bool startInvisible = true;
    [SerializeField] private float fadeInTime = 2f;
    [SerializeField] private float fadeOutTime = 2f;
    
    //[SerializeField] private Vector2 startPos;
    //private Vector2 endPos;
    //[SerializeField] private float moveTime = 2f;
    
    [SerializeField] private Ease showEaseType;
    [SerializeField] private Ease hideEaseType;

    [SerializeField] private GameObject nextSlideEye;
    [SerializeField] private GameObject nextSlideSunglasses;

    private void Awake()
    {
        parent = transform.parent.gameObject;
        rect = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        if (startInvisible == true)
        {
            canvasGroup.alpha = 0f;
        }
    }

    private void OnEnable()
    {
        TweenIn();
    }

    public void TweenIn()
    {
        Debug.Log("Activated");
        canvasGroup.alpha = 0f;
        canvasGroup.DOFade(1f, fadeInTime);
    }

    public void TweenOutEye()
    {
        canvasGroup.DOFade(0f, fadeOutTime).OnComplete(HideToEye);
    }
    public void TweenOutSunglasses()
    {
        canvasGroup.DOFade(0f, fadeOutTime).OnComplete(HideContinue);
    }

    private void HideToEye()
    {
        nextSlideEye.SetActive(true);
        parent.SetActive(false);
        gameObject.SetActive(false); //This won't trigger, but worth having in there anyway
    }

    private void HideContinue()
    {
        nextSlideSunglasses.SetActive(true);
        parent.SetActive(false);
        gameObject.SetActive(false); //This won't trigger, but worth having in there anyway
    }

}
