using UnityEngine;

namespace Atrapalhados
{
    public class BossAbelha : MonoBehaviour
    {
        [Header("Pontos (cantos da arena)")]
        [SerializeField] private Transform _p0;
        [SerializeField] private Transform _p1;
        [SerializeField] private Transform _p2;
        [SerializeField] private Transform _p3;

        [Header("Movimento")]
        [SerializeField] private float _speed = 15f;
        [SerializeField] private float _tempoEmCadaLado = 20f;
        [SerializeField] private float _height = 10f;
        [SerializeField] private float _rotationSpeed = 8f;
        [SerializeField] private float _arriveThreshold = 0.15f;

        [Header("Orientação")]
        [Tooltip("Opcional. Se não definir, o centro é calculado automaticamente como a média dos 4 pontos.")]
        [SerializeField] private Transform _arenaCenter;

        private Vector3 _centerPosition;

        private enum State { Patrolling, ReturningToAnchor, MovingToNextAnchor }

        // Cada índice define um "lado": um ponto-âncora e o ponto parceiro
        // com quem ele oscila. A ordem aqui já reproduz exatamente a
        // sequência do seu diagrama, sempre andando pela borda:
        // P2<->P3, depois P1<->P0, depois P0<->P3, depois P3<->P2, e repete.
        private Transform[] _anchors;
        private Transform[] _partners;
        private int _sideIndex;

        private State _state;
        private bool _movingToPartner;
        private float _timer;

        private void Start()
        {
            _anchors = new[] { _p2, _p1, _p0, _p3 };
            _partners = new[] { _p3, _p0, _p3, _p2 };

            _sideIndex = 0;
            _state = State.Patrolling;
            _movingToPartner = true;
            _timer = 0f;

            _centerPosition = _arenaCenter != null
                ? GetPosition(_arenaCenter)
                : new Vector3(
                    (_p0.position.x + _p1.position.x + _p2.position.x + _p3.position.x) / 4f,
                    _height,
                    (_p0.position.z + _p1.position.z + _p2.position.z + _p3.position.z) / 4f);

            transform.position = GetPosition(_anchors[_sideIndex]);
        }

        private void Update()
        {
            switch (_state)
            {
                case State.Patrolling:
                    UpdatePatrolling();
                    break;
                case State.ReturningToAnchor:
                    UpdateReturningToAnchor();
                    break;
                case State.MovingToNextAnchor:
                    UpdateMovingToNextAnchor();
                    break;
            }

            FaceCenter();
        }

        private void UpdatePatrolling()
        {
            _timer += Time.deltaTime;

            if (_timer >= _tempoEmCadaLado)
            {
                _state = State.ReturningToAnchor;
                return;
            }

            Transform anchor = _anchors[_sideIndex];
            Transform partner = _partners[_sideIndex];
            Transform target = _movingToPartner ? partner : anchor;

            MoveTowards(target);

            if (HasArrived(target))
            {
                transform.position = GetPosition(target);
                _movingToPartner = !_movingToPartner;
            }
        }

        private void UpdateReturningToAnchor()
        {
            Transform anchor = _anchors[_sideIndex];

            MoveTowards(anchor);

            if (HasArrived(anchor))
            {
                transform.position = GetPosition(anchor);

                _sideIndex = (_sideIndex + 1) % _anchors.Length;
                _state = State.MovingToNextAnchor;
            }
        }

        private void UpdateMovingToNextAnchor()
        {
            Transform nextAnchor = _anchors[_sideIndex];

            MoveTowards(nextAnchor);

            if (HasArrived(nextAnchor))
            {
                transform.position = GetPosition(nextAnchor);

                _timer = 0f;
                _movingToPartner = true;
                _state = State.Patrolling;
            }
        }

        private bool HasArrived(Transform target)
        {
            return Vector3.Distance(transform.position, GetPosition(target)) <= _arriveThreshold;
        }

        private void MoveTowards(Transform target)
        {
            Vector3 targetPosition = GetPosition(target);
            Vector3 direction = targetPosition - transform.position;

            if (direction.sqrMagnitude < 0.0001f)
                return;

            direction.Normalize();

            transform.position += direction * _speed * Time.deltaTime;
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
    }
}