using UnityEngine;

namespace Atrapalhados
{
    [RequireComponent(typeof(Collider))]
    public class BossActivationTrigger : MonoBehaviour
    {
        [SerializeField] private BossAbelha _boss;

        private void Reset()
        {
            // Garante que o Collider deste objeto já nasça como trigger.
            GetComponent<Collider>().isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player"))
                return;

            if (_boss != null)
                _boss.Activate();
        }
    }
}
