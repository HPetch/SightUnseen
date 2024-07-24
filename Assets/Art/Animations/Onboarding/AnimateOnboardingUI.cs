using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class AnimateOnboardingUI : MonoBehaviour
{
    private GameObject parent;
    private RectTransform rect;
    [SerializeField] private CanvasGroup canvasGroup;
    private bool startInvisible = true;
    [SerializeField] private float fadeInTime = 2f;
    [SerializeField] private float fadeOutTime = 2f;
    
    //[SerializeField] private Vector2 startPos;
    //private Vector2 endPos;
    //[SerializeField] private float moveTime = 2f;
    
    [SerializeField] private Ease showEaseType;
    [SerializeField] private Ease hideEaseType;
    [SerializeField] private GameObject nextSlide;
    
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

    public void TweenOut()
    {
        canvasGroup.DOFade(0f, fadeOutTime).OnComplete(Hide);
    }

    private void Hide()
    {
        nextSlide.SetActive(true);
        parent.SetActive(false);
        gameObject.SetActive(false); //This won't trigger, but worth having in there anyway
    }

}
