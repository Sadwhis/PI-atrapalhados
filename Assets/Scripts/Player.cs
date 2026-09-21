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

        [Header("Auto Run")]
        [SerializeField] private bool autoRun = true;
        [SerializeField] private bool autoSprint = true;

        public bool click;

        #region Input Handling

        [Header("Corrida Automática")]
        [SerializeField] private float tempoParaCorrer = 3f;

        private float tempoAndando = 0f;
        void OnMove(InputValue value)
        {
            FPController._moveInput = value.Get<Vector2>();
        }

        void Update()
        {
            if (FPController._moveInput.sqrMagnitude > 0.01f)
            {
                tempoAndando += Time.deltaTime;

                if (tempoAndando >= tempoParaCorrer)
                {
                    FPController._sprintInput = true;
                }
            }
            else
            {
                tempoAndando = 0f;
                FPController._sprintInput = false;
            }
        }
        
        void OnLook(InputValue value)
        {
            //FPController._lookInput = value.Get<Vector2>();

            
        }

        void OnSprint(InputValue value)
        {
            if (autoSprint)
            {
                FPController._sprintInput = true;
            }
            else
            {                FPController._sprintInput = value.isPressed;
            }
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

        
        public void OnDialogueNext(InputValue value)
        {
            if (value.isPressed)
            {
                
                NPC npc = FindFirstObjectByType<NPC>();

                if (npc != null)
                {
                    npc.ProximoDialogo();
                }
            }
        }

        #endregion

        #region Unity Methods

        private void OnValidate()
        {
            if (FPController == null)
                FPController = GetComponent<FPController>();
        }

        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        #endregion
    }
}

