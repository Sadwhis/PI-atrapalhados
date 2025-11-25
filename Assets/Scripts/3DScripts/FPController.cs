using UnityEngine;
using Unity.Cinemachine;
using UnityEditor.ShaderGraph.Internal;
using UnityEditor.Rendering.LookDev;


namespace Matinta
{
    [RequireComponent(typeof(CharacterController))]
    public class FPController : MonoBehaviour
    {
        [Header("Movement Parameters")]
        [SerializeField] float _maxSpeed => _sprintInput ? _SprintSpeed : _walkSpeed;
        [SerializeField] float _acceleration = 20f; // Acceleration rate

        [SerializeField] float _walkSpeed = 3.5f;
        [SerializeField] float _SprintSpeed = 8f;


        [Space(15)]
        [Tooltip("É assim que o personagem pode pular alto")]
        [SerializeField] float _jumpHeight = 2f; // Jump height

        public bool Sprinting
        {

            get
            {
                return _sprintInput && _currentSpeed > 0.1f;
            }
        }

        [Header("Look Parameters")]
        public Vector2 _lookSensitivity = new Vector2(0.1f, 0.1f);

        public float _pitchLimit = 85f; // Limit for pitch rotation

        [SerializeField] float _currentPitch = 0f; // Current pitch rotation

        public float CurrentPitch
        {
            get => _currentPitch;

            set
            {
                _currentPitch = Mathf.Clamp(value, -_pitchLimit, _pitchLimit);
            }
        }

        [Header("Camera Parameters")]
        [SerializeField] float _cameraNormalFOV = 60f; // Normal field of view
        [SerializeField] float _cameraSprintFOV = 80f; // Field of view when sprinting
        [SerializeField] float _cameraFOVSmoothing = 1f; // Smoothing factor for FOV transition

        float TargetCameraFOV
        {
            get
            {
                return Sprinting ? _cameraSprintFOV : _cameraNormalFOV;
            }
        }

        [Header("Physics Parameters")]
        [SerializeField] float _gravityScale = 3f; // Gravity value

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
            {
                _charactercontroller = GetComponent<CharacterController>();
            }
        }



        void Update()
        {
            MoveUpdate();
            LookUpdate();
            CameraUpdate();
        }

        #endregion




        #region Controller Methods

        public void TryJump()
        {
            if (IsGrounded == false)
            {
                return;
            }

            _verticalVelocity = Mathf.Sqrt(_jumpHeight * -2f * Physics.gravity.y * _gravityScale);
        }

        void MoveUpdate()
        {
            Vector3 motion = transform.forward * _moveInput.y + transform.right * _moveInput.x;
            motion.y = 0f;
            motion.Normalize();

            if (motion.sqrMagnitude >= 0.01f)
            {
                _currentVelocity = Vector3.MoveTowards(_currentVelocity, motion * _maxSpeed, _acceleration * Time.deltaTime);
            }
            else
            {
                _currentVelocity = Vector3.MoveTowards(_currentVelocity, Vector3.zero, _acceleration * Time.deltaTime);
            }


            if (IsGrounded && _verticalVelocity <= 0.01f)
            {
                _verticalVelocity = -3f;
            }
            else
            {
                _verticalVelocity += Physics.gravity.y * _gravityScale * Time.deltaTime;
            }

            Vector3 fullVelocity = new Vector3(_currentVelocity.x, _verticalVelocity, _currentVelocity.z);


            _charactercontroller.Move(fullVelocity * Time.deltaTime);

            _currentSpeed = _currentVelocity.magnitude;
        }

        void LookUpdate()
        {
            Vector2 input = new Vector2(_lookInput.x * _lookSensitivity.x, _lookInput.y * _lookSensitivity.y);

            //looking up and down
            CurrentPitch -= input.y;

            _fpCamera.transform.localRotation = Quaternion.Euler(CurrentPitch, 0f, 0f);

            //looking left and right
            transform.Rotate(Vector3.up * input.x);
        }

        void CameraUpdate()
        {
            float targetFOV = _cameraNormalFOV;

            if (Sprinting)
            {
                float speedRatio = _currentSpeed / _SprintSpeed;

                targetFOV = Mathf.Lerp(_cameraNormalFOV, _cameraSprintFOV, speedRatio);
            }

            _fpCamera.Lens.FieldOfView = Mathf.Lerp(_fpCamera.Lens.FieldOfView, targetFOV, _cameraFOVSmoothing * Time.deltaTime);
        }

        #endregion

















    }

}
