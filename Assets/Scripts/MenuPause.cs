using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuPause : MonoBehaviour
{
    public GameObject painel_do_menu;
    //public string nomeDaFase;
    void Start()
    {
        painel_do_menu.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseJoga();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            CarregarNovaFase();
        }
    }
    private void PauseJoga()
    {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
            painel_do_menu.SetActive(true);
        }
        else if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
            painel_do_menu.SetActive(false);
        }
    }
    public void Conitinuar()
    {
        PauseJoga();
    }
    public void CarregarNovaFase()
    {
        SceneManager.LoadScene(0);
    }
    public void config()
    {
    Debug.Log("Abrindo configurações");
    }
}
