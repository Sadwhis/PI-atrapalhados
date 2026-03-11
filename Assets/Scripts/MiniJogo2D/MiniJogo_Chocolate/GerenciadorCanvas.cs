using UnityEngine;
using UnityEngine.SceneManagement;

public class GerenciadorCanvas : MonoBehaviour
{
    [Header("Paineis")]
    [SerializeField] private GameObject _painelInicial;
    [SerializeField] private GameObject _painelMenu;

    private Princesa _princesa;
    private GerenciadorFases fases;

    void Start()
    {
        _princesa = GameObject.FindWithTag("Player").GetComponent<Princesa>();
        fases = GameObject.FindWithTag("fases").GetComponent<GerenciadorFases>();

        _painelInicial.SetActive(true);
        _painelMenu.SetActive(false);
        _princesa.SetRigidBody2D(true); // Garante que a Princesa comece congelada
    }

    public void BotaoStart()
    {
        _painelInicial.SetActive(false);
        fases.IniciarJogo(); // Substitua a linha antiga por essa!
    }

    public void AbrirMenu()
    {
        _painelMenu.SetActive(true);
        _princesa.SetRigidBody2D(true); // Pausa o jogo
    }

    public void Continuar()
    {
        _painelMenu.SetActive(false);
        _princesa.SetRigidBody2D(false); // Retoma o jogo
    }

    public void Sair()
    {
        // Se estiver no Editor da Unity
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    public void ReiniciarCena()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}