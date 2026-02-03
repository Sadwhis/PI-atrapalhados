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
            Spawnar();
            _timer = 0f;
        }
    }

    void Spawnar()
    {
        GameObject inimigo = GetPooledObject();

        if (_pontosDeSpawn.Count > 0)
        {
           
            int Sorteado = Random.Range(0, _pontosDeSpawn.Count);
            Transform pontoSorteado = _pontosDeSpawn[Sorteado];

            inimigo.transform.position = pontoSorteado.position;
            inimigo.transform.rotation = Quaternion.identity;
            inimigo.SetActive(true);

            bool irParaDireita = pontoSorteado.position.x < 0;

            InimigoDoce scriptInimigo = inimigo.GetComponent<InimigoDoce>();
            if (scriptInimigo != null)
            {
                scriptInimigo.Configurar(irParaDireita);
            }
        }
    }
}