using UnityEngine;

namespace Atrapalhados
{
    public class BossAbelha : MonoBehaviour
    {
        private enum State { Idle, Patrolling, Charging }

        [Header("Pontos do percurso (na ordem do ciclo A -> B -> C -> D -> A)")]
        [SerializeField] private Transform _pointA;
        [SerializeField] private Transform _pointB;
        [SerializeField] private Transform _pointC;
        [SerializeField] private Transform _pointD;

        [Header("Ativação")]
        [Tooltip("Onde o boss fica parado no topo da montanha até o player entrar no trigger. Se vazio, usa o Ponto A.")]
        [SerializeField] private Transform _homePoint;
        [Tooltip("Se não arrastar aqui, tenta achar automaticamente um objeto com a tag Player.")]
        [SerializeField] private Transform _player;

        [Header("Movimento")]
        [SerializeField] private float _speed = 15f;
        [SerializeField] private float _tempoPorLado = 5f;
        [SerializeField] private float _height = 10f;
        [SerializeField] private float _arriveThreshold = 0.15f;
        [SerializeField] private float _rotationSpeed = 8f;

        [Header("Voo (balanço vertical)")]
        [Tooltip("Quanto ela sobe/desce enquanto voa.")]
        [SerializeField] private float _bobHeight = 1f;
        [Tooltip("Velocidade do balanço de subir/descer.")]
        [SerializeField] private float _bobSpeed = 2f;

        [Header("Orientação")]
        [Tooltip("Opcional. Se vazio, o centro é calculado automaticamente como a média dos 4 pontos.")]
        [SerializeField] private Transform _arenaCenter;

        [Header("Ataque de investida")]
        [SerializeField] private float _chargeCooldown = 10f;
        [SerializeField] private float _chargeSpeed = 30f;
        [SerializeField] private float _chargeArriveThreshold = 0.5f;
        [Tooltip("Por quanto tempo ela persegue o player antes de desviar pro ponto final, mesmo se não alcançar.")]
        [SerializeField] private float _chargeChaseTime = 1.5f;
        [Tooltip("Distância do player considerada 'alcançou', que já faz ela desviar pro ponto final.")]
        [SerializeField] private float _chargePlayerArriveThreshold = 2f;

        [Header("Hit no player")]
        [Tooltip("Força horizontal aplicada no player quando o hitbox do boss encosta nele.")]
        [SerializeField] private float _hitKnockbackForce = 15f;
        [Tooltip("Força vertical (pra cima), pra dar um arco no knockback.")]
        [SerializeField] private float _hitKnockbackUpward = 4f;
        [Tooltip("Tempo mínimo entre dois hits, pra não aplicar força toda hora enquanto os colliders se sobrepõem.")]
        [SerializeField] private float _hitCooldown = 1f;

        private float _hitCooldownTimer;

        [Header("Vida")]
        [SerializeField] private int _maxLives = 3;
        [SerializeField] private Color _damageFlashColor = Color.red;
        [SerializeField] private float _damageFlashDuration = 0.2f;
        [Tooltip("Tempo mínimo entre dois hits no ponto fraco (topo), pra não descontar várias vidas de uma vez.")]
        [SerializeField] private float _weakPointHitCooldown = 0.5f;
        [SerializeField] private UnityEngine.Events.UnityEvent _onDefeated;

        private int _currentLives;
        private bool _isDefeated;
        private float _weakPointCooldownTimer;
        private Renderer[] _renderers;
        private Color[] _originalColors;
        private Coroutine _flashCoroutine;

        private Transform[] _points;
        private int _pointIndex;
        private int _chargeLandingIndex;
        private bool _chargePhaseLanding;
        private float _chargePhaseTimer;
        private float _sideTimer;
        private float _chargeTimer;
        private Vector3 _centerPosition;
        private Vector3 _chargeTarget;
        private State _state;

        private void Start()
        {
            _points = new[] { _pointA, _pointB, _pointC, _pointD };

            _centerPosition = _arenaCenter != null
                ? GetPosition(_arenaCenter)
                : new Vector3(
                    (_pointA.position.x + _pointB.position.x + _pointC.position.x + _pointD.position.x) / 4f,
                    _height,
                    (_pointA.position.z + _pointB.position.z + _pointC.position.z + _pointD.position.z) / 4f);

            if (_player == null)
            {
                GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
                if (playerObj != null)
                    _player = playerObj.transform;
            }

            Transform start = _homePoint != null ? _homePoint : _pointA;
            transform.position = GetPosition(start);

            _pointIndex = 0;
            _sideTimer = 0f;
            _chargeTimer = 0f;
            _state = State.Idle;

            _currentLives = _maxLives;
            _isDefeated = false;

            _renderers = GetComponentsInChildren<Renderer>();
            _originalColors = new Color[_renderers.Length];
            for (int i = 0; i < _renderers.Length; i++)
            {
                _originalColors[i] = _renderers[i].material.color;
            }
        }

