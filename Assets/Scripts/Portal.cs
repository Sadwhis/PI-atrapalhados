using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private string nomeCena;

    bool carregando;

    private void OnTriggerEnter(Collider other)
    {
        if (carregando) return;

        if (other.CompareTag("Player"))
        {
            carregando = true;

            FadeManager.Instance.TrocarCena(nomeCena);
        }
    }
}