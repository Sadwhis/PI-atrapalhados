using UnityEngine;
using UnityEngine.SceneManagement;

public class TrocarDeFase : MonoBehaviour
{
    public string nomeDaFase;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            CarregarNovaFase();
        }
    }
    private void CarregarNovaFase()
    {
        SceneManager.LoadScene(nomeDaFase);
    }
}
