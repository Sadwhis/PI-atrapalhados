using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MOVE_OUT : MonoBehaviour
{
    [Header("Componentes")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GAMECONTROL_OUT gameController;

    [Header("Movimento")]
    [SerializeField] private float velocidade = 5f;
    private Vector2 moveInput;

    [Header("Pulo")]
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;
    private bool isGrounded;

    [Header("Diálogo")]
    [SerializeField] public bool botaoDialogo;
    [SerializeField] public bool botaoDialogoExit;

    private void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (gameController == null) gameController = GameObject.FindWithTag("GAMEOUT").GetComponent<GAMECONTROL_OUT>();
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
            gameController._Falando.SetActive(true);
        }
    }

    /*
    // botões de diálogo no futuro
    public void ButtonDialogue()
    {
        if (botaoDialogo)
        {
            SceneManager.LoadScene("Sapo-Cururu");
        }
    }

    public void ButtonExit()
    {
        if (botaoDialogoExit)
        {
            gameController._Falando.SetActive(false);
        }
    }
    */

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
