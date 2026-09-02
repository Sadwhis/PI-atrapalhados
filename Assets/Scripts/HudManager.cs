using Atrapalhados;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class HudManager : MonoBehaviour
{
    public static HudManager instance;

    [Header("Configuração do Painel")]
    [SerializeField] private GameObject[] _botaoFechar;
    [SerializeField] private CanvasGroup _painelInstrucoes;
    [SerializeField] private CanvasGroup _painelInstrucoes2;
    [SerializeField] private RectTransform _painelTutorial; 
    [SerializeField] private RectTransform _botaoDuvidaPos;
    [SerializeField] private float _tempoVisivel = 5f;
    [SerializeField] private float _tempoParaSumir = 1f;
    FPController _moveScript;
    [Header("Pontuação na Tela")]
    [SerializeField] private TextMeshProUGUI _textoPontuacao;
    bool poAbri;
    Animator animatorPlayer;
    private bool _tutorialAberto = false;
    private Vector3 _posicaoOriginalPainel;
    PlayerInput player;
    void Awake()
    {
        instance = this;
        _moveScript = GameObject.FindWithTag("Player").GetComponent<FPController>();
        animatorPlayer = GameObject.FindWithTag("Player").GetComponentInChildren<Animator>();
        player = GameObject.FindWithTag("Player").GetComponent<PlayerInput>();
    }

    void Start()
    {
       
        Invoke("DestravarMouse", 1f);
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
            //_moveScript.enabled = false;
            player.enabled = false;
            DestravarMouse();
            EventSystem.current.SetSelectedGameObject(_botaoFechar[0]);
        }


    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            //AtivaPainelDuvida();
            FecharTutorialsecundario();
        }

        if (poAbri)
        {
            Invoke("AbrirPainelSecundario", 3f);
            poAbri = false;
        }

    }

    public void AbrirPainelSecundario()
    {
        _painelInstrucoes2.gameObject.SetActive(true);
        _painelInstrucoes2.alpha = 1f;

        //_moveScript.enabled = false; 
        player.enabled = false;
        DestravarMouse();
        animatorPlayer.Rebind();
        animatorPlayer.Update(0f);
        EventSystem.current.SetSelectedGameObject(_botaoFechar[1]);
    }
    public void FecharTutorialInicial()
    {
        if (_painelInstrucoes == null) return;

        _painelInstrucoes.DOFade(0f, 0.3f).OnComplete(() =>
        {
            _painelInstrucoes.gameObject.SetActive(false);
            TravarMouse();
        });

        Debug.Log("esta clicando");

        //_moveScript.enabled = true;
        player.enabled = true;
        poAbri = true;
    }

    public void FecharTutorialsecundario()
    {
        if (_painelInstrucoes2 == null) return;

        _painelInstrucoes2.DOFade(0f, 0.3f).OnComplete(() =>
        {
            _painelInstrucoes2.gameObject.SetActive(false);

           // _moveScript.enabled = true;
            player.enabled = true;
            TravarMouse();
        });


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
            

            
            _painelTutorial.DOMove(_botaoDuvidaPos.position, 0.4f).SetEase(Ease.InBack).SetUpdate(true);
            _painelTutorial.DOScale(0f, 0.4f).SetEase(Ease.InBack).SetUpdate(true);
            _painelTutorial.GetComponent<CanvasGroup>().DOFade(0f, 0.3f).SetUpdate(true)
                           .OnComplete(() => _painelTutorial.gameObject.SetActive(false));
        }
    }

    private void TravarMouse() { Cursor.lockState = CursorLockMode.Locked; Cursor.visible = false; }
    private void DestravarMouse() { Cursor.lockState = CursorLockMode.None; Cursor.visible = true; }
}