using UnityEngine;
using UnityEngine.InputSystem;

public class MOVEWATER : MonoBehaviour
{

    [SerializeField] Rigidbody2D _RB;
    [SerializeField] float _velocidade=5;
     Vector2 _moveInput;
    GameContolerMJ1 _gameContolerMJ1;


    void Start()
    {

       
        _RB = GetComponent<Rigidbody2D>();
        _gameContolerMJ1 = GameObject.FindWithTag("GameController").GetComponent<GameContolerMJ1>();

    }

    
    void Update()
    {
        _velocidade = 5;
         _RB.linearVelocity = new Vector2(_moveInput.x * _velocidade, _moveInput.y * _velocidade);
    }


    public void SETMOVE(InputAction.CallbackContext value)
    {
        _moveInput = value.ReadValue<Vector2>();
       
    }
}
