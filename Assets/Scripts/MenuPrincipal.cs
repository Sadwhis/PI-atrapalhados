using UnityEngine;
using UnityEngine.SceneManagement; // Essencial para trocar de cena!

public class MenuPrincipal : MonoBehaviour
{
    [Header("Nome da fase que vai abrir")]
    public string nomeDaCenaDoJogo = "1_Main";

    // Coloque essa função no evento OnClick do botão START/INICIAR
    public void IniciarJogo()
    {
        // Carrega a cena do seu jogo
        SceneManager.LoadScene(nomeDaCenaDoJogo);
    }

    // Coloque essa função no evento OnClick do botão QUIT/SAIR
    public void SairDoJogo()
    {
        Debug.Log("Fechando o jogo..."); // Mostra um aviso no console para você saber que clicou

        // Isso fecha o jogo quando ele estiver instalado no celular ou PC
        Application.Quit();

        // Isso faz o botão funcionar até mesmo dentro do Unity Editor para você testar
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}