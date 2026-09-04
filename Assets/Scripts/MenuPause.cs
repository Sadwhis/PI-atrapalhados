using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPause : MonoBehaviour
{
    public GameObject painel_do_menu;
    
    void Start()
    {
        painel_do_menu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            PausaJogo();
        }
    }
    private void PausaJogo()
    {
        if (Time.timeScale ==1)
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
    public void VoltarMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("1-Tela_Inicial");
    }
}
