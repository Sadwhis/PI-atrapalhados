using Atrapalhados;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
public class FlyEnemy : MonoBehaviour
{
    [Header("---- ALVO ----")]
    public Transform targetToDefend;

    [Tooltip("Player")]
    public Transform player;


    Rigidbody rb;
    [SerializeField] float _flightHeight = 3.0f;
    [SerializeField] float _flightLow = 3.0f;
    [SerializeField] float _moveSpeed = 3.5f;
    private float _alturaAlvo;
    public float velocidadeSuavizacao = 5.0f;
    [SerializeField] float _detectionRadius = 10f;
    public float forcaDoEmpurrao = 10.0f;
    public float tempoDoEmpurrao = 0.2f;
    [Range(0.001f, 0.1f)][SerializeField] private float StillThreshold = 0.05f;
    [SerializeField] LayerMask _obstaclesLayer;
    [SerializeField] private float MaxKnockbackTime = 0.5f;

    [Header("---- PATRULHA ----")]
    [SerializeField] float _patrolRadius = 5f;
    [SerializeField] float _patrolWaitTime = 2f;

    [Header("---- VIDA ----")]
    [SerializeField] private int _maxLives = 3;
    [SerializeField] private Color _damageFlashColor = Color.red;
    [SerializeField] private float _damageFlashDuration = 0.2f;
    [Tooltip("Tempo mínimo entre dois hits no ponto fraco, pra não descontar várias vidas de uma vez.")]
    [SerializeField] private float _weakPointHitCooldown = 0.5f;
    [SerializeField] private UnityEngine.Events.UnityEvent _onDefeated;

    [Header("---- MORTE / QUEDA ----")]
    [Tooltip("Tempo (em segundos) que a queda até o chão/posição final leva.")]
    [SerializeField] private float _deathFallDuration = 1.5f;
    [Tooltip("Rotação final (eixo Z, em graus) para onde o inimigo tomba ao morrer.")]
    [SerializeField] private float _deathTargetZ = -65f;

    private int _currentLives;
    private bool _isDefeated;
    private float _weakPointCooldownTimer;
    private Renderer[] _renderers;
    private Color[] _originalColors;
    private Coroutine _flashCoroutine;

    [Header("---- HIT NO PLAYER (corpo) ----")]
    [Tooltip("Layer do player, usada pro corpo do inimigo detectar o toque.")]
    [SerializeField] private LayerMask _playerLayer;
    [Tooltip("Força horizontal aplicada no player quando o corpo dela encosta nele.")]
    [SerializeField] private float _hitKnockbackForce = 12f;
    [Tooltip("Força vertical (pra cima), pra dar um arco no knockback.")]
    [SerializeField] private float _hitKnockbackUpward = 3f;
    [SerializeField] private float _hitCooldown = 1f;

    private float _hitCooldownTimer;

    [Header("---- INVESTIDA ----")]
    [SerializeField] private float _chargeCooldown = 8f;
    [SerializeField] private float _chargeSpeed = 14f;
    [SerializeField] private float _chargeDuration = 1.2f;
    [SerializeField] private float _chargeArriveDistance = 1.5f;
    [Tooltip("Distância mínima do player pra sequer TENTAR investir. " +
             "Se o inimigo já estiver perto (perseguindo), a investida é pulada " +
             "pra não acabar em 0 frames (bug clássico: a coroutine já nasce com " +
             "a distância <= _chargeArriveDistance e quebra na primeira checagem).")]
    [SerializeField] private float _chargeMinDistance = 4f;

    private float _chargeTimer;
    private bool _isCharging;

    private NavMeshAgent _agent;
    private float _waitTimer;
    private bool _isPerseguindo = false;

    private Coroutine MoveCoroutine;

    Animator anim;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        _agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        _agent.baseOffset = _flightHeight;
        _agent.speed = _moveSpeed;
        _agent.updateRotation = false;

        _currentLives = _maxLives;
        _isDefeated = false;

