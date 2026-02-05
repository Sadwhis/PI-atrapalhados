using Unity.VisualScripting;
using UnityEngine;

public class Princesa : MonoBehaviour
{
    [Header("Configuração do Pulo")]
    [SerializeField] float _jump = 15f;
    [SerializeField] float _aberturaCurva;
    [SerializeField] float _forçaParedeX;
    [SerializeField] float _forçaParedeY;
    public float _pont;
    public bool _pauseGame = true;
    public bool _jumpAnim;
    private Rigidbody2D _rb;
    private Animator _anim;
     
    Vector2 _posSalva;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponentInChildren<Animator>();
        
    }
    void Start()
    {
       
        SetRigidBody2D(true);
    }

    private void Update()
    {
        Anim();
    }

    public void SetRigidBody2D(bool Value) 
    {
        

        if (Value)
        {
            _posSalva = _rb.linearVelocity;
            _rb.bodyType = RigidbodyType2D.Kinematic;
            _rb.linearVelocity = Vector2.zero;
            _anim.enabled = !Value;
            _pauseGame = true;

        }
        else
        {
            _rb.bodyType = RigidbodyType2D.Dynamic;
            _rb.linearVelocity = _posSalva;
            _anim.enabled = !Value;
            _pauseGame = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
       
        if (collision.gameObject.CompareTag("Plataforma"))
        {
            _jumpAnim = true;
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

    private void OnCollisionExit2D(Collision2D collision)
    {
       _jumpAnim = false;
    }

    void Pular()
    {
        
        _rb.linearVelocity = Vector2.zero;
        float desvioX = Random.Range(-1f, 1f);
        Vector2 vetorPulo = new Vector2(desvioX * _aberturaCurva, _jump);
        _rb.AddForce(vetorPulo, ForceMode2D.Impulse);
    }

    public void Gravidade(float gravidade, float novaForcaPulo)
    {
        _rb.gravityScale = gravidade;
        _jump = novaForcaPulo;
    }

    void Anim() 
    {
        _anim.SetBool("IsGround", _jumpAnim);
    }

    void PularParede(int direcao)
    {
        _rb.linearVelocity = Vector2.zero;
        Vector2 vetorParede = new Vector2(direcao * _forçaParedeX, _forçaParedeY);
        _rb.AddForce(vetorParede, ForceMode2D.Impulse);
    }

 
}
