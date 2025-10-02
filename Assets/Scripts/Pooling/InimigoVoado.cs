using UnityEngine;

public class InimigoVoado : MonoBehaviour
{
    public float speed = 2f;                  // Velocidade normal
    public Transform pointA;                  // Ponto inicial
    public Transform pointB;                  // Ponto final
    public float detectionRadius = 5f;        // Raio de detecção do player
    public float liftHeight = 3f;             // Quanto o inimigo levanta o player
    public float liftSpeed = 2f;              // Velocidade ao levantar o player
    public LayerMask playerLayer;             // Layer do player

    private Vector3 target;                   // Alvo atual
    private Transform player;                 // Referência ao player
    private bool carryingPlayer = false;      // Se o inimigo está carregando o player

    void Start()
    {
        target = pointA.position; // começa indo para A
    }

    void Update()
    {
        if (carryingPlayer)
        {
            // Levanta o player junto
            Vector3 liftTarget = new Vector3(transform.position.x, transform.position.y + liftHeight, transform.position.z);
            transform.position = Vector2.MoveTowards(transform.position, liftTarget, liftSpeed * Time.deltaTime);

            // Move o player junto com o inimigo
            if (player != null)
            {
                player.position = transform.position + new Vector3(0, -0.5f, 0);
            }
        }
        else
        {
            // Procura o player dentro do raio de visão
            Collider2D playerCheck = Physics2D.OverlapCircle(transform.position, detectionRadius, playerLayer);

            if (playerCheck != null)
            {
                // Player detectado → vai até ele
                player = playerCheck.transform;
                transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

                // Se chegou perto o suficiente, agarra
                if (Vector2.Distance(transform.position, player.position) < 0.5f)
                {
                    carryingPlayer = true;
                }
            }
            else
            {
                // Patrulha normal entre os pontos
                transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);

                if (Vector2.Distance(transform.position, target) < 0.1f)
                {
                    if (target == pointA.position)
                    {
                        target = pointB.position;
                        transform.eulerAngles = new Vector3(0, 180, 0);
                    }
                    else
                    {
                        target = pointA.position;
                        transform.eulerAngles = new Vector3(0, 0, 0);
                    }
                }
            }
        }
    }

    // Gizmo para visualizar área de detecção
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        if (pointA != null && pointB != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(pointA.position, pointB.position);
        }
    }
}
