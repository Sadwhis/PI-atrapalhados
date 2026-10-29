using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    private static HashSet<GameObject> portalObjects =
        new HashSet<GameObject>();

    [Header("Modo do Portal")]
    [SerializeField] private bool trocarDeCena = false;

    [Header("Teletransporte na mesma cena")]
    [SerializeField] private Transform destination;

    [Header("Troca de cena")]
    [SerializeField] private string nomeCena;
    [SerializeField] int _numbCena;
    private bool carregando;

    private void OnTriggerEnter(Collider other)
    {
        GameObject jogador = other.attachedRigidbody != null
            ? other.attachedRigidbody.gameObject
            : other.gameObject;

        if (!jogador.CompareTag("Player"))
            return;

        if (portalObjects.Contains(jogador))
            return;

        // Troca de cena
        if (trocarDeCena)
        {

            PlayerPrefs.SetInt("pos", _numbCena);
            
            if (carregando)
                return;

            if (string.IsNullOrEmpty(nomeCena))
                return;

            carregando = true;

            // Não salva a posição do portal
            FadeManager.Instance.TrocarCena(nomeCena);
            return;
        }

        // Teletransporte na mesma cena
        if (destination == null)
            return;

        portalObjects.Add(jogador);

        jogador.transform.position = destination.position;

        if (jogador.TryGetComponent(out Rigidbody rb))
        {
            rb.linearVelocity = Vector3.zero;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        GameObject jogador = other.attachedRigidbody != null
            ? other.attachedRigidbody.gameObject
            : other.gameObject;

        portalObjects.Remove(jogador);
    }

    public void trocarCena(string nomeCenaa)
    {
        if (carregando || string.IsNullOrEmpty(nomeCenaa))
            return;

        carregando = true;
        FadeManager.Instance.TrocarCena(nomeCenaa);
       
    }

}