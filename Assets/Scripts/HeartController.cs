using UnityEngine;

public class HeartController : MonoBehaviour
{

    public int vida;
    public int vidaMaxima;

    public GameObject[] coracoesCheios;
    
    public GameObject[] coracoesVazioObj;

    void Update()
    {
        HealthLogic();
    }

    void HealthLogic()
    {
        
        if (vida > vidaMaxima) vida = vidaMaxima;
        if (vida < 0) vida = 0;

        for (int i = 0; i < coracoesCheios.Length; i++)
        {
            
            if (i < vidaMaxima)
            {
               
                if (i < vida)
                {
                    coracoesCheios[i].SetActive(true);
                    coracoesVazioObj[i].SetActive(false);
                }
                else
                {
                   
                    coracoesCheios[i].SetActive(false);
                    coracoesVazioObj[i].SetActive(true);
                }
            }
            else
            {
                coracoesCheios[i].SetActive(false);
                coracoesVazioObj[i].SetActive(false);
            }
        }
    }
}