        private void Update()
        {
            if (_isDefeated)
                return;

            if (_hitCooldownTimer > 0f)
                _hitCooldownTimer -= Time.deltaTime;

            if (_weakPointCooldownTimer > 0f)
                _weakPointCooldownTimer -= Time.deltaTime;

            switch (_state)
            {
                case State.Idle:
                    UpdateIdle();
                    break;
                case State.Patrolling:
                    UpdatePatrolling();
                    break;
                case State.Charging:
                    UpdateCharging();
                    return; // durante a investida ela olha pra direção do dash, não pro centro
            }

            FaceCenter();
        }

        private void LateUpdate()
        {
            if (_isDefeated)
                return;

            // Balanço vertical de voo, por cima de qualquer movimento/estado.
            // Não mexe em X/Z, só sobrepõe um sobe-e-desce no Y.
            float bob = Mathf.Sin(Time.time * _bobSpeed) * _bobHeight;
            transform.position = new Vector3(transform.position.x, _height + bob, transform.position.z);
        }

        private void UpdateIdle()
        {
            // Parada esperando o trigger externo chamar Activate().
        }

        /// <summary>
        /// Chamado pelo BossActivationTrigger quando o player entra na zona de ativação.
        /// </summary>
        public void Activate()
        {
            if (_state != State.Idle)
                return;

            _pointIndex = 1 % _points.Length; // primeiro alvo do ciclo é o Ponto B
            _sideTimer = 0f;
            _chargeTimer = 0f;
            _state = State.Patrolling;
        }

        /// <summary>
        /// Chamado pelo BossResetTrigger quando o player cai da montanha.
        /// Ela volta pro ponto fixo e fica esperando (o balanço de voo do
        /// LateUpdate continua rodando, então ela não fica totalmente
        /// parada — só não anda mais até ser ativada de novo).
        /// </summary>
        public void ResetToHome()
        {
            _state = State.Idle;
            _pointIndex = 0;
            _sideTimer = 0f;
            _chargeTimer = 0f;
            _chargePhaseLanding = false;
            _chargePhaseTimer = 0f;

            Transform start = _homePoint != null ? _homePoint : _pointA;
            transform.position = GetPosition(start);
        }

        private void UpdatePatrolling()
        {
            _chargeTimer += Time.deltaTime;
            _sideTimer += Time.deltaTime;

            if (_player != null && _chargeTimer >= _chargeCooldown)
            {
                StartCharge();
                return;
            }

            Transform target = _points[_pointIndex];
            Vector3 targetPosition = GetPosition(target);

            if (!HasArrived(targetPosition))
            {
                MoveTowards(targetPosition, _speed);
            }
            else
            {
                transform.position = targetPosition; // chegou antes do tempo: espera o resto parado
            }

            if (_sideTimer >= _tempoPorLado)
            {
                _pointIndex = (_pointIndex + 1) % _points.Length;
                _sideTimer = 0f;
            }
        }

        private void StartCharge()
        {
            // Sempre pousa no ponto OPOSTO do lado em que ela está agora
            // no ciclo: A <-> C e B <-> D, sempre no mesmo par fixo.
            _chargeLandingIndex = (_pointIndex + 2) % _points.Length;
            _chargeTarget = GetPosition(_points[_chargeLandingIndex]);

            _chargePhaseLanding = false;
            _chargePhaseTimer = 0f;
            _chargeTimer = 0f;
            _state = State.Charging;
        }

        private void UpdateCharging()
        {
            if (!_chargePhaseLanding)
            {
                _chargePhaseTimer += Time.deltaTime;

                Vector3 chaseTarget = _player != null ? GetPosition(_player) : _chargeTarget;
                MoveAndFace(chaseTarget);

                bool reachedPlayer = _player != null
                    && Distance2D(transform.position, _player.position) <= _chargePlayerArriveThreshold;
                bool chaseTimedOut = _chargePhaseTimer >= _chargeChaseTime;

                if (_player == null || reachedPlayer || chaseTimedOut)
                {
                    _chargePhaseLanding = true;
                }
            }
            else
            {
                MoveAndFace(_chargeTarget);

                if (Distance2D(transform.position, _chargeTarget) <= _chargeArriveThreshold)
                {
                    EndCharge();
                }
            }
        }

