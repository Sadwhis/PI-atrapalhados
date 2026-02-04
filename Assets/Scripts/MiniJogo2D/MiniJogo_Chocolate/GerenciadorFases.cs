using TMPro;
using UnityEngine;

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
    void Start()
    {
        _princesa = GameObject.FindWithTag("Player").GetComponent<Princesa>();
        _plataforma = GameObject.FindWithTag("Plataforma").GetComponent<MovePlataforma>();
    }

    
    void Update()
    {
        
        _tempoTotalJogo += Time.deltaTime;
        _timerFase += Time.deltaTime;

        _textoTempo.text = _tempoTotalJogo.ToString("F0") + "s";

       
        if (_timerFase >= _tempoFase && _faseAtual < 7)
        {
            Fases(_faseAtual);
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
                
                _princesa.Gravidade(1.5f, 16f); 
                break;

            case 5:
                
                _plataforma.MudarTamanho(2); 
                break;

            case 6:
               
                break;

            case 7:
                
                _princesa.Gravidade(2.5f, 20f);
                break;
        }
    }
}

