using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
public class FlyEnemy : MonoBehaviour
{
    [Header("---- ALVO ----")]
    public Transform targetToDefend;

    [Tooltip("Player")]
    public Transform player;

   
    [SerializeField] float _flightHeight = 3.0f;
    [SerializeField] float _flightLow = 3.0f;
    [SerializeField] float _moveSpeed = 3.5f;
    private float _alturaAlvo;
    public float velocidadeSuavizacao = 5.0f;
    [SerializeField] float _detectionRadius = 10f; 

    [SerializeField] LayerMask _obstaclesLayer;

    [Header("---- PATRULHA ----")]
    [SerializeField] float _patrolRadius = 5f; 
    [SerializeField] float _patrolWaitTime = 2f;

 
    private NavMeshAgent _agent;
    private float _waitTimer;
    private bool _isPerseguindo = false;

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();

        _agent.baseOffset = _flightHeight; 
        _agent.speed = _moveSpeed;
        _agent.updateRotation = false;
       
    }

    void Update()
    {
        
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