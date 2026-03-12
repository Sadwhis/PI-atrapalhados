using UnityEngine;

public class ItensColetaveis : MonoBehaviour
{
    [Header("Tipo do Item")]
    public int tipo;
    public int valor;

    [Header("Efeitos ao Pegar")]
    public AudioClip somColetar;      
    public GameObject particulaEfeito; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
           
            GameManager.instance.ColetarItem(tipo, valor);

            
            if (particulaEfeito != null)
            {
                Instantiate(particulaEfeito, transform.position, Quaternion.identity);
            }

           
            if (somColetar != null)
            {
                AudioSource.PlayClipAtPoint(somColetar, transform.position);
            }

            // 4. Destrói a moeda
            Destroy(gameObject);
        }
    }
}