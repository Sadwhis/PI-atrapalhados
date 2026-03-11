using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEditor.Purchasing;
using UnityEngine;
using UnityEngine.UI;


public class GerenciadorFases : MonoBehaviour
{
    [Header("Referências")]
    private MovePlataforma _plataforma;
    private GeradorInimigos _geradorInimigos;
    private Princesa _princesa;
    [SerializeField] TextMeshProUGUI _textoTempo;

    [SerializeField] float _tempoFase;

    private float _tempoTotalJogo;
    private int _faseAtual = 1;
    private float _timerFase;
    private float velocidadeFases;

    [SerializeField] TextMeshProUGUI _textoAnuncioFase;
    [SerializeField] RawImage RawImage;

  

    bool _isPaused;
    void Start()
    {
        
        _princesa = GameObject.FindWithTag("Player").GetComponent<Princesa>();
        _plataforma = GameObject.FindWithTag("Plataforma").GetComponent<MovePlataforma>();
        velocidadeFases = _plataforma._velocidadeNormal;
        _textoTempo.text = _tempoTotalJogo.ToString("F0") + "s";
       // //StartCoroutine(SequenciaMudancaFase(_faseAtual));
        //_faseAtual++;
    }

    public void IniciarJogo()
    {
        _faseAtual = 1;
        StartCoroutine(SequenciaMudancaFase(_faseAtual));
        _faseAtual++; // Já prepara a próxima fase para o Update não repetir a 1
        _timerFase = 0f; // Zera o cronômetro
    }
    void Update()
    {
        if (!_princesa._pauseGame)
        {
            _tempoTotalJogo += Time.deltaTime;
            _timerFase += Time.deltaTime;
            _textoTempo.text = _tempoTotalJogo.ToString("F0") + "s";
        }



        if (_timerFase >= _tempoFase && _faseAtual <= 7)
        {
            // APAGUE A LINHA "Fases(_faseAtual);" QUE FICAVA AQUI!

            StartCoroutine(SequenciaMudancaFase(_faseAtual)); // A corrotina já chama o Fases() no final dela
            _faseAtual++;

            _timerFase = 0f;
        }
    }

    void Fases(int nivel)
    {
        Debug.Log("FASE: " + nivel);

        switch (nivel)
        {
            case 1:
                
                _plataforma.MudarTamanho(0); 
                _princesa.Gravidade(0.8f, 12f); 
                break;

            case 2:
                
                _plataforma.MudarTamanho(1); 
                break;

            case 3:
                
                
                break;

            case 4:
                _plataforma._velocidadeNormal = 25f;
                velocidadeFases = 25f;
                _princesa.Gravidade(1.5f, 16f); 
                break;

            case 5:
                
                _plataforma.MudarTamanho(2); 
                break;

            case 6:
               
                break;

            case 7:
                
                _princesa.Gravidade(2.5f, 20f);
                _plataforma._velocidadeNormal = 30f;
                velocidadeFases = 30f;
                break;
        }
    }

    IEnumerator SequenciaMudancaFase(int nivel)
    {
       
        _princesa.SetRigidBody2D(true);

        if (_textoAnuncioFase != null)
        {
            _plataforma._velocidadeNormal = 0f;
            CanvasGroup cg = _textoAnuncioFase.GetComponent<CanvasGroup>();
            cg.alpha = 0;
            _textoAnuncioFase.transform.localScale = Vector3.one * 0.5f;
            _textoAnuncioFase.text = "FASE " + nivel;
            _textoAnuncioFase.gameObject.SetActive(true);
            cg.DOFade(1, 0.5f); 
            _textoAnuncioFase.transform.DOScale(1.2f, 0.5f).SetEase(Ease.OutBack);
            yield return new WaitForSeconds(1.5f);
            cg.DOFade(0, 0.5f);
            yield return new WaitForSeconds(0.5f); 
        }

        _plataforma._velocidadeNormal = velocidadeFases;
        Fases(nivel);
        _princesa.SetRigidBody2D(false);
    }


}

