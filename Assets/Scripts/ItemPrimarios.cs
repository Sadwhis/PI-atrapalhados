using Atrapalhados;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.UIElements;

[RequireComponent(typeof(Player))]
public class ItemPrimarios : MonoBehaviour
{
    [Header("Items")]
    [SerializeField] Player Player;


    [SerializeField] int _ItemAtivo;
    public GameObject _pplayer;
    public float _marshMelow;
    string returnValue = "OnJump()";
    
    void Start()
    {
       
    }

    
    void Update()
    {
        
    }

    public void Marshmellow(InputValue value)
    {
        
        if (_ItemAtivo > 0)
        {
           
            Player.OnJump(value);


        }



    }
}
