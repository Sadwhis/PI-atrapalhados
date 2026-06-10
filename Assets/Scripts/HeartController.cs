using UnityEngine;
using DG.Tweening;

public class HeartController : MonoBehaviour
{
    [Header("Configurações de Vida")]
    public int vida;
    public int vidaMaxima;

    [Header("Arrays de Objetos (UI)")]
    public GameObject[] coracoesCheios;
    public GameObject[] coracoesVazioObj;

    [Header("Componentes para Animação de Entrada")]
    [SerializeField] private RectTransform _fundoDaBarra; 
    [SerializeField] private RectTransform _rostoDoce;    

    [Header("Ajustes de Tempo")]
    [SerializeField] private float _duracaoFundo = 0.6f;
    [SerializeField] private float _duracaoRostoGiro = 0.5f;
    [SerializeField] private float _duracaoBrotoCoracao = 0.35f;
    [SerializeField] private float _atrasoEntreCoracoes = 0.12f;

    void Start()
    {
        vida = vidaMaxima;

        
        AtualizarInterfaceSemAnimar();

        
        IniciarAnimacaoIntroducao();
    }

    public void TomarDano(int quantidade)
    {
        if (vida <= 0) return;

        vida -= quantidade;

        if (AudioPlayer.Instance != null)
        {
            AudioPlayer.Instance.SomStart(1);
        }

        AtualizarInterfaceSemAnimar();

        if (vida <= 0)
        {
            Morrer();
        }
    }

    void Morrer()
    {
        Debug.Log("O Player morreu");
        AudioPlayer.Instance.SomStart(2);
    }

    public void AtualizarInterfaceSemAnimar()
    {
        for (int i = 0; i < coracoesCheios.Length; i++)
        {
            if (i < vidaMaxima)
            {
                bool estaAtivo = (i < vida);
                coracoesCheios[i].SetActive(estaAtivo);
                coracoesVazioObj[i].SetActive(!estaAtivo);
            }
            else
            {
                coracoesCheios[i].SetActive(false);
                coracoesVazioObj[i].SetActive(false);
            }
        }
    }

    private void IniciarAnimacaoIntroducao()
    {
        Sequence seq = DOTween.Sequence();

        //ANIMAÇÃO DO FUNDO
        if (_fundoDaBarra != null)
        {
            Vector2 posicaoFinalFundo = _fundoDaBarra.anchoredPosition;
            _fundoDaBarra.anchoredPosition = new Vector2(-Screen.width, posicaoFinalFundo.y);
            seq.Append(_fundoDaBarra.DOAnchorPos(posicaoFinalFundo, _duracaoFundo).SetEase(Ease.OutCubic));
        }

        //ANIMAÇÃO DO ROSTO
        if (_rostoDoce != null)
        {
            
            Vector3 escalaOriginalRosto = _rostoDoce.localScale;
            _rostoDoce.localScale = Vector3.zero;
            seq.Append(_rostoDoce.DOScale(escalaOriginalRosto, 0.2f).SetEase(Ease.OutBack));
            seq.Append(_rostoDoce.DORotate(new Vector3(0, 0, -360f), _duracaoRostoGiro, RotateMode.FastBeyond360).SetEase(Ease.OutQuad));
        }

 
        float[] posicoesYOriginais = new float[coracoesCheios.Length];

  
        for (int i = 0; i < coracoesCheios.Length; i++)
        {
            GameObject coracaoAtual = coracoesCheios[i].activeSelf ? coracoesCheios[i] : coracoesVazioObj[i];

            if (coracaoAtual != null)
            {
                RectTransform rect = coracaoAtual.GetComponent<RectTransform>();
                posicoesYOriginais[i] = rect.anchoredPosition.y;

                rect.localScale = Vector3.zero;
                rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, posicoesYOriginais[i] - 30f);
            }
        }

        seq.AppendInterval(0.05f);

        for (int i = 0; i < coracoesCheios.Length; i++)
        {
            GameObject coracaoAtual = coracoesCheios[i].activeSelf ? coracoesCheios[i] : coracoesVazioObj[i];

            if (coracaoAtual != null)
            {
                RectTransform rectCoracao = coracaoAtual.GetComponent<RectTransform>();

                float tempoDeEspera = i * _atrasoEntreCoracoes;
                float metadeDoTempo = _duracaoBrotoCoracao / 2f;

          
                seq.Join(rectCoracao.DOScale(1f, _duracaoBrotoCoracao)
                    .SetEase(Ease.OutBack)
                    .SetDelay(tempoDeEspera));

         
                seq.Join(rectCoracao.DOAnchorPosY(posicoesYOriginais[i] + 50f, metadeDoTempo)
                    .SetEase(Ease.OutQuad)
                    .SetDelay(tempoDeEspera));

                seq.Join(rectCoracao.DOAnchorPosY(posicoesYOriginais[i], metadeDoTempo)
                    .SetEase(Ease.InQuad)
                    .SetDelay(tempoDeEspera + metadeDoTempo));
            }
        }

        seq.OnComplete(() =>
        {
            RostoMovimento();
        });

    }

    private void RostoMovimento()
    {
        if (_rostoDoce != null)
        {
            _rostoDoce.DOComplete();
            _rostoDoce.DORotate(new Vector3(0, 0, 4f), 2f)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo);
        }
    }
}