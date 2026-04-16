using UnityEngine;

public class HeartController : MonoBehaviour
{
    [Header("Configurações de Vida")]
    public int vida;
    public int vidaMaxima;

    [Header("Arrays de Objetos (UI)")]
    public GameObject[] coracoesCheios;
    public GameObject[] coracoesVazioObj;

    void Start()
    {
        // Inicia o jogo com vida cheia se preferir
        vida = vidaMaxima;
        AtualizarInterface();
    }

    // ESTA É A FUNÇÃO DE TOMAR DANO
    public void TomarDano(int quantidade)
    {
        if (vida <= 0) return; // Se já morreu, não faz nada

        vida -= quantidade;

        // 1. Toca o som de Hit (usando o sistema que criamos antes)
        if (AudioPlayer.Instance != null)
        {
            AudioPlayer.Instance.SomStart(1); // 1 = Som de Hit no seu ScriptableObject
        }

        // 2. Atualiza os corações na tela
        AtualizarInterface();

        // 3. Verifica se o player morreu
        if (vida <= 0)
        {
            Morrer();
        }
    }

    void Morrer()
    {
        Debug.Log("O Player morreu!");
        // Toca som de morte
        AudioPlayer.Instance.SomStart(2);

        // Aqui você pode carregar o Game Over ou reiniciar a fase
    }

    public void AtualizarInterface()
    {
        for (int i = 0; i < coracoesCheios.Length; i++)
        {
            if (i < vidaMaxima)
            {
                bool estaAtivo = (i < vida);
                coracoesCheios[i].SetActive(estaAtivo);
                coracoesVazioObj[i].SetActive(!estaAtivo);
            }
            else
            {
                coracoesCheios[i].SetActive(false);
                coracoesVazioObj[i].SetActive(false);
            }
        }
    }
}