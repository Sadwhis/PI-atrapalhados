using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MOVE_OUT : MonoBehaviour
{
    [SerializeField] Rigidbody2D _RigBy;
    [SerializeField] Vector2 _Move;
    [SerializeField] int _Velocidade;
    GameControl _GC;
    public bool _BD;
    

    void Start()
    {
        _RigBy=GetComponent<Rigidbody2D>();
        _GC = GameObject.FindWithTag("GameController").GetComponent<GameControl>();
        
    }

    
    void Update()
    {
        _Velocidade = 5;
        _RigBy.linearVelocity = new Vector2(_Move.x * _Velocidade, _Move.y * _Velocidade);
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
       

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        
    }

    public void ButtonDialogue(bool Active)
    {
        
        if(_BD == true)
        {
            SceneManager.LoadScene("Sapo-Cururu");

        }

    }
}
