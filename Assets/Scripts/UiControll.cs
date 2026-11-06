using DG.Tweening;
using UnityEngine;

public class UiControll : MonoBehaviour
{
    public CanvasGroup CanvasGroup;
    public float duration;
    void Start()
    {
        CanvasGroup.DOFade(1f, duration);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
