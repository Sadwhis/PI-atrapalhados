using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
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
        // Teclado: ESC
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            AlternarPause();
        }

        // Gamepad: botão Start/Menu
        if (Gamepad.current != null && Gamepad.current.startButton.wasPressedThisFrame)
        {
            AlternarPause();
        }
    }

    private void AlternarPause()
    {
        if (paused)
        {
            HideMenuPause();
        }
        else
        {
            ShowMenuPause();
        }

        //Cursor.lockState = CursorLockMode.None;
        //Cursor.visible = true;
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
    public void ClickToHome()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(0);
    }

}
