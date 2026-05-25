using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class ItemPrimarios : MonoBehaviour
{

    [SerializeField] int _ItemAtivo;
    [SerializeField] GameObject _Player;
    public float _marshMelow;
    
    
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    public void Marshmellow()
    {
        
        if (_ItemAtivo > 0)
        {
           
            if ()
            {   
                if (_Player.CompareTag("Player"))
                {

                    _marshMelow = 4f;


                }

            }
        }



    }
}
