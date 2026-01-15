using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int moedas;

    private void Awake()
    {
        instance = this;
    }

    public void ColetarItem(int tipo, int valor)
    {
        if (tipo == 1) 
        {
            moedas += valor;
            Debug.Log("Moedas: " + moedas);
        }
    }
}
