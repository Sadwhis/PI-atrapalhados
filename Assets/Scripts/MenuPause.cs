using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public class MenuPause : MonoBehaviour
{
    public GameObject pauseMenu;

    private bool paused = false;

    void Start()
    {
        HidePauseMenu();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused)
                HidePauseMenu();
            else
                ShowPauseMenu();
        }
        
    }

    private void ShowPauseMenu()
    {
        paused = true;
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    private void HidePauseMenu()
    {
        paused = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void BTN_Resume()
    {
        HidePauseMenu();
    }

    public void BTN_Quit()
    {
        Application.Quit();

#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#endif
    }

}
