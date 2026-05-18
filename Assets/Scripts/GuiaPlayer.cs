using UnityEngine;

public class GuiaPlayer : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Transform npcIni;
    [SerializeField] Transform general;

    private LineRenderer lineRenderer;
    private Transform targetAtual;

    public float distanciaDoPlayer = 1.2f;
    public float tamanhoDaLinha = 0.8f;
    public float alturaLinha = 0.5f;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        targetAtual = npcIni; 
    }

   
    void LateUpdate()
    {
        if (player != null && targetAtual != null)
        {
            AtualizarSeta();
        }
    }

    void AtualizarSeta()
    {
        Vector3 posPlayer = player.position;
        Vector3 posAlvo = targetAtual.position;

      
        posAlvo.y = posPlayer.y;

        Vector3 direcao = (posAlvo - posPlayer).normalized;

      
        if (direcao.sqrMagnitude < 0.01f) return;

        Vector3 pontoInicial = posPlayer + (direcao * distanciaDoPlayer);
        pontoInicial.y += alturaLinha;

        Vector3 pontoFinal = pontoInicial + (direcao * tamanhoDaLinha);

        lineRenderer.SetPosition(0, pontoInicial);
        lineRenderer.SetPosition(1, pontoFinal);
    }

    public void IrParaNpcIni() => targetAtual = npcIni;
    public void NpcGeneral() => targetAtual = general;
}