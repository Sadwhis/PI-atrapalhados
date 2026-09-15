using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;
    public bool MenuPause { get; private set; }
    private PlayerInput _playerInput;
    private InputAction _MenuPause;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        _playerInput = GetComponent<PlayerInput>();
        _MenuPause = _playerInput.actions["MenuPause"];
    }
    private void Update()
    {
        MenuPause = _MenuPause.WasPressedThisFrame();
    }
}
