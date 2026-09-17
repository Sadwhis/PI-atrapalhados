using UnityEngine;

namespace Atrapalhados
{
    public class BossSideTrigger : MonoBehaviour
    {
        [Header("Próximo lado")]
        [SerializeField] private Transform _nextPointA;
        [SerializeField] private Transform _nextPointB;

        private void OnTriggerEnter(Collider other)
        {
        
        }
    }
}