using UnityEngine;
using UnityEngine.SceneManagement;

public class GerenciadorCanvas : MonoBehaviour
{
    [Header("Paineis")]
    [SerializeField] private GameObject _painelInicial;
    [SerializeField] private GameObject _painelMenu;
    [SerializeField] private GameObject _painelFimDeJogo;

    private Princesa _princesa;
    private GerenciadorFases fases;

    void Start()
    {
        _princesa = GameObject.FindWithTag("Player").GetComponent<Princesa>();
        fases = GameObject.FindWithTag("fases").GetComponent<GerenciadorFases>();

        _painelInicial.SetActive(true);
        _painelMenu.SetActive(false);
        _princesa.SetRigidBody2D(true);
        _painelFimDeJogo.SetActive(false);
    }

    public void BotaoStart()
    {
        _painelInicial.SetActive(false);
        
            fases.IniciarJogo(); 
    }

    public void AbrirMenu()
    {
        _painelMenu.SetActive(true);
        _princesa.SetRigidBody2D(true); 
    }

    public void Continuar()
    {
        _painelMenu.SetActive(false);
        _princesa.SetRigidBody2D(false); 
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

    public void MostrarFimDeJogo()
    {
        _painelFimDeJogo.SetActive(true); 
        _princesa.SetRigidBody2D(true);   
    }

    public void VoltarParaMenuPrincipal()
    {
        
        SceneManager.LoadScene("Tela_Inicial");
    }
}