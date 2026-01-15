using UnityEngine;
using DG.Tweening;

public class NPC : MonoBehaviour
{
    [Header("Configurações")]
    public GameObject _TextoFrog; 
    public float tempoAnimacao = 0.5f;

    private void Start()
    {
       
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            _TextoFrog.transform.DOKill();


            _TextoFrog.SetActive(true);


            _TextoFrog.transform.DOScale(1, tempoAnimacao).SetEase(Ease.OutBack);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _TextoFrog.transform.DOKill();


            _TextoFrog.transform.DOScale(0, tempoAnimacao).SetEase(Ease.InBack).OnComplete(() =>
            {
                _TextoFrog.SetActive(false);
            });
        }
    }
}