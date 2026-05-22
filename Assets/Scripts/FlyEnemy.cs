using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
public class FlyEnemy : MonoBehaviour, IHitable
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

    [SerializeField] LayerMask _obstaclesLayer;

    [Header("---- PATRULHA ----")]
    [SerializeField] float _patrolRadius = 5f; 
    [SerializeField] float _patrolWaitTime = 2f;

 
    private NavMeshAgent _agent;
    private float _waitTimer;
    private bool _isPerseguindo = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        _agent = GetComponent<NavMeshAgent>();

        _agent.baseOffset = _flightHeight; 
        _agent.speed = _moveSpeed;
        _agent.updateRotation = false;

       
    }

    void Update()
    {
        if (!_agent.isActiveAndEnabled) return;

        if (CheckPlayer())
        {
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

    public void Execute(Transform executionSource)
    {
        // Inicia a coroutine para gerenciar o tempo do empurrão
        StartCoroutine(RotinaKnockback(executionSource));
    }

    private IEnumerator RotinaKnockback(Transform executionSource)
    {
        // 1. Desliga o agente para a física agir
        _agent.enabled = false;
        rb.isKinematic = false;

        // 2. Calcula e aplica a força do empurrão
        Vector3 dir = (transform.position - executionSource.position).normalized;
        dir.y = 0; // Evita empurrar para o chão ou para o céu

        rb.AddForce(dir * forcaDoEmpurrao, ForceMode.Impulse);

        // 3. Espera o tempo do empurrão
        yield return new WaitForSeconds(tempoDoEmpurrao);

        // 4. Trava a física novamente
        rb.linearVelocity = Vector3.zero;
        rb.isKinematic = true;

        // 5. 🔴 A MÁGICA ACONTECE AQUI 🔴
        _agent.enabled = true; // Religa o agente primeiro

        // Força a memória interna do agente a sincronizar com a posição atual do corpo
        _agent.Warp(transform.position);

        // Se ele estiver em uma área válida, limpa a rota antiga para recalcular do zero
        if (_agent.isOnNavMesh)
        {
            _agent.ResetPath();
        }
    }

    private void KnockBackEntity(Transform executionSource) 
    {
        Vector3 dir = (transform.position - executionSource.transform.position).normalized;
        rb.AddForce(dir * forcaDoEmpurrao, ForceMode.Impulse);
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
}