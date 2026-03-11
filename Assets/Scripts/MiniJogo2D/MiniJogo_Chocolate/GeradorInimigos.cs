using UnityEngine;
using System.Collections.Generic;

public class GeradorInimigos : ObjectPool
{
    [Header("Locais de Spawn")]
    [SerializeField] List<Transform> _pontosDeSpawn;

    // Removemos o _tempoEntreSpawns daqui, pois o Gerenciador de Fases vai controlar isso.
    // private float _tempoEntreSpawns = 2f; 
    // private float _timer;

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
        // Só tenta spawnar se estiver permitido, a frequência for maior que zero e o jogo não estiver pausado
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

    // Método que o GerenciadorFases vai chamar para ligar/desligar e configurar a velocidade
    public void ConfigurarSpawn(bool ativar, float tempoEntreSpawns = 0f)
    {
        _podeSpawnar = ativar;
        _frequenciaAtual = tempoEntreSpawns;
        _timerSpawn = 0f; // Reseta o timer ao mudar de fase
    }

    private void SpawnarInimigo()
    {
        if (_pontosDeSpawn == null || _pontosDeSpawn.Count == 0) return;

        int indexAleatorio = Random.Range(0, _pontosDeSpawn.Count);
        Transform pontoEscolhido = _pontosDeSpawn[indexAleatorio];

        GameObject inimigoObj = GetPooledObject(); // Lembre-se de usar o método correto da sua classe ObjectPool

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