        _renderers = GetComponentsInChildren<Renderer>();
        _originalColors = new Color[_renderers.Length];
        for (int i = 0; i < _renderers.Length; i++)
        {
            _originalColors[i] = _renderers[i].material.color;
        }
    }

    void Update()
    {
        if (_hitCooldownTimer > 0f)
            _hitCooldownTimer -= Time.deltaTime;

        if (_weakPointCooldownTimer > 0f)
            _weakPointCooldownTimer -= Time.deltaTime;

        if (_isDefeated)
            return;

        if (_isCharging)
            return; // a coroutine ChargeAttack já está cuidando do movimento

        if (!_agent.isActiveAndEnabled) return;

        _chargeTimer += Time.deltaTime;

        if (CheckPlayer())
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            bool distanciaOkParaInvestida = distanceToPlayer >= _chargeMinDistance;

            if (_chargeTimer >= _chargeCooldown && distanciaOkParaInvestida)
            {
                StartCoroutine(ChargeAttack());
                return;
            }

            AttackPlayer();
            VoandoBaixo();
        }
        else
        {
            Patrulhando();
            VoandoAlto();
        }

        Voando();

        RotaçãoMelhorada();

    }

    private IEnumerator ChargeAttack()
    {
        _isCharging = true;

        // Desliga o NavMeshAgent temporariamente (mesmo truque já usado
        // no ApplyKnockback) pra poder mover na mão em linha reta rápida.
        _agent.enabled = false;

        float elapsed = 0f;

        while (elapsed < _chargeDuration)
        {
            if (player == null)
                break;

            Vector3 direction = player.position - transform.position;
            direction.y = 0f;

            float distance = direction.magnitude;

            if (distance <= _chargeArriveDistance)
                break;

            direction.Normalize();

            transform.position += direction * _chargeSpeed * Time.deltaTime;

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 8f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Religa o NavMeshAgent e sincroniza ele com a posição atual
        // (senão o agent acha que ainda está no lugar de antes da investida).
        _agent.Warp(transform.position);
        _agent.enabled = true;

        _chargeTimer = 0f;
        _isCharging = false;
    }

    public void GetKnockedBack(Vector3 force)
    {

        StartCoroutine(ApplyKnockback(force));
    }

    private IEnumerator ApplyKnockback(Vector3 force)
    {
        yield return null;
        _agent.enabled = false;
        //rb.useGravity = true;
        rb.isKinematic = false;
        rb.AddForce(force, ForceMode.Impulse);


        yield return new WaitForSeconds(.5f);
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        // rb.useGravity = false;
        rb.isKinematic = true;
        _agent.Warp(transform.position);
        _agent.enabled = true;

        yield return null;
    }

    /// <summary>
    /// Chamado pelo BossHitbox (corpo dela) quando encosta no player.
    /// Empurra o player pra longe, com um cooldown.
    /// </summary>
    public void HitPlayer(Collider playerCollider)
    {
        if (_isDefeated)
            return;

        if (_hitCooldownTimer > 0f)
            return;

        _hitCooldownTimer = _hitCooldown;

        Vector3 direction = playerCollider.transform.position - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.0001f)
            direction = new Vector3(transform.forward.x, 0f, transform.forward.z);

        direction.Normalize();

        Vector3 force = direction * _hitKnockbackForce + Vector3.up * _hitKnockbackUpward;

        FPController playerController = playerCollider.GetComponent<FPController>();
        if (playerController != null)
            playerController.ApplyKnockback(force);
    }

    /// <summary>
    /// Chamado pelo BossWeakPoint (ponto fraco em cima) quando o player
    /// pisa nela. Desconta uma vida, pisca vermelho, derrota se acabar.
    /// </summary>
    public void TakeDamage()
    {
        if (_isDefeated)
            return;

        if (_weakPointCooldownTimer > 0f)
            return;

        _weakPointCooldownTimer = _weakPointHitCooldown;
        _currentLives--;

        //if (_flashCoroutine != null)
        //    StopCoroutine(_flashCoroutine);
        //_flashCoroutine = StartCoroutine(FlashDamage());

        if (_currentLives <= 0)
        {
            Defeat();
        }
    }

    //private IEnumerator FlashDamage()
    //{
    //    for (int i = 0; i < _renderers.Length; i++)
    //    {
    //        if (_renderers[i] != null)
    //            _renderers[i].material.color = _damageFlashColor;
    //    }

    //    yield return new WaitForSeconds(_damageFlashDuration);

    //    for (int i = 0; i < _renderers.Length; i++)
    //    {
    //        if (_renderers[i] != null)
    //            _renderers[i].material.color = _originalColors[i];
    //    }
    //}

    private void Defeat()
    {
        _isDefeated = true;

        // Para qualquer investida/knockback em andamento
        StopAllCoroutines();

        anim.enabled = false;
        if (_agent != null)
        {
            _agent.isStopped = true;
        }

        // Desativa física (ela não deve mais ser empurrada nem cair por gravidade)
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        // Desativa todos os colliders (não recebe nem causa mais hits)
        Collider[] colliders = GetComponentsInChildren<Collider>();
        foreach (var col in colliders)
        {
            if (col != null)
                col.enabled = false;
        }

        _onDefeated?.Invoke();

        StartCoroutine(DeathFallSequence());
    }

    /// <summary>
    /// Sequência de morte: baixa a altura de voo até 0 e move o inimigo
    /// até a posição Z de destino (_deathTargetZ), depois desliga o script.
    /// </summary>
    private IEnumerator DeathFallSequence()
    {
        float elapsed = 0f;

        float startHeight = _agent != null ? _agent.baseOffset : _flightHeight;
        Vector3 startPos = transform.position;
        Vector3 targetPos = startPos;
        targetPos.y -= startHeight; // desce até a "altura 0" (chão)

        Quaternion startRotation = transform.rotation;
        Vector3 startEuler = startRotation.eulerAngles;
        Quaternion targetRotation = Quaternion.Euler(startEuler.x, startEuler.y, _deathTargetZ);

        // Agent desligado só agora, depois de já termos lido o baseOffset atual
        if (_agent != null)
            _agent.enabled = false;

        while (elapsed < _deathFallDuration)
        {
            float t = elapsed / _deathFallDuration;

            _flightHeight = Mathf.Lerp(startHeight, 0f, t);
            _alturaAlvo = _flightHeight;

            transform.position = Vector3.Lerp(startPos, targetPos, t);
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);

            elapsed += Time.deltaTime;
            yield return null;
        }

        _flightHeight = 0f;
        _alturaAlvo = 0f;
        transform.position = targetPos;
        transform.rotation = targetRotation;

        // Desativa o script inteiro (Update para de rodar, nenhuma
        // funcionalidade continua ativa)
        enabled = false;
    }


    bool CheckPlayer()
    {

        Vector3 directionToPlayer = player.position - transform.position;

        float distSquared = directionToPlayer.sqrMagnitude;
        float radiusSquared = _detectionRadius * _detectionRadius;

        if (distSquared > radiusSquared)
        {
            return false;
        }

        RaycastHit hit;

        if (Physics.Raycast(transform.position, directionToPlayer.normalized, out hit, _detectionRadius, _obstaclesLayer))
        {
            if (hit.transform == player)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        return true;
    }

    void AttackPlayer()
    {
        _isPerseguindo = true;
        _agent.isStopped = false;
        _agent.SetDestination(player.position);
        _waitTimer = 0f;
    }

    void RotaçãoMelhorada()
    {

        if (_agent.velocity.sqrMagnitude > 0.1f)
        {

            Vector3 direction = _agent.velocity.normalized;


            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));


            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }

    void Patrulhando()
    {

        if (_isPerseguindo)
        {
            _isPerseguindo = false;

        }



        if (!_agent.pathPending && _agent.remainingDistance < 0.5f)
        {
            _waitTimer += Time.deltaTime;

            if (_waitTimer >= _patrolWaitTime)
            {
                NovaPatrulha();
                _waitTimer = 0f;
            }
        }
    }

    void NovaPatrulha()
    {

        Vector2 randomCircle = Random.insideUnitCircle * _patrolRadius;
        Vector3 randomPoint = targetToDefend.position + new Vector3(randomCircle.x, 0, randomCircle.y);


        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 2.0f, NavMesh.AllAreas))
        {
            _agent.SetDestination(hit.position);
        }
    }

    void VoandoAlto()
    {
        _alturaAlvo = _flightHeight;

    }

    void VoandoBaixo()
    {
        _alturaAlvo = _flightLow;

    }

    void Voando()
    {
        float efeitoFlutuacao = Mathf.Sin(Time.time * 2.0f) * 0.5f;
        float alturaDesejada = _alturaAlvo + efeitoFlutuacao;
        _agent.baseOffset = Mathf.Lerp(_agent.baseOffset, alturaDesejada, Time.deltaTime * velocidadeSuavizacao);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _detectionRadius);

        if (targetToDefend != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(targetToDefend.position, _patrolRadius);
        }


        if (player != null)
        {
            bool isVisible = CheckPlayer();
            Gizmos.color = isVisible ? Color.green : Color.yellow;
            Gizmos.DrawLine(transform.position, player.position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Corrigido: antes isso chamava ApplyKnockback (que empurra o PRÓPRIO
        // inimigo, usando o Rigidbody dele) em vez de empurrar o player.
        // Agora usa o HitPlayer(), que calcula direção, respeita o cooldown
        // e aplica a força no FPController do player.
        if (((1 << other.gameObject.layer) & _playerLayer) == 0)
            return;

        HitPlayer(other);
    }
}