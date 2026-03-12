using UnityEngine;

public class LimpoTeleporte : MonoBehaviour
{
    [Header("Configurações de Teleporte")]
    [SerializeField] private Transform _pontoTeleporte;

    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("Player"))
        {
           
            CharacterController cc = other.GetComponent<CharacterController>();

            if (cc != null)
            {
                
                cc.enabled = false;
                other.transform.position = _pontoTeleporte.position;
                cc.enabled = true;
            }
          

            Debug.Log("Teleporte 3D funcionou!");
        }
    }
}