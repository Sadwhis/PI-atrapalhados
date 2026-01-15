using UnityEngine;
using DG.Tweening;

public class CoinAnimation : MonoBehaviour
{
 
    public float _rotationVelocidade = 180f;

    public float _subir = 0.25f;
    public float _duracao = 1f;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;

        
        transform.DOMoveY(startPos.y + _subir, _duracao).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }

    void Update()
    {
        
        transform.Rotate(Vector3.up * _rotationVelocidade * Time.deltaTime);
    }
}
