using UnityEngine;
using DG.Tweening;

public class HudManager : MonoBehaviour
{
    [Header("Configuração do Painel")]
    [SerializeField] private CanvasGroup _painelInstrucoes;

    [SerializeField] private float _tempoVisivel = 5f; 
    [SerializeField] private float _tempoParaSumir = 1f;

    void Start()
    {
        if (_painelInstrucoes != null)
        {
            
            _painelInstrucoes.gameObject.SetActive(true);
            _painelInstrucoes.alpha = 1f;

          
            _painelInstrucoes.DOFade(0, _tempoParaSumir)
                             .SetDelay(_tempoVisivel)
                             .OnComplete(() => _painelInstrucoes.gameObject.SetActive(false)); 
        }
    }
}