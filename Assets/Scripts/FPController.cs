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

        void MoveUpdate()
        {
            // Durante diálogo/interação:
            // não permite movimento, mas mantém o Controller ativo.
            if (MovementLocked)
            {
                _currentVelocity = Vector3.MoveTowards(
                    _currentVelocity,
                    Vector3.zero,
                    _acceleration * Time.deltaTime
                );

                ApplyGravity();

                Vector3 lockedVelocity = new Vector3(
                    0f,
                    _verticalVelocity,
                    0f
                );

                _charactercontroller.Move(
                    lockedVelocity * Time.deltaTime
                );

                _currentSpeed = 0f;

                UpdateAnimations(false);

                return;
            }

            Vector3 motion = Vector3.zero;

            // Movimento relativo à orientação do personagem.
            Vector3 direction = new Vector3(
                _moveInput.x,
                0f,
                _moveInput.y
            );

            if (direction.sqrMagnitude > 0.01f)
            {
                direction.Normalize();

                motion =
                    transform.forward * direction.z +
                    transform.right * direction.x;

                motion.Normalize();
            }

            motion.y = 0f;

            // Aceleração
            if (motion.sqrMagnitude >= 0.01f)
            {
                _currentVelocity = Vector3.MoveTowards(
                    _currentVelocity,
                    motion * _maxSpeed,
                    _acceleration * Time.deltaTime
                );
            }
            else
            {
                _currentVelocity = Vector3.MoveTowards(
                    _currentVelocity,
                    Vector3.zero,
                    _acceleration * Time.deltaTime
                );
            }

            ApplyGravity();

            Vector3 fullVelocity = new Vector3(
                _currentVelocity.x,
                _verticalVelocity,
                _currentVelocity.z
            );

            _charactercontroller.Move(
                fullVelocity * Time.deltaTime
            );

            _currentSpeed = _currentVelocity.magnitude;

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

            Vector2 input = _lookInput;

            if (input.sqrMagnitude < 0.0001f)
                return;

            bool usandoControle = Gamepad.current != null &&
                                  Gamepad.current.rightStick.ReadValue().sqrMagnitude > 0.01f;

            float sensibilidadeX;
            float sensibilidadeY;

            if (usandoControle)
            {
                // CONTROLE
                sensibilidadeX = 120f;
                sensibilidadeY = 120f;

                input *= Time.deltaTime;

                _currentYaw += input.x * sensibilidadeX;
                _currentPitch -= input.y * sensibilidadeY;
            }
            else
            {
                // MOUSE
                sensibilidadeX = _lookSensitivity.x;
                sensibilidadeY = _lookSensitivity.y;

                _currentYaw += input.x * sensibilidadeX;
                _currentPitch -= input.y * sensibilidadeY;
            }

            // Limita a câmera para cima/baixo
            _currentPitch = Mathf.Clamp(
                _currentPitch,
                -_pitchLimit,
                _pitchLimit
            );

            // Agora SOMENTE a câmera gira.
            // O personagem não recebe mais transform.Rotate().
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

