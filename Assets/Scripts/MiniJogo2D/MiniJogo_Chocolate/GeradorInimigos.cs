using UnityEngine;
using System.Collections.Generic; 

public class GeradorInimigos : ObjectPool
{
    [Header("Locais de Spawn")]
    [SerializeField] List<Transform> _pontosDeSpawn;

    [SerializeField] float _tempoEntreSpawns = 2f;

    private float _timer;

    public override void Start()
    {
        base.Start(); 
    }

    void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= _tempoEntreSpawns)
        {
            
            _timer = 0f;
        }
    }

    
    
}