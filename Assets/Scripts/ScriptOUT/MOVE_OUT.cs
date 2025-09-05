using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MOVE_OUT : MonoBehaviour
{
    [SerializeField] Rigidbody2D _RigBy;
    [SerializeField] Vector2 _Move;
    [SerializeField] int _Velocidade;
    GAMECONTROL_OUT _GC;
    public bool _BD;
    [SerializeField] float _jump;

    [SerializeField] bool _CheckGround;

    void Start()
    {
        _RigBy=GetComponent<Rigidbody2D>();
        _GC = GameObject.FindWithTag("GAMEOUT").GetComponent<GAMECONTROL_OUT>();
       
    }


    void Update()
    {
        _Velocidade = 5;
        _RigBy.linearVelocity = new Vector2(_Move.x * _Velocidade,_RigBy.linearVelocity.y);
        
        ButtonDialogue();
       

    }
    public void Jump(InputAction.CallbackContext value)
    { 
        if (_CheckGround == true)
        {
            _RigBy.AddForce(new Vector2(0, 3), ForceMode2D.Impulse);

        }

    }


    public void SetMove(InputAction.CallbackContext value)
    {
        _Move=value.ReadValue<Vector2>();

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("FALA(NPC)"))
        {
            _GC._Falando.SetActive(true);
            
        }
        if (collision.gameObject.tag == "Ground")
        {
            _CheckGround = true;

        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Ground")
        {
            _CheckGround = false;
        }

    }

    public void ButtonDialogue()
    {
        
        if(_BD == true)
        {
            SceneManager.LoadScene("Sapo-Cururu");

        }

    }
}
