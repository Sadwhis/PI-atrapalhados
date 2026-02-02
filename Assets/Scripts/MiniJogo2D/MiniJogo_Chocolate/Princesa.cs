using UnityEngine;

public class Princesa : MonoBehaviour
{
    [Header("Configuração do Pulo")]
    [SerializeField] float _jump = 15f; 

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
        }
    }

    void Pular()
    {
        
        _rb.linearVelocity = Vector2.zero;

        
        _rb.AddForce(Vector2.up * _jump, ForceMode2D.Impulse);
    }
}