        private void MoveAndFace(Vector3 targetPosition)
        {
            Vector3 direction = targetPosition - transform.position;
            direction.y = 0f;

            if (direction.sqrMagnitude <= 0.0001f)
                return;

            Vector3 dirNormalized = direction.normalized;

            transform.position += dirNormalized * _chargeSpeed * Time.deltaTime;

            Quaternion targetRotation = Quaternion.LookRotation(dirNormalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
        }

        private void EndCharge()
        {
            transform.position = _chargeTarget;

            // Ela pousou exatamente no ponto _chargeLandingIndex, então o
            // ciclo continua normalmente a partir do próximo ponto depois dele.
            _pointIndex = (_chargeLandingIndex + 1) % _points.Length;
            _sideTimer = 0f;
            _chargeTimer = 0f;
            _state = State.Patrolling;
        }

        /// <summary>
        /// Chamado pelo BossHitbox quando o collider dele toca o player.
        /// Empurra o player pra longe do boss, com um cooldown pra não
        /// aplicar força repetidamente enquanto os colliders se sobrepõem.
        /// </summary>
        public void HitPlayer(Collider playerCollider)
        {
            if (_hitCooldownTimer > 0f)
                return;

            _hitCooldownTimer = _hitCooldown;

            Vector3 direction = Flat(playerCollider.transform.position - transform.position);

            if (direction.sqrMagnitude < 0.0001f)
                direction = Flat(transform.forward);

            direction.Normalize();

            Vector3 force = direction * _hitKnockbackForce + Vector3.up * _hitKnockbackUpward;

            FPController playerController = playerCollider.GetComponent<FPController>();
            if (playerController != null)
                playerController.ApplyKnockback(force);
        }

        /// <summary>
        /// Chamado pelo BossWeakPoint (o hitbox de cima) quando o player
        /// pisa nela. Desconta uma vida, pisca vermelho, e derrota ela se
        /// acabarem as vidas.
        /// </summary>
        public void TakeDamage()
        {
            if (_isDefeated)
                return;

            if (_weakPointCooldownTimer > 0f)
                return;

            _weakPointCooldownTimer = _weakPointHitCooldown;
            _currentLives--;

            if (_flashCoroutine != null)
                StopCoroutine(_flashCoroutine);
            _flashCoroutine = StartCoroutine(FlashDamage());

            if (_currentLives <= 0)
            {
                Defeat();
            }
        }

        private System.Collections.IEnumerator FlashDamage()
        {
            for (int i = 0; i < _renderers.Length; i++)
            {
                if (_renderers[i] != null)
                    _renderers[i].material.color = _damageFlashColor;
            }

            yield return new WaitForSeconds(_damageFlashDuration);

            for (int i = 0; i < _renderers.Length; i++)
            {
                if (_renderers[i] != null)
                    _renderers[i].material.color = _originalColors[i];
            }
        }

        private void Defeat()
        {
            _isDefeated = true;
            _state = State.Idle;
            _onDefeated?.Invoke();
        }

        private bool HasArrived(Vector3 targetPosition)
        {
            return Distance2D(transform.position, targetPosition) <= _arriveThreshold;
        }

        private void MoveTowards(Vector3 targetPosition, float speed)
        {
            Vector3 direction = targetPosition - transform.position;
            direction.y = 0f; // o balanço vertical é só visual, não deve afetar a direção do voo

            if (direction.sqrMagnitude < 0.0001f)
                return;

            direction.Normalize();

            transform.position += direction * speed * Time.deltaTime;
        }

        private void FaceCenter()
        {
            Vector3 direction = _centerPosition - transform.position;
            direction.y = 0f;

            if (direction.sqrMagnitude < 0.0001f)
                return;

            direction.Normalize();

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
        }

        private Vector3 GetPosition(Transform point)
        {
            return new Vector3(point.position.x, _height, point.position.z);
        }

        private Vector3 Flat(Vector3 v)
        {
            return new Vector3(v.x, 0f, v.z);
        }

        private float Distance2D(Vector3 a, Vector3 b)
        {
            return Vector3.Distance(Flat(a), Flat(b));
        }
    }
}