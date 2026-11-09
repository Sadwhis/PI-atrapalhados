using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class UiControll : MonoBehaviour
{
    [SerializeField] private float scaleFactor = 1.2f;
    [SerializeField] private float durantion = 0.5f;
    private Vector3 originalScale;
    void Start()
    {
        originalScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnPointerEnter()
    {
        transform.DOScale(endValue: originalScale * scaleFactor, durantion).SetEase(Ease.OutQuad);
    }
    public void OnPointerExit()
    {
        transform.DOScale(originalScale, durantion).SetEase(Ease.OutQuad);
    }


}

