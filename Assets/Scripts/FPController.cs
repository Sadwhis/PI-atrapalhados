using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Atrapalhados
{
    [RequireComponent(typeof(CharacterController))]
    public class FPController : MonoBehaviour
    {
        [Header("Movement Parameters")]
        public float _maxSpeed => _sprintInput ? _SprintSpeed : _walkSpeed;
        [SerializeField] float _acceleration = 20f;
        public float _walkSpeed = 3.5f;
        [SerializeField] float _SprintSpeed = 8f;

        [SerializeField] float _movementSmoothTime = 0.12f;
        Vector2 _smoothMoveInput;

        [Header("Rotation Parameters (TPS)")]
        [SerializeField] float _turnSmoothTime = 0.1f;
        float _turnSmoothVelocity;

        [Space(15)]
        [SerializeField] float _jumpHeight = 2f;

        public bool Sprinting => _sprintInput && _currentSpeed > 0.1f;

        [Header("Look Parameters")]
        public Vector2 _lookSensitivity = new Vector2(0.1f, 0.1f);
        public float _pitchLimit = 85f;
        [SerializeField] float _currentPitch = 0f;
        [SerializeField] float _currentYaw = 0f;

        [Header("Controle")]
        public bool MovementLocked { get; set; }
        public bool LookLocked { get; set; }

        public float CurrentPitch
        {
            get => _currentPitch;
            set => _currentPitch = Mathf.Clamp(value, -_pitchLimit, _pitchLimit);
        }

        [Header("Configurações do Soco")]
        [SerializeField] private GameObject objetoDoSoco;
        [SerializeField] private float tempoAteOImpacto = 0.2f;
        [SerializeField] private float duracaoDoSoco = 0.1f;

        [Header("Camera Parameters")]
        [SerializeField] float _cameraNormalFOV = 60f;
        [SerializeField] float _cameraSprintFOV = 80f;
        [SerializeField] float _cameraFOVSmoothing = 1f;

        [Header("Camera TPS")]
        [Tooltip("Objeto que controla a inclinação vertical da câmera.")]
        [SerializeField] Transform _cameraRoot;

        public Transform CameraRoot => _cameraRoot;

        [Header("Physics Parameters")]
        [SerializeField] float _gravityScale = 3f;
        public float _verticalVelocity = 0f;

        public Vector3 _currentVelocity { get; private set; }
        public float _currentSpeed { get; private set; }

        public bool IsGrounded => _charactercontroller.isGrounded;

        [Header("Input")]
        public Vector2 _moveInput;
        public Vector2 _lookInput;
        public bool _sprintInput;


        [Header("Components")]
        [SerializeField] CinemachineCamera _tpsCamera;
        [SerializeField] CharacterController _charactercontroller;
        [SerializeField] Animator _animator;
        [SerializeField] FlyEnemy flyEnemy;

        public Vector3 _KnockBackForce;

        [SerializeField] private float controllerLookSensitivity = 120f;
        #region Unity Methods

        void OnValidate()
        {
            if (_charactercontroller == null)
                _charactercontroller = GetComponent<CharacterController>();
        }

        void Start()
        {
            // flyEnemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<FlyEnemy>();
        }

        void Update()
        {
            MoveUpdate();
            LookUpdate();
            CameraUpdate();

            if (_animator != null)
            {
                float pitchNormalizado = _currentPitch / _pitchLimit;
                _animator.SetFloat("MiraV", -pitchNormalizado);
            }
        }

        #endregion

        #region Controller Methods

        public void TryJump()
        {
            if (MovementLocked)
                return;

            if (!IsGrounded)
                return;

            _verticalVelocity = Mathf.Sqrt(
                _jumpHeight * -2f * Physics.gravity.y * _gravityScale
            );

            if (_animator != null)
            {
                _animator.SetTrigger("Pular");
            }
        }

        public void ToggleCameraView()
        {
            // O jogo permanece sempre em terceira pessoa.
        }

        public void ClickSoco()
        {
            if (MovementLocked)
                return;

            if (_animator != null)
            {
                _animator.SetTrigger("Socar");
            }
        }

        private void MoveUpdate()
        {
            if (MovementLocked)
            {
                _currentSpeed = 0f;
                ApplyGravity();

                _currentVelocity = new Vector3(
                    0f,
                    _verticalVelocity,
                    0f
                );

                _charactercontroller.Move(
                    _currentVelocity * Time.deltaTime
                );

                UpdateAnimations(false);
                return;
            }

            _smoothMoveInput = Vector2.Lerp(
     _smoothMoveInput,
     _moveInput,
     1f - Mathf.Exp(-1f / _movementSmoothTime * Time.deltaTime)
 );

            Vector3 direction = new Vector3(
                _smoothMoveInput.x,
                0f,
                _smoothMoveInput.y
            );

            Vector3 motion = Vector3.zero;

            if (direction.sqrMagnitude > 0.01f)
            {
                direction.Normalize();

                // Direção horizontal da câmera
                float yaw = _currentYaw * Mathf.Deg2Rad;

                Vector3 cameraForward = new Vector3(
                    Mathf.Sin(yaw),
                    0f,
                    Mathf.Cos(yaw)
                );

                Vector3 cameraRight = new Vector3(
                    Mathf.Cos(yaw),
                    0f,
                    -Mathf.Sin(yaw)
                );

                // Movimento relativo à câmera
                motion =
                    cameraForward * direction.z +
                    cameraRight * direction.x;

                motion.Normalize();

                // Rotaciona o personagem para a direção que está andando
                float targetAngle =
                    Mathf.Atan2(motion.x, motion.z) * Mathf.Rad2Deg;

                float smoothAngle = Mathf.SmoothDampAngle(
                    transform.eulerAngles.y,
                    targetAngle,
                    ref _turnSmoothVelocity,
                    _turnSmoothTime
                );

                transform.rotation = Quaternion.Euler(
                    0f,
                    smoothAngle,
                    0f
                );
            }

            // Velocidade desejada
            Vector3 targetVelocity =
                motion * _maxSpeed;

            // Aceleração/desaceleração
            Vector3 horizontalVelocity = new Vector3(
                _currentVelocity.x,
                0f,
                _currentVelocity.z
            );

            horizontalVelocity = Vector3.MoveTowards(
                horizontalVelocity,
                targetVelocity,
                _acceleration * Time.deltaTime
            );

            // Gravidade
            ApplyGravity();

            // Junta movimento horizontal + vertical
            _currentVelocity = new Vector3(
                horizontalVelocity.x,
                _verticalVelocity,
                horizontalVelocity.z
            );

            // Move o CharacterController
            _charactercontroller.Move(
                _currentVelocity * Time.deltaTime
            );

            // Velocidade usada pela animação/FOV
            _currentSpeed = new Vector3(
                _currentVelocity.x,
                0f,
                _currentVelocity.z
            ).magnitude;

            UpdateAnimations(true);
        }

        void ApplyGravity()
        {
            if (IsGrounded && _verticalVelocity <= 0.01f)
            {
                _verticalVelocity = -3f;
            }
            else
            {
                _verticalVelocity +=
                    Physics.gravity.y *
                    _gravityScale *
                    Time.deltaTime;
            }
        }

        void UpdateAnimations(bool allowMovement)
        {
            if (_animator == null)
                return;

            bool estaAndando =
                allowMovement &&
                _currentSpeed > 0.1f;

            _animator.SetBool(
                "TaAndando",
                estaAndando
            );

            _animator.SetBool(
                "NoChao",
                IsGrounded
            );

            float multiplicador =
                _currentSpeed / _walkSpeed;

            _animator.SetFloat(
                "VelocidadeAnim",
                multiplicador
            );
        }

        private void LookUpdate()
        {
            if (LookLocked)
                return;

            if (Gamepad.current == null)
                return;

            Vector2 input = Gamepad.current.rightStick.ReadValue();

            if (input.sqrMagnitude < 0.01f)
                return;

            float sensibilidadeX = 120f;
            float sensibilidadeY = 120f;

            input *= Time.deltaTime;

            _currentYaw += input.x * sensibilidadeX;
            _currentPitch -= input.y * sensibilidadeY;

            _currentPitch = Mathf.Clamp(
                _currentPitch,
                -_pitchLimit,
                _pitchLimit
            );

            _cameraRoot.localRotation = Quaternion.Euler(
                _currentPitch,
                _currentYaw,
                0f
            );
        }

        void CameraUpdate()
        {
            if (_tpsCamera == null)
                return;

            float targetFOV = _cameraNormalFOV;

            if (Sprinting)
            {
                float speedRatio =
                    Mathf.Clamp01(
                        _currentSpeed / _SprintSpeed
                    );

                targetFOV = Mathf.Lerp(
                    _cameraNormalFOV,
                    _cameraSprintFOV,
                    speedRatio
                );
            }

            _tpsCamera.Lens.FieldOfView =
                Mathf.Lerp(
                    _tpsCamera.Lens.FieldOfView,
                    targetFOV,
                    _cameraFOVSmoothing *
                    Time.deltaTime
                );
        }

        #endregion
    }
}

