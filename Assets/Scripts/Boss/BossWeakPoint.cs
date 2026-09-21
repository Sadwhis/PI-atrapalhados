using UnityEngine;

namespace Atrapalhados
{
    [RequireComponent(typeof(Collider))]
    public class BossWeakPoint : MonoBehaviour
    {
        [SerializeField] private BossAbelha _boss;
        [Tooltip("Altura do pulo automático do player ao pisar aqui (mesma unidade do _jumpHeight do FPController).")]
        [SerializeField] private float _bounceHeight = 3f;

        private void Reset()
        {
            GetComponent<Collider>().isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player"))
                return;

            FPController player = other.GetComponent<FPController>();
            if (player != null)
                player.Bounce(_bounceHeight);

            if (_boss != null)
                _boss.TakeDamage();
        }
    }
}