using UnityEngine;

namespace Atrapalhados
{
    public class BossAbelha : MonoBehaviour
    {
        [Header("Movement Points")]
        [SerializeField] private Transform[] _points;

        [Header("Movement")]
        [SerializeField] private float _speed = 4f;
        [SerializeField] private float _waitTime = 2f;

        [Header("Change Side")]
        [SerializeField] private float _changeSideTime = 10f;

        private int _pointA;
        private int _pointB;

        private int _targetPoint;
        private float _waitTimer;
        private float _sideTimer;

        private bool _isWaiting;

        private void Start()
        {
            if (_points == null || _points.Length < 4)
            {
                Debug.LogWarning("Configure 4 pontos de movimento.");
                enabled = false;
                return;
            }

            _pointA = 0;
            _pointB = 1;

            transform.position = _points[_pointA].position;

            _targetPoint = _pointB;
        }

        private void Update()
        {
            _sideTimer += Time.deltaTime;

            if (_sideTimer >= _changeSideTime)
            {
                ChangeSide();
                _sideTimer = 0f;
            }

            if (_isWaiting)
            {
                _waitTimer += Time.deltaTime;

                if (_waitTimer >= _waitTime)
                {
                    _isWaiting = false;
                    _waitTimer = 0f;

                    ChangeTarget();
                }

                return;
            }

            MoveBoss();
        }

        private void MoveBoss()
        {
            Transform target = _points[_targetPoint];

            Vector3 direction = target.position - transform.position;

            direction.y = 0f;

            if (direction.sqrMagnitude > 0.01f)
            {
                direction.Normalize();

                transform.position += direction * _speed * Time.deltaTime;

                Quaternion targetRotation = Quaternion.LookRotation(direction);

                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    targetRotation,
                    8f * Time.deltaTime
                );
            }

            if (Vector3.Distance(transform.position, target.position) <= 0.15f)
            {
                transform.position = new Vector3(
                    target.position.x,
                    transform.position.y,
                    target.position.z
                );

                _isWaiting = true;
                _waitTimer = 0f;
            }
        }

        private void ChangeTarget()
        {
            if (_targetPoint == _pointA)
                _targetPoint = _pointB;
            else
                _targetPoint = _pointA;
        }

        private void ChangeSide()
        {
            // Troca para o lado oposto do quadrado.
            _pointA = (_pointA + 2) % 4;
            _pointB = (_pointB + 2) % 4;

            _targetPoint = _pointA;

            _isWaiting = true;
            _waitTimer = 0f;
        }
    }
}
