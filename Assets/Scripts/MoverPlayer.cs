using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoverPlayer : MonoBehaviour
{
    Rigidbody2D _rb;
    [SerializeField] Vector2 _moveInput;
    [SerializeField] float _speed;
    [SerializeField] float _forceJump;
    [SerializeField] bool _checkGround;
    [SerializeField] int _numbSort;
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        _rb.linearVelocity = new Vector2(_moveInput.x * _speed, _rb.linearVelocity.y);
    }
    public void SetMove(InputAction.CallbackContext value)
    {
        _moveInput = value.ReadValue<Vector2>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("JumpGround"))
        {
            GroundJumpControl groundJump = collision.gameObject.GetComponent<GroundJumpControl>();
            if(groundJump._numbcor == 1 || groundJump._numbcor==0) 
            {
                Jump();
              Debug.Log("Pular");

                _numbSort = Random.Range(1, 5);
            }
           
        }
    }
    void Jump()
    {
        _rb.linearVelocityY = 0;
        _rb.AddForceY(_forceJump);
    }
}
