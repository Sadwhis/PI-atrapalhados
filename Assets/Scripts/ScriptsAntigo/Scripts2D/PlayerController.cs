using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Componentes")]
    public Rigidbody2D rb;
    public Main_HudControll _mainHud;

    [Header("Movimento")]
    [SerializeField] public float velocidade;
    private Vector2 moveInput;

    [Header("Pulo")]
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;
    public bool isGrounded;

    private void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        _mainHud = GameObject.FindWithTag("Hud").GetComponent<Main_HudControll>();
    }

    private void Update()
    {
        // Movimento horizontal
        rb.linearVelocity = new Vector2(moveInput.x * velocidade, rb.linearVelocity.y);

        // Checa se está no chão
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);
    }

    // Input do Movimento
    public void SetMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    // Input do Pulo
    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("FALA(NPC)"))
        {
            _mainHud.MostrarDialogo();
           
        }
    }
    private void OnDrawGizmosSelected()
    {
        // Gizmo para visualizar o Ground Check
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
        }
    }
}
