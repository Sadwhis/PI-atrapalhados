using Atrapalhados;
using UnityEditor.U2D.Animation;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovePlataforma : MonoBehaviour
{

    [SerializeField] float _velocidade;

     Vector2 _move;
    void Start()
    {
        
    }

    
    void Update()
    {
        Move();
    }

    void Move() 
    {
        Vector3 movimento = new Vector3(_move.x,_move.y,0) * _velocidade * Time.deltaTime;

        transform.Translate(movimento, Space.World);
    }

    void OnPlataforma(InputValue value)
    {
        _move = value.Get<Vector2>();
    }
}
