using Atrapalhados;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(FPController))]
[RequireComponent(typeof(Player))]
public class ItemMarshmellow : Player
{
    [Header("Components")]
    [SerializeField] FPController _FPCtroller;
    [SerializeField] Player _PlayerCompo;
    public bool click;
    
    

    void Start()
    {
       


    }

    
    void Update()
    {
        
    }


    public void JumpMarshmellow(InputValue value)
    {
        

       if (Player.OnJump() = value.isPressed)
       {
            if (value.isPressed)
            {
                _FPCtroller.TryJump();
            }


       } 
         
    }


}