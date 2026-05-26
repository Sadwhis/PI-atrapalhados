using UnityEngine;

public class ColliderSoco : MonoBehaviour
{

    public bool acertouInimigo = false;

    
    private void OnTriggerEnter(Collider other)
    {
       
        if (other.CompareTag("Enemy"))
        {
            acertouInimigo = true;
            Debug.Log("O soco acertou!");

           
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            acertouInimigo = false;
        }
    }
}
