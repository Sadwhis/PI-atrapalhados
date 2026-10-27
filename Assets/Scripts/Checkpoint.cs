using UnityEngine;
using UnityEngine.SceneManagement;

public class Checkpoint : MonoBehaviour
{
    [Header("Ponto de renascimento")]
    [SerializeField] private Transform pontoRespawn;

    private void OnTriggerEnter(Collider other)
    {
        GameObject jogador = other.attachedRigidbody != null
            ? other.attachedRigidbody.gameObject
            : other.gameObject;

        if (!jogador.CompareTag("Player"))
            return;

        Vector3 posicao = pontoRespawn != null
            ? pontoRespawn.position
            : transform.position;

        string cena = SceneManager.GetActiveScene().name;
        string chave = "Checkpoint_" + cena;

        PlayerPrefs.SetFloat(chave + "_X", posicao.x);
        PlayerPrefs.SetFloat(chave + "_Y", posicao.y);
        PlayerPrefs.SetFloat(chave + "_Z", posicao.z);

        PlayerPrefs.SetInt(chave + "_Salvo", 1);
        PlayerPrefs.Save();

        Debug.Log("Checkpoint salvo: " + cena);
    }
}
