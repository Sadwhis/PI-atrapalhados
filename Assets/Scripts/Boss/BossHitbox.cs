using UnityEngine;

namespace Atrapalhados
{
    [RequireComponent(typeof(Collider))]
    public class BossHitbox : MonoBehaviour
    {
        [SerializeField] private BossAbelha _boss;

        private void Reset()
        {
            GetComponent<Collider>().isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player"))
                return;

            if (_boss != null)
                _boss.HitPlayer(other);
        }
    }
}