using UnityEngine;

namespace Atrapalhados
{
    [RequireComponent(typeof(Collider))]
    public class BossResetTrigger : MonoBehaviour
    {
        [SerializeField] private BossAbelha _boss;
        [SerializeField] private CameraDirector _cameraDirector;

        private void Reset()
        {
            GetComponent<Collider>().isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player"))
                return;

            if (_boss != null)
                _boss.ResetToHome();

            if (_cameraDirector != null)
                _cameraDirector.ShowPlayerCamera();
        }
    }
}