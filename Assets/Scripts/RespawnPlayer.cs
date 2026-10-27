using UnityEngine;
using UnityEngine.SceneManagement;

public class RespawnPlayer : MonoBehaviour
{
    [Header("Posição inicial da fase")]
    [SerializeField] private Transform InitialPosition;
    [SerializeField] private Transform pontoRespawn;
    private void Start()
    {
        Respawn();
    }

    public void Respawn()
    {
        string cena = SceneManager.GetActiveScene().name;
        string chave = "Checkpoint_" + cena;

        // Verifica se existe checkpoint salvo
        if (PlayerPrefs.GetInt(chave + "_Salvo", 0) == 1)
        {
            float x = PlayerPrefs.GetFloat(chave + "_X");
            float y = PlayerPrefs.GetFloat(chave + "_Y");
            float z = PlayerPrefs.GetFloat(chave + "_Z");

            // Reposiciona o jogador
            transform.position = new Vector3(x, y, z);
        }
        else if (InitialPosition != null)
        {
            // Sem checkpoint: posição inicial
            transform.position = InitialPosition.position;
        }

        // Zera a velocidade, se houver Rigidbody
        if (TryGetComponent(out Rigidbody rb))
        {
            rb.linearVelocity = Vector3.zero;
        }
    }
}
