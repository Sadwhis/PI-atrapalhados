using UnityEngine;

public class InimigoVoado : MonoBehaviour
{
    [Header("Patrol")]
    public float speed = 2f;
    public Transform pointA;
    public Transform pointB;

    [Header("Detection / Grab")]
    public float detectionRadius = 5f;
    public float grabDistance = 0.5f;
    public LayerMask playerLayer;

    [Header("Carry / Lift")]
    public float liftSpeed = 2f;
    public Vector3 carryOffset = new Vector3(0f, -0.5f, 0f);

    [Header("Release / Drop")]
    public float dropForce = 5f;
    public float cooldownTime = 5f; // tempo de espera antes de poder agarrar de novo

    // internals
    private Vector3 patrolTarget;
    private Transform player;
    private Rigidbody2D playerRb;
    private Transform originalParent;
    private float originalGravity;
    private bool carryingPlayer = false;
    private bool onCooldown = false;

    void Start()
    {
        patrolTarget = (pointA != null) ? pointA.position : transform.position;
    }

    void Update()
    {
        if (carryingPlayer)
        {
            // Carregando o player → vai até o próximo ponto de patrulha
            transform.position = Vector2.MoveTowards(transform.position, patrolTarget, liftSpeed * Time.deltaTime);

            // Se chegou no ponto de patrulha → solta o player e entra em cooldown
            if (Vector2.Distance(transform.position, patrolTarget) < 0.1f)
            {
                ReleasePlayer();
                StartCoroutine(StartCooldown());
            }
        }
        else
        {
            if (onCooldown)
            {
                // Se está em cooldown → apenas patrulha normalmente
                Patrol();
            }
            else
            {
                // Procura player
                Collider2D playerCheck = Physics2D.OverlapCircle(transform.position, detectionRadius, playerLayer);

                if (playerCheck != null)
                {
                    Transform pT = playerCheck.transform;
                    transform.position = Vector2.MoveTowards(transform.position, pT.position, speed * Time.deltaTime);

                    if (Vector2.Distance(transform.position, pT.position) < grabDistance)
                    {
                        GrabPlayer(pT);
                    }
                }
                else
                {
                    Patrol();
                }
            }
        }
    }

    void Patrol()
    {
        transform.position = Vector2.MoveTowards(transform.position, patrolTarget, speed * Time.deltaTime);

        if (pointA != null && pointB != null && Vector2.Distance(transform.position, patrolTarget) < 0.1f)
        {
            patrolTarget = (patrolTarget == pointA.position) ? pointB.position : pointA.position;
            transform.eulerAngles = (patrolTarget == pointA.position) ? Vector3.zero : new Vector3(0, 180, 0);
        }
    }

    [System.Obsolete]
    void GrabPlayer(Transform p)
    {
        player = p;
        playerRb = player.GetComponent<Rigidbody2D>();
        originalParent = player.parent;

        if (playerRb != null)
        {
            originalGravity = playerRb.gravityScale;
            playerRb.linearVelocity = Vector2.zero;
            playerRb.gravityScale = 0f;
            playerRb.isKinematic = true;
        }

        player.SetParent(transform);
        player.localPosition = carryOffset;

        carryingPlayer = true;
    }

    [System.Obsolete]
    void ReleasePlayer()
    {
        if (player == null) return;

        player.SetParent(originalParent);

        if (playerRb != null)
        {
            playerRb.isKinematic = false;
            playerRb.gravityScale = originalGravity;
            if (dropForce != 0f)
            {
                playerRb.AddForce(Vector2.down * dropForce, ForceMode2D.Impulse);
            }
        }

        player = null;
        carryingPlayer = false;
    }

    System.Collections.IEnumerator StartCooldown()
    {
        onCooldown = true;
        yield return new WaitForSeconds(cooldownTime); // espera segundos ou minutos
        onCooldown = false;
    }

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

