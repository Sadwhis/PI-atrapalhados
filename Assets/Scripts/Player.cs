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
            Debug.Log($"Sprint Input mudou para: {FPController._sprintInput}");
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
                FPController.ClickSoco();
                AtaqueKnock();
            }
        }

        public void AtaqueKnock()
        {
            RaycastHit hit;
            float attackRange = 2.5f; 

            
            Vector3 origem = transform.position;
            Vector3 direcao = transform.forward;

            
            Debug.DrawRay(origem, direcao * attackRange, Color.red, 2f);

           
            if (Physics.Raycast(origem, direcao, out hit, attackRange))
            {
                IHitable hitable = hit.transform.GetComponent<IHitable>();
                if (hitable != null)
                {
                    hitable.Execute(transform);

                    
                    Debug.DrawRay(origem, direcao * hit.distance, Color.green, 2f);
                }
            }
        }
    }
    #endregion
}