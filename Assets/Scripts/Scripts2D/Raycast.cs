using UnityEngine;

public class Raycast : MonoBehaviour
{
    [Header("Origem e direção")]
    public Transform originTransform;            // Se nulo, usa a posição deste GameObject
    public Vector2 originOffset = Vector2.zero;  // deslocamento local aplicado à origem
    public Vector2 direction = Vector2.right;    // direção do ray (local se useLocalDirection == true)

    [Header("Config")]
    public bool useLocalDirection = true;        // se true, direction é relativo à rotação do GameObject
    public float distance = 5f;                  // comprimento do ray
    public LayerMask layerMask = ~0;             // quais layers colidem (padrão: tudo)

    [Header("Hits")]
    public bool detectAll = false;               // se true usa RaycastAll (todas as colisões)
    public ContactFilter2D contactFilter;        // mais controle (opcional)

    [Header("Debug")]
    public bool drawDebug = true;
    public Color debugHitColor = Color.red;
    public Color debugMissColor = Color.green;
    public float debugDuration = 0.02f;

    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public Transform groundCheck;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool isGrounded;


    void Reset()
    {
        // Inicializa contact filter para usar o layerMask se o usuário não tocar nisso
        contactFilter = new ContactFilter2D();
        contactFilter.SetLayerMask(layerMask);
        contactFilter.useLayerMask = true;
    }

    void OnValidate()
    {
        // mantém o contactFilter alinhado ao layerMask quando editado no inspector
        contactFilter.SetLayerMask(layerMask);
        contactFilter.useLayerMask = true;
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Movimento horizontal
        float moveInput = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        // Verifica se está no chão
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);

        // Pular
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
        float move = Input.GetAxisRaw("Horizontal");
        transform.Translate(Vector2.right * move * moveSpeed * Time.deltaTime);
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        Vector2 moveDir = new Vector2(moveX, moveY).normalized;
        rb.linearVelocity = moveDir * moveSpeed;
        Vector2 origin = transform.position;

        Vector2 rayorigin = (originTransform != null) ? (Vector2)originTransform.position + originOffset : (Vector2)transform.position + originOffset;
        Vector2 dir = direction.normalized;
        if (useLocalDirection)
            dir = (Vector2)(transform.TransformDirection(direction)).normalized;

        if (!detectAll)
        {
            // Raycast simples: pega primeiro collider encontrado
            RaycastHit2D hit = Physics2D.Raycast(origin, dir, distance, layerMask);
            if (hit.collider != null)
            {
                Debug.DrawLine(rayorigin, rayorigin + dir * hit.distance, Color.red);
                Debug.DrawRay(hit.point, hit.normal * 0.5f, Color.yellow);
                HandleHit(hit);
            }
            else
            {
                HandleMiss(origin, dir);
            }

            if (drawDebug)
            {
                Vector3 endPoint = origin + dir * distance;
                Debug.DrawLine(origin, endPoint, hitColorForLastCheck(hit: Physics2D.Raycast(origin, dir, distance, layerMask)), debugDuration);
                if (Physics2D.Raycast(origin, dir, distance, layerMask))
                {
                    // marcar o ponto de impacto
                    RaycastHit2D h = Physics2D.Raycast(origin, dir, distance, layerMask);
                    Debug.DrawRay(h.point, Vector3.zero, debugHitColor, debugDuration);
                }
            }
        }
        else
        {
            // RaycastAll: pega todas as colisões ao longo do raio
            RaycastHit2D[] hits = Physics2D.RaycastAll(origin, dir, distance, layerMask);
            if (hits.Length > 0)
            {
                for (int i = 0; i < hits.Length; i++)
                    HandleHit(hits[i]);
            }
            else
            {
                HandleMiss(origin, dir);
            }

            if (drawDebug)
            {
                Color col = (Physics2D.Raycast(origin, dir, distance, layerMask)) ? debugHitColor : debugMissColor;
                Debug.DrawLine(origin, origin + dir * distance, col, debugDuration);
            }
        }
    }
    // Tratamento padrão quando há hit
    void HandleHit(RaycastHit2D hit)
    {
        if (hit.collider.CompareTag("Player"))
        {
            // lógica para jogador
        }
        else if (hit.collider.CompareTag("Wall"))
        {
            // lógica para parede
        }
        Debug.Log($"Raycast hit: {hit.collider.name} at {hit.point} (distance {hit.distance})", hit.collider);
    }

    // Tratamento quando não houve colisão
    void HandleMiss(Vector2 origin, Vector2 dir)
    {
        // Exemplo: apenas log (remova em builds)
        // Debug.Log("Raycast miss");
    }

    // Função auxiliar para escolher cor rápida
    Color hitColorForLastCheck(RaycastHit2D hit)
    {
        return (hit.collider != null) ? debugHitColor : debugMissColor;
    }

    // Opcional: desenha gizmos no editor
    void OnDrawGizmosSelected()
    {
        if (!drawDebug) return;

        Vector2 origin = (originTransform != null) ? (Vector2)originTransform.position + originOffset : (Vector2)transform.position + originOffset;
        Vector2 dir = useLocalDirection ? (Vector2)(transform.TransformDirection(direction)).normalized : direction.normalized;
        Vector3 end = origin + dir * distance;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(origin, end);
        Gizmos.DrawSphere(end, 0.05f);
    }
}
