using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class UiControll : MonoBehaviour
{
    [SerializeField] private float scaleFactor = 1.2f;
    [SerializeField] private float durantion = 0.5f;
    private Vector3 originalScale;
    [SerializeField] Transform Titulo;
    void Start()
    {
        originalScale = transform.localScale;
        Titulo.DOShakeScale(3.0f, new Vector3(0.9f, 0.5f), 2, 0.5f).SetLoops(-1, LoopType.Restart);

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

