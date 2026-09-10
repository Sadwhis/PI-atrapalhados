using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuPause : MonoBehaviour
{
    public GameObject _MenuPause;

    private bool paused = false;

    void Start()
    {
        HideMenuPause();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused)
            {
                HideMenuPause();
            }
            else
            {
                ShowMenuPause();
            }
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        
    }

    private void ShowMenuPause()
    {
        paused = true;

        if (_MenuPause != null)
        {
            _MenuPause.SetActive(true);
        }

        Time.timeScale = 0f;
    }

    private void HideMenuPause()
    {
        paused = false;

        if (_MenuPause != null)
        {
            _MenuPause.SetActive(false);
        }

        Time.timeScale = 1f;
    }

    // Botão Continuar
    public void BTN_Resume()
    {
        HideMenuPause();
    }

    // Botão Sair
    public void BTN_Quit()
    {
        Time.timeScale = 1f;

        Application.Quit();


#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#endif
    }
    
}
