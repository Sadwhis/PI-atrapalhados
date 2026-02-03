using UnityEngine;

public class INIMIGOSPAI : InimigoPool
{

    public static INIMIGOSPAI _InimigosPai;

    [SerializeField] int _Vida;
    [SerializeField] float _Velocidade;
    [SerializeField] Rigidbody _RB;

    public override void Awake()
    {
        _RB = GetComponent<Rigidbody>();


    }
        

    
    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        
    }
}
