using UnityEngine;
using DG.Tweening;
using TMPro;

public class HudManager : MonoBehaviour
{
    public static HudManager instance;

    [Header("Configuração do Painel")]
    [SerializeField] private CanvasGroup _painelInstrucoes;
    [SerializeField] private RectTransform _painelTutorial; 
    [SerializeField] private RectTransform _botaoDuvidaPos;
    [SerializeField] private float _tempoVisivel = 5f;
    [SerializeField] private float _tempoParaSumir = 1f;

    [Header("Pontuação na Tela")]
    [SerializeField] private TextMeshProUGUI _textoPontuacao;

    private bool _tutorialAberto = false;
    private Vector3 _posicaoOriginalPainel;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        
        if (_painelTutorial != null)
        {
            _posicaoOriginalPainel = _painelTutorial.localPosition;
            _painelTutorial.gameObject.SetActive(false);
            _painelTutorial.localScale = Vector3.zero; 
        }

      
        if (_painelInstrucoes != null)
        {
            _painelInstrucoes.gameObject.SetActive(true);
            _painelInstrucoes.alpha = 1f;
            _painelInstrucoes.DOFade(0, _tempoParaSumir).SetDelay(_tempoVisivel)
                             .OnComplete(() => _painelInstrucoes.gameObject.SetActive(false));
        }

        if (_textoPontuacao != null) _textoPontuacao.text = "0";

        TravarMouse();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Escape))
        {
            AtivaPainelDuvida();
        }
    }

    public void AtivaPainelDuvida()
    {
        if (_painelTutorial == null || _botaoDuvidaPos == null) return;

        _tutorialAberto = !_tutorialAberto;

        
        _painelTutorial.DOKill();

        if (_tutorialAberto)
        {
            _painelTutorial.gameObject.SetActive(true);
            DestravarMouse();

            
            _painelTutorial.position = _botaoDuvidaPos.position;
            _painelTutorial.localScale = Vector3.zero; 

          
            _painelTutorial.DOLocalMove(Vector3.zero, 1f).SetEase(Ease.OutBack).SetUpdate(true);
            _painelTutorial.DOScale(1f, 0.5f).SetEase(Ease.OutBack).SetUpdate(true);
            _painelTutorial.GetComponent<CanvasGroup>().DOFade(1f, 0.3f).SetUpdate(true);
        }
        else
        {
            TravarMouse();

            
            _painelTutorial.DOMove(_botaoDuvidaPos.position, 0.4f).SetEase(Ease.InBack).SetUpdate(true);
            _painelTutorial.DOScale(0f, 0.4f).SetEase(Ease.InBack).SetUpdate(true);
            _painelTutorial.GetComponent<CanvasGroup>().DOFade(0f, 0.3f).SetUpdate(true)
                           .OnComplete(() => _painelTutorial.gameObject.SetActive(false));
        }
    }

    private void TravarMouse() { Cursor.lockState = CursorLockMode.Locked; Cursor.visible = false; }
    private void DestravarMouse() { Cursor.lockState = CursorLockMode.None; Cursor.visible = true; }
}