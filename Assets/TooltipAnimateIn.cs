using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TooltipAnimateIn : MonoBehaviour
{
    [SerializeField] private Ease easing = Ease.OutExpo;
    [SerializeField] private float time = 1f;
    private Vector3 originalScale;

    //Every time the tooltip is drawn, animate it in
    private void OnEnable()
    {
        originalScale = transform.localScale;
        transform.localScale = Vector3.zero;
        transform.DOScale(originalScale, time).SetEase(easing);
    }
}
