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

        #region Unity Methods
        void OnValidate()
        {
            if (_charactercontroller == null)
                _charactercontroller = GetComponent<CharacterController>();
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
        }

        public void ToggleCameraView()
        {
            _isFirstPerson = !_isFirstPerson;
        }

        void MoveUpdate()
        {
            Vector3 motion = transform.forward * _moveInput.y + transform.right * _moveInput.x;
            motion.y = 0f;
            motion.Normalize();

            if (motion.sqrMagnitude >= 0.01f)
                _currentVelocity = Vector3.MoveTowards(_currentVelocity, motion * _maxSpeed, _acceleration * Time.deltaTime);
            else
                _currentVelocity = Vector3.MoveTowards(_currentVelocity, Vector3.zero, _acceleration * Time.deltaTime);

            if (IsGrounded && _verticalVelocity <= 0.01f)
                _verticalVelocity = -3f;
            else
                _verticalVelocity += Physics.gravity.y * _gravityScale * Time.deltaTime;

            Vector3 fullVelocity = new Vector3(_currentVelocity.x, _verticalVelocity, _currentVelocity.z);
            _charactercontroller.Move(fullVelocity * Time.deltaTime);
            _currentSpeed = _currentVelocity.magnitude;
        }

        void LookUpdate()
        {
            Vector2 input = new Vector2(_lookInput.x * _lookSensitivity.x, _lookInput.y * _lookSensitivity.y);

            // Olhar Cima/Baixo
            CurrentPitch -= input.y;

            // | Alterado | Rotacionamos o CameraRoot em vez de só a _fpCamera
            // Se _cameraRoot for nulo, tenta usar a _fpCamera como fallback
            if (_cameraRoot != null)
            {
                _cameraRoot.localRotation = Quaternion.Euler(CurrentPitch, 0f, 0f);
            }
            else if (_fpCamera != null)
            {
                _fpCamera.transform.localRotation = Quaternion.Euler(CurrentPitch, 0f, 0f);
            }

            
            transform.Rotate(Vector3.up * input.x);
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

           
            // Index 0 = FPS Camera
            // Index 1 = TPS Camera
            _mixingCamera.SetWeight(0, 1f - _cameraMixVal); // Se Mix é 0 (FPS), Peso é 1. Se Mix é 1 (TPS), Peso é 0.
            _mixingCamera.SetWeight(1, _cameraMixVal);      // Oposto da FPS
        }

        #endregion
    }
}