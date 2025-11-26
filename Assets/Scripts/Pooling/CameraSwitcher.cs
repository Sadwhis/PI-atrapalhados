using Atrapalhados;
using Unity.Cinemachine;
using UnityEngine;

namespace Atrapalhados
{
    public class CameraSwitcher : MonoBehaviour
    {
        [Header("Cameras")]
        public CinemachineCamera fpsCam;
        public CinemachineFreeLook tpsCam;

        [Header("References")]
        public FPController fpController;

        bool isFPS = true;

        void Start()
        {
            // Começa em FPS
            fpsCam.Priority = 20;
            tpsCam.Priority = 10;

            fpController.UseFPSCamera = true;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.V))
            {
                isFPS = !isFPS;

                if (isFPS)
                {
                    // FPS
                    fpsCam.Priority = 20;
                    tpsCam.Priority = 10;
                    fpController.UseFPSCamera = true;
                }
                else
                {
                    // TPS
                    fpsCam.Priority = 10;
                    tpsCam.Priority = 20;
                    fpController.UseFPSCamera = false;
                }
            }
        }
    }
}
