using UnityEngine;

public class Princesa : MonoBehaviour
{
    [Header("Configuração do Pulo")]
    [SerializeField] float _jump = 15f;
    [SerializeField] float _aberturaCurva;
    [SerializeField] float _forçaParedeX;
    [SerializeField] float _forçaParedeY;
    public float _pont;

    private Rigidbody2D _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    
    void OnCollisionEnter2D(Collision2D collision)
    {
       
        if (collision.gameObject.CompareTag("Plataforma"))
        {
            Pular();
            _pont++;
        }
        if (collision.gameObject.CompareTag("ParedeEsq"))
        {
            PularParede(1);
        }
        if (collision.gameObject.CompareTag("ParedeDir"))
        {
            PularParede(-1);
        }
    }

    void Pular()
    {
        
        _rb.linearVelocity = Vector2.zero;
        float desvioX = Random.Range(-1f, 1f);
        Vector2 vetorPulo = new Vector2(desvioX * _aberturaCurva, _jump);
        _rb.AddForce(vetorPulo, ForceMode2D.Impulse);
    }

    void PularParede(int direcao)
    {
        _rb.linearVelocity = Vector2.zero;
        Vector2 vetorParede = new Vector2(direcao * _forçaParedeX, _forçaParedeY);
        _rb.AddForce(vetorParede, ForceMode2D.Impulse);
    }
}
