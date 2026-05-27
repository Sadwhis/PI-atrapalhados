using UnityEngine;

namespace Atrapalhados
{
    public class AnimationBridge : MonoBehaviour
    {
        private FPController _controller;

        void Start()
        {
            
            _controller = GetComponentInParent<FPController>();
        }

        
        
    }
}