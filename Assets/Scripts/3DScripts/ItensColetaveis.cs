using UnityEngine;

public class ItensColetaveis : MonoBehaviour
{
    [Header("Tipo do Item")]
    public int tipo; 
    public int valor;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.ColetarItem(tipo, valor);
            Destroy(gameObject);
        }
    }
}

