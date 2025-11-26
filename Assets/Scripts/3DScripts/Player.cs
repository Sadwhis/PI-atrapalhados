using Atrapalhados;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Atrapalhados
{
    [RequireComponent(typeof(FPController))]
    public class Player : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] FPController FPController;

        #region Input Handling

        void OnMove(InputValue value)
        {
            FPController._moveInput = value.Get<Vector2>();
        }

        void OnLook(InputValue value)
        {
            FPController._lookInput = value.Get<Vector2>();
        }

        void OnSprint(InputValue value)
        {
            FPController._sprintInput = value.isPressed;
        }

        void OnJump(InputValue value)
        {
            if (value.isPressed)
            {
                FPController.TryJump();
            }
        }

        void OnSwitchCamera(InputValue value)
        {
            if (value.isPressed)
            {
                FPController.ToggleCameraView();
            }
        }

        #endregion

        #region Unity Methods

        private void OnValidate()
        {
            if (FPController == null) FPController = GetComponent<FPController>();
        }

        void Start()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
    #endregion
}