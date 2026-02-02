using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using TMPro;

public class MovePlataforma : MonoBehaviour
{
    
    [SerializeField] float _velocidadeNormal = 10f;
    [SerializeField] float _limiteX = 8f;

    [Header("Dificuldade (Tamanhos)")]
    [SerializeField] Vector3[] _tamanhos = { new Vector3(2f, 1f, 1f), new Vector3(1.5f, 1f, 1f), new Vector3(1f, 1f, 1f) };

    private Vector2 _move;
    private float _velocidadeAtual;
    

    Princesa _princ;

    [SerializeField] TextMeshProUGUI _pontuaçãoUI;
    void Start()
    {
        MudarTamanho(0);
        _princ = GameObject.FindWithTag("Player").GetComponent<Princesa>();
    }

    void Update()
    {
        Move();
        AtualizacaoUI();
    }

    void Move()
    {
        Vector3 movimento = new Vector3(_move.x, 0, 0) * _velocidadeNormal * Time.deltaTime;
        transform.Translate(movimento, Space.World);

        
        float xTravado = Mathf.Clamp(transform.position.x, -_limiteX, _limiteX);
        transform.position = new Vector3(xTravado, transform.position.y, transform.position.z);
    }

    void AtualizacaoUI() 
    {
        _pontuaçãoUI.text = $"Score: {_princ._pont:F0}";
    }
    void OnPlataforma(InputValue value)
    {
        _move = value.Get<Vector2>();
    }

    
    public void MudarTamanho(int indice)
    {
        if (indice >= 0 && indice < _tamanhos.Length)
        {
            transform.localScale = _tamanhos[indice];
        }
    }

   
   
}