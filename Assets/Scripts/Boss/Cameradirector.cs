using Unity.Cinemachine;
using UnityEngine;

namespace Atrapalhados
{
    /// <summary>
    /// Controla a troca entre a câmera do player e a câmera do boss.
    /// A troca em si é feita por Priority: o Cinemachine Brain sempre
    /// dá o controle pra câmera ativa de maior prioridade, e faz o
    /// blend (transição suave) sozinho, configurado no próprio
    /// CinemachineBrain (veja o comentário no fim do arquivo).
    /// </summary>
    public class CameraDirector : MonoBehaviour
    {
        [SerializeField] private CinemachineCamera _playerCamera;
        [SerializeField] private CinemachineCamera _bossCamera;

        [SerializeField] private int _activePriority = 20;
        [SerializeField] private int _inactivePriority = 10;

        private void Start()
        {
            ShowPlayerCamera();
        }

        public void ShowBossCamera()
        {
            if (_bossCamera != null) _bossCamera.Priority = _activePriority;
            if (_playerCamera != null) _playerCamera.Priority = _inactivePriority;
        }

        public void ShowPlayerCamera()
        {
            if (_playerCamera != null) _playerCamera.Priority = _activePriority;
            if (_bossCamera != null) _bossCamera.Priority = _inactivePriority;
        }
    }
}

// Pra deixar o blend "bonito": selecione o objeto com o CinemachineBrain
// (geralmente na Main Camera) e no Inspector configure o "Default Blend":
// - Style: Ease In Out (ou Custom)
// - Time: uns 1.5 a 2 segundos
// Isso faz a troca de câmera suavizar sozinha sempre que a Priority mudar,
// sem precisar de nenhuma animação manual.