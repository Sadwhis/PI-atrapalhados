using UnityEngine;
using DG.Tweening;
using TMPro;

public class HudManager : MonoBehaviour
{
    public static HudManager instance; // Permite que outros scripts achem o HUD fácil

    [Header("Configuração do Painel")]
    [SerializeField] private CanvasGroup _painelInstrucoes;
    [SerializeField] private float _tempoVisivel = 5f;
    [SerializeField] private float _tempoParaSumir = 1f;

    [Header("Pontuação na Tela")]
    [SerializeField] private TextMeshProUGUI _textoPontuacao;

    void Awake()
    {
        instance = this; // Liga a instância quando o jogo começa
    }

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

        // Deixa o texto em 0 logo que o jogo começa
        if (_textoPontuacao != null)
            _textoPontuacao.text = "0";
    }

    // O GameManager vai chamar essa função quando a Princesa pegar a moeda!
    public void AnimarTextoPontos(int totalMoedas)
    {
        if (_textoPontuacao != null)
        {
            _textoPontuacao.text = totalMoedas.ToString(); // Atualiza o número

            // O efeito mágico do DOTween: dá um "soco/pulo" na escala e volta ao normal
            _textoPontuacao.transform.DOPunchScale(new Vector3(0.5f, 0.5f, 0), 0.3f, 10, 1);
        }
    }
}