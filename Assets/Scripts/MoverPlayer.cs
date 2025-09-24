using DG.Tweening;
//using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoverPlayer : MonoBehaviour
{
    SpriteRenderer _sp;
    bool _checkHit;
    [SerializeField] float _tempoInicial;
    [SerializeField] bool _rodado;
    [SerializeField] float _tempoRestante;
    GameControl _gameControl;
    Rigidbody2D _rb;
    Vector2 _moveInput;
    [SerializeField] float _speed;
    [SerializeField] float _forceJump;
    [SerializeField] bool _checkGround;
    [SerializeField] int _numbSort;
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _sp = GetComponent<SpriteRenderer>();
        _gameControl = GameObject.FindWithTag("GameController").GetComponent<GameControl>();
        _tempoRestante = _tempoInicial;
    }

    // Update is called once per frame
    void Update()
    {
        if (_rodado)
        {
            _tempoRestante -= Time.deltaTime;
            if(_tempoRestante <= 0f)
            {
                _tempoRestante = _tempoInicial;
                _sp.DOColor(Color.white, 0.25f);
                _checkHit = false;
                _rodado = false;

            }
        }
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
            if(groundJump._numbcor == _numbSort || groundJump._numbcor==0) 
            {
                Jump();
              Debug.Log(_numbSort);

                _numbSort = Random.Range(1, 5);
                _gameControl._menuControl.CorPulo(_numbSort);
            }
           
        }
        if (collision.gameObject.CompareTag("FimGame"))
        {
            _gameControl._gameStay = false;
            _gameControl._fimGame = true;

            _rb.bodyType = RigidbodyType2D.Kinematic;
            _rb.linearVelocity = new Vector2(0, 0);

            _gameControl._panelFimGame.gameObject.SetActive(true);
            _gameControl._panelFimGame.transform.localScale = Vector3.one;

            _gameControl.GameStay(false);
        }
    }

    private void FixedUpdate()
    {
       
        
            _rb.linearVelocity = new Vector2(_moveInput.x * _speed, _rb.linearVelocity.y);
        
    }
    void Jump()
    {
        if (_rb.linearVelocityY <= 0)
        {
            _rb.linearVelocityY = 0;
            _rb.AddForceY(_forceJump);
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("HitPlayer") && _checkHit==false)
        {
            Debug.Log("Hit");
            _gameControl._hudControl.HitSlider();
            _sp.DOColor(Color.red, 0.25f);
            _checkHit = true;
            _rodado = true;
        }
    }
}
