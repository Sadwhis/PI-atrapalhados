using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.UI;

public class HeartController : MonoBehaviour
{
    public int vida;
    public int vidaMaxima;

    public Image[] coracao;
    public Sprite cheio;
    public Sprite vazio;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HealthLogic();
    }

    void HealthLogic()
    {
        if(vida > vidaMaxima) 
        { 
            vida = vidaMaxima;
        }

        for (int i = 0; i < coracao.Length; i++)
        {
            if (i < vida) 
            {
                coracao[i].sprite = cheio;
            }
            else
            {
                coracao[i].sprite = vazio;
            }

            if (i < vidaMaxima)
            {
                coracao[i].enabled = true;
            }
            else
            {
                coracao[i].enabled = false;
            }
        }
    }
}
