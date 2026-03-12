using UnityEngine;
using System.Collections.Generic;

public class GeradorInimigos : ObjectPool
{
    [Header("Locais de Spawn")]
    [SerializeField] List<Transform> _pontosDeSpawn;

 
    private float _frequenciaAtual = 0f;
    private float _timerSpawn = 0f;
    private bool _podeSpawnar = false;
    private Princesa _princesa; // Para verificar o pause

    public override void Start()
    {
        base.Start();
        _princesa = GameObject.FindWithTag("Player").GetComponent<Princesa>();
    }

    void Update()
    {
        
        if (_podeSpawnar && _frequenciaAtual > 0 && !_princesa._pauseGame)
        {
            _timerSpawn += Time.deltaTime;

            if (_timerSpawn >= _frequenciaAtual)
            {
                SpawnarInimigo();
                _timerSpawn = 0f;
            }
        }
    }

  
    public void ConfigurarSpawn(bool ativar, float tempoEntreSpawns = 0f)
    {
        _podeSpawnar = ativar;
        _frequenciaAtual = tempoEntreSpawns;
        _timerSpawn = 0f;
    }

    private void SpawnarInimigo()
    {
        if (_pontosDeSpawn == null || _pontosDeSpawn.Count == 0) return;

        int indexAleatorio = Random.Range(0, _pontosDeSpawn.Count);
        Transform pontoEscolhido = _pontosDeSpawn[indexAleatorio];

        GameObject inimigoObj = GetPooledObject(); 

        if (inimigoObj != null)
        {
            inimigoObj.transform.position = pontoEscolhido.position;

            InimigoDoce scriptInimigo = inimigoObj.GetComponent<InimigoDoce>();
            if (scriptInimigo != null)
            {
                bool deveIrParaDireita = pontoEscolhido.position.x < 0;
                scriptInimigo.Configurar(deveIrParaDireita);
            }

            inimigoObj.SetActive(true);
        }
    }
}