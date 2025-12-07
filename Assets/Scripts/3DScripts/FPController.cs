using UnityEngine;
using Unity.Cinemachine;

namespace Atrapalhados
{
    [RequireComponent(typeof(CharacterController))]
    public class FPController : MonoBehaviour
    {
        [Header("Movement Parameters")]
        [SerializeField] float _maxSpeed => _sprintInput ? _SprintSpeed : _walkSpeed;
        [SerializeField] float _acceleration = 20f;
        [SerializeField] float _walkSpeed = 3.5f;
        [SerializeField] float _SprintSpeed = 8f;

        // --- NOVO: Suavização da rotação para 3ª pessoa ---
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

        public float CurrentPitch
        {
            get => _currentPitch;
            set => _currentPitch = Mathf.Clamp(value, -_pitchLimit, _pitchLimit);
        }

        [Header("Camera Parameters")]
        [SerializeField] float _cameraNormalFOV = 60f;
        [SerializeField] float _cameraSprintFOV = 80f;
        [SerializeField] float _cameraFOVSmoothing = 1f;

        //Mixing Camera |
        [Header("Camera Switching")]
        [SerializeField] CinemachineMixingCamera _mixingCamera;
        [Tooltip("Arraste o objeto que segura as câmeras aqui (CameraRoot) para que ambas girem verticalmente")]
        [SerializeField] Transform _cameraRoot;
        [SerializeField] float _switchSpeed = 5f;
        private bool _isFirstPerson = true;
        private float _cameraMixVal = 0f; // 0 = FPS, 1 = TPS

        float TargetCameraFOV => Sprinting ? _cameraSprintFOV : _cameraNormalFOV;

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
        [SerializeField] CinemachineCamera _fpCamera;
        [SerializeField] CharacterController _charactercontroller;
        [SerializeField] Animator _animator;

        // Referência para a câmera principal para calcular direção na 3ª pessoa
        private Transform _mainCamTransform;

        #region Unity Methods
        void OnValidate()
        {
            if (_charactercontroller == null)
                _charactercontroller = GetComponent<CharacterController>();
        }

        void Start()
        {
            // Pega a referência da câmera principal (usada na lógica de 3ª pessoa)
            if (Camera.main != null)
                _mainCamTransform = Camera.main.transform;
        }

        void Update()
        {
            MoveUpdate();
            LookUpdate();
            CameraUpdate(); // Lida com FOV
            CameraMixingUpdate(); // Lida com a troca de câmera
        }
        #endregion

        #region Controller Methods

        public void TryJump()
        {
            if (!IsGrounded) return;

            _verticalVelocity = Mathf.Sqrt(_jumpHeight * -2f * Physics.gravity.y * _gravityScale);

            if (_animator != null)
            {
                _animator.SetTrigger("Pular");
            }
        }

        public void ToggleCameraView()
        {
            _isFirstPerson = !_isFirstPerson;
        }

        void MoveUpdate()
        {
            Vector3 motion = Vector3.zero;

           
            if (_isFirstPerson)
            {
               
                
                motion = transform.forward * _moveInput.y + transform.right * _moveInput.x;
            }
            else
            {
               
                Vector3 direction = new Vector3(_moveInput.x, 0f, _moveInput.y).normalized;

                if (direction.magnitude >= 0.1f && _mainCamTransform != null)
                {
                    
                    float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + _mainCamTransform.eulerAngles.y;

                  
                    float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, _turnSmoothTime);

                  
                    transform.rotation = Quaternion.Euler(0f, angle, 0f);

                    
                    Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                    motion = moveDir;
                }
            }

            // Normaliza e Aplica Velocidade
            motion.y = 0f;
            motion.Normalize();

            if (motion.sqrMagnitude >= 0.01f)
                _currentVelocity = Vector3.MoveTowards(_currentVelocity, motion * _maxSpeed, _acceleration * Time.deltaTime);
            else
                _currentVelocity = Vector3.MoveTowards(_currentVelocity, Vector3.zero, _acceleration * Time.deltaTime);

            // Gravidade
            if (IsGrounded && _verticalVelocity <= 0.01f)
                _verticalVelocity = -3f;
            else
                _verticalVelocity += Physics.gravity.y * _gravityScale * Time.deltaTime;

            Vector3 fullVelocity = new Vector3(_currentVelocity.x, _verticalVelocity, _currentVelocity.z);
            _charactercontroller.Move(fullVelocity * Time.deltaTime);
            _currentSpeed = _currentVelocity.magnitude;

            // Animação
            if (_animator != null)
            {
                bool estaAndando = _currentSpeed > 0.1f;
                _animator.SetBool("TaAndando", estaAndando);
                _animator.SetBool("NoChao", IsGrounded);
            }
        }

        void LookUpdate()
        {
            Vector2 input = new Vector2(_lookInput.x * _lookSensitivity.x, _lookInput.y * _lookSensitivity.y);

            // Pitch (Olhar para cima e para baixo) - Acontece nos dois modos para mover a câmera
            CurrentPitch -= input.y;

            if (_cameraRoot != null)
            {
                _cameraRoot.localRotation = Quaternion.Euler(CurrentPitch, 0f, 0f);
            }
            else if (_fpCamera != null)
            {
                _fpCamera.transform.localRotation = Quaternion.Euler(CurrentPitch, 0f, 0f);
            }

            // Yaw (Girar o corpo esquerda/direita) - SÓ NA 1ª PESSOA
            if (_isFirstPerson)
            {
                transform.Rotate(Vector3.up * input.x);
            }
            // Na 3ª pessoa, a rotação do corpo é controlada pelo MoveUpdate
        }

        void CameraUpdate()
        {
            if (_fpCamera == null) return;

            float targetFOV = _cameraNormalFOV;
            if (Sprinting)
            {
                float speedRatio = _currentSpeed / _SprintSpeed;
                targetFOV = Mathf.Lerp(_cameraNormalFOV, _cameraSprintFOV, speedRatio);
            }
            _fpCamera.Lens.FieldOfView = Mathf.Lerp(_fpCamera.Lens.FieldOfView, targetFOV, _cameraFOVSmoothing * Time.deltaTime);
        }

        void CameraMixingUpdate()
        {
            if (_mixingCamera == null) return;

            float targetMix = _isFirstPerson ? 0f : 1f;

            _cameraMixVal = Mathf.MoveTowards(_cameraMixVal, targetMix, _switchSpeed * Time.deltaTime);

            // Index 0 = FPS Camera, Index 1 = TPS Camera
            _mixingCamera.SetWeight(0, 1f - _cameraMixVal);
            _mixingCamera.SetWeight(1, _cameraMixVal);

            // REMOVIDO: A linha que forçava a rotação para identity (0,0,0) foi apagada daqui.
            // A rotação correta agora é tratada no MoveUpdate.
        }

        #endregion
    }
}