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
       
        vida = vidaMaxima;
        AtualizarInterface();
    }

   
    public void TomarDano(int quantidade)
    {
        if (vida <= 0) return; 

        vida -= quantidade;

        if (AudioPlayer.Instance != null)
        {
            AudioPlayer.Instance.SomStart(1); 
        }
        AtualizarInterface();
        if (vida <= 0)
        {
            Morrer();
        }
    }

    void Morrer()
    {
        Debug.Log("O player faleceu");
       
        AudioPlayer.Instance.SomStart(2);

       
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