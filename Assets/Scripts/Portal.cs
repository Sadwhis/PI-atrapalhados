using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [SerializeField] private string nomeCena;
    [SerializeField] private Transform InitialPosition;
    private bool carregando;

    private void OnTriggerEnter(Collider other)
    {
        if (carregando) return;

        if (other.CompareTag("Player"))
        {
            carregando = true;

            // Salva a posição do jogador na cena atual
            SalvarPosicao(other.transform);

            // Faz a transição usando seu FadeManager
            FadeManager.Instance.TrocarCena(nomeCena);
        }
    }

    private void SalvarPosicao(Transform jogador)
    {
        string cenaAtual = SceneManager.GetActiveScene().name;
        string chave = "PortalPos_" + cenaAtual;

        PlayerPrefs.SetFloat(chave + "_X", jogador.position.x);
        PlayerPrefs.SetFloat(chave + "_Y", jogador.position.y);
        PlayerPrefs.SetFloat(chave + "_Z", jogador.position.z);

        PlayerPrefs.SetInt(chave + "_Salvo", 1);

        PlayerPrefs.Save();
    }

    public void trocarCena(string nomeCenaa)
    {
        FadeManager.Instance.TrocarCena(nomeCenaa);
    }

    private void Respawn()
    {
        string cenaAtual = SceneManager.GetActiveScene().name;
        string chave = "PortalPos_" + cenaAtual;

        // Verifica se existe uma posição salva pelo portal
        if (PlayerPrefs.GetInt(chave + "_Salvo", 0) == 1)
        {
            float x = PlayerPrefs.GetFloat(chave + "_X");
            float y = PlayerPrefs.GetFloat(chave + "_Y");
            float z = PlayerPrefs.GetFloat(chave + "_Z");

            transform.position = new Vector3(x, y, z);

            // Remove somente o salvamento de chegada desta cena
            PlayerPrefs.DeleteKey(chave + "_Salvo");
            PlayerPrefs.DeleteKey(chave + "_X");
            PlayerPrefs.DeleteKey(chave + "_Y");
            PlayerPrefs.DeleteKey(chave + "_Z");

            PlayerPrefs.Save();
            return;
        }

        // Se não veio de um portal, mantém o respawn antigo
        float checkpointX = PlayerPrefs.GetFloat("checkpointX");
        float checkpointY = PlayerPrefs.GetFloat("checkpointY");

        if (checkpointX != 0 || checkpointY != 0)
        {
            transform.position = new Vector3(
                checkpointX,
                checkpointY,
                transform.position.z
            );
        }
        else if (InitialPosition != null)
        {
            transform.position = InitialPosition.position;
        }
    }
}