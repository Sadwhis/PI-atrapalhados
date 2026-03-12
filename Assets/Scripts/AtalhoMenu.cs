using UnityEngine;
using UnityEngine.SceneManagement;

public class AtalhoMenu : MonoBehaviour
{
    [Header("Nome da Cena do Menu")]
    [Tooltip("Digite o nome exato da sua cena de Menu Inicial")]
    public string nomeDaCenaMenu = "Tela_Inicial";

    void Update()
    {
       
        if ((Input.GetKeyDown(KeyCode.B)))
        {
            Debug.Log("Voltando ao menu e liberando o mouse!");

            // 1. Libera o mouse para ele não ficar preso no meio da tela
            Cursor.lockState = CursorLockMode.None;

            // 2. Torna a setinha do mouse visível novamente
            Cursor.visible = true;

            // Carrega a cena do menu
            SceneManager.LoadScene(nomeDaCenaMenu);
        }
    }
}
