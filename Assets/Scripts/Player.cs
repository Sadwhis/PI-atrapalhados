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
        public bool click;

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
            //Debug.Log($"Sprint Input mudou para: {FPController._sprintInput}");
        }

       public void OnJump(InputValue value)
        {
            if (value.isPressed)
            {
                FPController.TryJump();
            }
        }

        public void OnSwitchCamera(InputValue value)
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
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

        }


        void OnAttack(InputValue value)
        {
            if (value.isPressed)
            {
                click = true;
                FPController.ClickSoco(); 
            }
            else
            {
                click = false;
            }
        }


    }
    #endregion
}