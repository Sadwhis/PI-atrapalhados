using UnityEngine;
using DG.Tweening;

public class ExclamationFloat : MonoBehaviour
{
    [SerializeField] private float moveDistance = 0.2f;
    [SerializeField] private float duration = 0.5f;

    private Vector3 startPos;

    private void Start()
    {
        startPos = transform.localPosition;

        transform.DOLocalMoveY(startPos.y + moveDistance, duration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);

        //transform.DOScale(1.1f, 0.8f)
            //.SetEase(Ease.InOutSine)
            //.SetLoops(-1, LoopType.Yoyo);


    }
}