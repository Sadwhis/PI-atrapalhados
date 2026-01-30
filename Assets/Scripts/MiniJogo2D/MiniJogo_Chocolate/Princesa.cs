using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class Princesa : MonoBehaviour
{
    [SerializeField] float _jump;
    
    Rigidbody2D _rb;
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

  
}
